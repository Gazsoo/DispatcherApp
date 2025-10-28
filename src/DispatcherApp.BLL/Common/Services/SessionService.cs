using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Sessions;
using DispatcherApp.BLL.Sessions.Commands.UpdateSession;
using DispatcherApp.Common.Abstractions;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.DTOs.Session;
using DispatcherApp.Common.Entities;
using DispatcherApp.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DispatcherApp.BLL.Common.Services;
public class SessionService (
    ISessionNotifier notifier,
    ISessionRepository repository,
    TimeProvider timeProvider,
    IMapper mapper): ISessionService
{
    private readonly ISessionRepository _sessionRepo = repository;
    private readonly ISessionNotifier _notifier = notifier;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IMapper _mapper = mapper;

    public async Task<SessionResponse> GetSessionDataAsync(int sessionId, CancellationToken ct)
    {
        var sesssion = await _sessionRepo.GetByIdAsync(sessionId, ct);
        Guard.Against.NotFound(sessionId, nameof(sessionId));
        return _mapper.Map<SessionResponse>(sesssion);
    }
    public async Task<SessionResponse> UpdateSessionDataAsync(
        UpdateSessionCommand command,
        CancellationToken ct)
    {
        var session = await _sessionRepo.GetBySessionIdAsync(command.Id);
        Guard.Against.Null(session, nameof(session));

        if (session?.Version != command.IfMatchVersion)
            throw new ConcurrencyException("Version mismatch");

        session.Status = command.Status;
        session.Version++;
        session.UpdatedAt = DateTimeOffset.UtcNow;
        //_sessionRepo.Update(session);
        await _sessionRepo.SaveChangesAsync(ct);
        await _notifier.BroadcastUpdatedAsync(command.Id, session.Version, "CUCCOK", ct);
        return _mapper.Map<SessionResponse>(session);
    }
    public async Task<IEnumerable<SessionResponse>> ListSessionsAsync(CancellationToken ct)
    {
        var sesssions = await _sessionRepo.GetAll(ct);
        return _mapper.Map<List<SessionResponse>>(sesssions);
    }
    public async Task<SessionResponse> JoinOrCreateAsync(string sessionId, string ownerUserId, CancellationToken ct = default)
    {
        var session = await GetOrCreate(sessionId, ownerUserId, ct);
        Guard.Against.NotFound(sessionId, session.GroupId);
        
        if (session.Participants.Where(p => p.UserId == ownerUserId).Any())
        {
            return _mapper.Map<SessionResponse>(session);
        }

        await _sessionRepo.AddParticipant(new SessionParticipant
        {
            UserId = ownerUserId
        }, session.GroupId,  ct);
        await _sessionRepo.SaveChangesAsync(ct);
        return _mapper.Map<SessionResponse>(session);
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

    public async Task LeaveSessionAsync(string sessionId, string ownerUserId, CancellationToken ct = default)
    {
        if (await _sessionRepo.GetBySessionIdAsync(sessionId, ct) is DispatcherSession existingSession)
        {
            existingSession.Participants.Remove(
                existingSession.Participants.FirstOrDefault(p => p.UserId == ownerUserId)!);
            if (existingSession.Participants.Count == 0)
            { 
                existingSession.Status = DispatcherApp.Common.Constants.DispatcherSessionStatus.Finished; 
            }
            _sessionRepo.Update(existingSession);
            await _sessionRepo.SaveChangesAsync(ct);
            //return existingSession;
        }
    }

    public async Task<IEnumerable<SessionResponse>> ListActiveSessionsAsync(CancellationToken ct)
    {
        var sesssions = await _sessionRepo.GetActiveSessions(ct);
        return _mapper.Map<List<SessionResponse>>(sesssions);
    }
}
