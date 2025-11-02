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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        Guard.Against.NotFound(command.Id, session);

        session.Status = command.Status;

        session = await SaveSessionUpdateAsync(session, ct);
        return await SendSessionUpdateAsync(session, ct);
    }
    public async Task<SessionResponse> UpdateSessionStatusAsync(
        string sessionId,
        DispatcherSessionStatus status,
        CancellationToken ct = default)
    {
        var session = await _sessionRepo.GetBySessionIdAsync(sessionId);
        Guard.Against.NotFound(sessionId, session);

        session.Status = status;

        session = await SaveSessionUpdateAsync(session, ct);
        return await SendSessionUpdateAsync(session, ct);
    }

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

        if (session.Participants.Where(p => p.UserId == currentUserId).Any())
        {
            return _mapper.Map<SessionResponse>(session);
        }

        await _sessionRepo.AddParticipant(new SessionParticipant
        {
            UserId = currentUserId
        }, session.GroupId, ct);
        await SaveSessionUpdateAsync(session, ct);


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
        await SaveSessionUpdateAsync(session, ct);

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
            //_sessionRepo.Update(existingSession);
            await SaveSessionUpdateAsync(existingSession, ct);
            //return existingSession;
        }
    }

    public async Task<IEnumerable<SessionResponse>> ListActiveSessionsAsync(CancellationToken ct)
    {
        var sesssions = await _sessionRepo.GetActiveSessions(ct);
        return _mapper.Map<List<SessionResponse>>(sesssions);
    }

    public async Task LeaveAllUserSessionsAsync(string currentUserId, CancellationToken ct = default)
    {
        IEnumerable<DispatcherSession> existingSession = await _sessionRepo.GetSessionsByUserIdAsync(currentUserId, ct);

        foreach (var sess in existingSession)
        {
            sess.Participants.Remove(
            sess.Participants.FirstOrDefault(p => p.UserId == currentUserId)!);
        }

        await SaveSessionUpdateAsync(existingSession, ct);
    }
    private async Task<DispatcherSession> SaveSessionUpdateAsync(DispatcherSession ds, CancellationToken ct)
    {
        ds.UpdatedAt = _timeProvider.GetUtcNow();
        ds.Version++;
        try
        {
            await _sessionRepo.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("The session was updated by another process.", ex);
        }

        return ds;
    }
    private async Task<IEnumerable<DispatcherSession>> SaveSessionUpdateAsync(IEnumerable<DispatcherSession> dss, CancellationToken ct)
    {
        foreach (var ds in dss)
        {

            ds.UpdatedAt = _timeProvider.GetUtcNow();
            ds.Version++;

        }
        try
        {
            await _sessionRepo.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("The session was updated by another process.", ex);
        }

        return dss;
    }

   
    public async Task SendOutSessionsAcitvityAsync(CancellationToken ct = default)
    {
        var sessionsList = await _sessionRepo.GetActiveSessions(ct);
        await _notifier.BrooadcastSessionsAcitvityAsync(new SessionActivityResponse
        {
            sessions = _mapper.Map<IEnumerable<SessionResponse>>(sessionsList)
        }, ct);
    }
}
