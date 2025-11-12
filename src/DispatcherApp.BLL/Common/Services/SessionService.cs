using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Sessions;
using DispatcherApp.BLL.Sessions.Commands.UpdateSession;
using DispatcherApp.Common.Abstractions;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.Constants;
using DispatcherApp.Common.DTOs.Session;
using DispatcherApp.Common.Entities;
using DispatcherApp.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DispatcherApp.BLL.Common.Services;
public class SessionService (
    ISessionNotifier notifier,
    IUserRepository userRepository,
    ISessionRepository repository,
    TimeProvider timeProvider,
    IMapper mapper,
    ILogger<SessionService> logger): ISessionService
{
    private readonly ISessionRepository _sessionRepo = repository;
    private readonly IUserRepository _userRepository= userRepository;
    private readonly ISessionNotifier _notifier = notifier;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<SessionService> _logger = logger;

    public async Task<SessionResponse> GetSessionDataAsync(int sessionId, CancellationToken ct)
    {
        var sesssion = await _sessionRepo.GetByIdAsync(sessionId, ct);
        Guard.Against.NotFound(sessionId, nameof(sessionId));
        return _mapper.Map<SessionResponse>(sesssion);
    }
    public async Task<SessionResponse> UpdateSessionDataAsync(
        UpdateSessionCommand command,
        CancellationToken ct)
        => await ApplySessionUpdateAsync(
            command.Id,
            session =>
            {
                session.Status = command.Status;
                return Task.CompletedTask;
            },
            ct);

    public async Task<SessionResponse> UpdateSessionStatusAsync(
        string sessionId,
        DispatcherSessionStatus status,
        CancellationToken ct = default)
        => await ApplySessionUpdateAsync(
            sessionId,
            session =>
            {
                session.Status = status;
                return Task.CompletedTask;
            },
            ct);

    private async Task<SessionResponse> SendSessionUpdateAsync(DispatcherSession ds, CancellationToken ct)
    {
        
        Guard.Against.Null(ds.GroupId, nameof(ds.GroupId));
        
        var response = _mapper.Map<SessionResponse>(ds);
        await _notifier.BroadcastUpdatedAsync(
            ds.GroupId,
            ds.Version,
            response,
            ct);
        return response;
    }
    public async Task<IEnumerable<SessionResponse>> ListSessionsAsync(CancellationToken ct)
    {
        var sesssions = await _sessionRepo.GetAll(ct);
        return _mapper.Map<List<SessionResponse>>(sesssions);
    }
    public async Task<SessionResponse> JoinOrCreateAsync(string sessionId, string currentUserId, CancellationToken ct = default)
    {
        var session = await GetOrCreate(sessionId, currentUserId, ct);
        Guard.Against.NotFound(sessionId, session.GroupId);

        if (session.Participants.Any(p => p.UserId == currentUserId))
        {
            return _mapper.Map<SessionResponse>(session);
        }

        return await ApplySessionUpdateAsync(
            session.GroupId,
            async s =>
            {
                if (s.Participants.All(p => p.UserId != currentUserId))
                {
                    var user = await _userRepository.GetByIdAsync(currentUserId, ct);
                    s.Participants.Add(new SessionParticipant
                    {
                        User = user,
                        UserId = currentUserId
                    });
                }
            },
            ct);
    }
    public async Task<DispatcherSession> GetOrCreate(string sessionId, string ownerUserId, CancellationToken ct = default)
    {
        if (await _sessionRepo.GetBySessionIdAsync(sessionId, ct) is DispatcherSession existingSession)
        {
            return existingSession;
        }

        var session = new DispatcherSession
        {
            GroupId = Guid.NewGuid().ToString(),
            OwnerId = ownerUserId,
            AssignmentId = null,
            Status = DispatcherApp.Common.Constants.DispatcherSessionStatus.Started,
            StartTime = _timeProvider.GetUtcNow(),
            Participants = new List<SessionParticipant>(),
            UpdatedAt = _timeProvider.GetUtcNow(),
            Version = 1
        };
        await _sessionRepo.AddAsync(session, ct);
        await _sessionRepo.SaveChangesAsync(ct);

        return session;
    }

    public async Task LeaveSessionAsync(string sessionId, string userId, CancellationToken ct = default)
    {
        await ApplySessionUpdateAsync(
            sessionId,
            async session =>
            {
                var participant = session.Participants.FirstOrDefault(p => p.UserId == userId);
                if (participant != null)
                {
                    session.Participants.Remove(participant);
                }

                if (session.Participants.Count == 0)
                {
                    session.Status = DispatcherSessionStatus.Finished;
                }

                await _sessionRepo.SaveChangesAsync();
            },
            ct);
    }

    public async Task<IEnumerable<SessionResponse>> ListActiveSessionsAsync(CancellationToken ct)
    {
        var sesssions = await _sessionRepo.GetActiveSessions(ct);
        return _mapper.Map<List<SessionResponse>>(sesssions);
    }

    public async Task LeaveAllUserSessionsAsync(string currentUserId, CancellationToken ct = default)
    {
        var sessionIds = (await _sessionRepo.GetSessionsByUserIdAsync(currentUserId, ct))
            .Select(s => s.GroupId)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .ToList();

        foreach (var sessionId in sessionIds)
        {
            Guard.Against.NullOrEmpty(sessionId, nameof(sessionId));
            await LeaveSessionAsync(sessionId, currentUserId, ct);
        }
    }

   
    public async Task SendOutSessionsAcitvityAsync(CancellationToken ct = default)
    {
        var sessionsList = await _sessionRepo.GetActiveSessions(ct);
        await _notifier.BrooadcastSessionsAcitvityAsync(new SessionActivityResponse
        {
            sessions = _mapper.Map<IEnumerable<SessionResponse>>(sessionsList)
        }, ct);
    }

    private async Task<SessionResponse> ApplySessionUpdateAsync(
        string sessionId,
        Func<DispatcherSession, Task> mutation,
        CancellationToken ct)
    {
        const int maxAttempts = 2;

        for (var attempt = 0; attempt < maxAttempts; attempt++)
        {
            var session = await _sessionRepo.GetBySessionIdAsync(sessionId, ct);
            Guard.Against.NotFound(sessionId, session);
            _logger.LogDebug("Attempt {Attempt} updating session {SessionId} version {Version}", attempt + 1, sessionId, session.Version);

            await mutation(session);

            session.UpdatedAt = _timeProvider.GetUtcNow();
            session.Version++;

            try
            {
                await _sessionRepo.SaveChangesAsync(ct);
                _logger.LogInformation("Session {SessionId} saved with version {Version}", sessionId, session.Version);
                return await SendSessionUpdateAsync(session, ct);
            }
            catch (DbUpdateConcurrencyException) when (attempt < maxAttempts - 1)
            {
                _logger.LogWarning("Concurrency conflict on session {SessionId} (attempt {Attempt}); retrying", sessionId, attempt + 1);
                // Retry with a fresh read
                continue;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var latest = await _sessionRepo.GetBySessionIdAsync(sessionId, ct);
                if (latest != null && !HasParticipantDifference(latest, session))
                {
                    _logger.LogInformation("Session {SessionId} already updated with equivalent state; returning latest version {Version}", sessionId, latest.Version);
                    return await SendSessionUpdateAsync(latest, ct);
                }

                throw new ConcurrencyException("The session was updated by another process.", ex);
            }
        }

        throw new ConcurrencyException("Unable to persist session due to repeated concurrent updates.");
    }

    private static bool HasParticipantDifference(DispatcherSession latest, DispatcherSession attempted)
    {
        var latestParticipants = latest.Participants.Select(p => p.UserId).OrderBy(id => id).ToArray();
        var attemptedParticipants = attempted.Participants.Select(p => p.UserId).OrderBy(id => id).ToArray();
        if (!latestParticipants.SequenceEqual(attemptedParticipants))
        {
            return true;
        }

        return latest.Status != attempted.Status;
    }
}
