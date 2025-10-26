using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Sessions;
using DispatcherApp.BLL.Sessions.Commands;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.DTOs.Session;
using DispatcherApp.Common.Exceptions;
using MediatR;

namespace DispatcherApp.BLL.Common.Services;
public class SessionService (
    ISessionRepository repository,
    IMapper mapper): ISessionService
{
    private readonly ISessionRepository _repo = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<SessionResponse> GetSessionDataAsync(string sessionId, CancellationToken ct)
    {
        // Placeholder implementation
        await Task.Delay(100, ct); // Simulate async work
        return new SessionResponse( );
    }
    public async Task<SessionResponse> UpdateSessionDataAsync(
        UpdateSessionCommand command,
        CancellationToken ct)
    {
        var session = await _repo.GetBySessionIdAsync(command.Id, ct);
        Guard.Against.NotFound(command.Id, nameof(command.Id));

        if (session.Version != command.IfMatchVersion)
            throw new ConcurrencyException("Version mismatch");

        session.Status = command.Status;
        session.Version++;
        session.UpdatedAt = DateTimeOffset.UtcNow;
        var result = await _repo.UpdateAsync(session, ct);
        await _repo.SaveChangesAsync(ct);

        return _mapper.Map<SessionResponse>(result);
    }
    public async Task<IEnumerable<SessionResponse>> ListSessionsAsync(CancellationToken ct)
    {
        // Placeholder implementation
        await Task.Delay(100, ct); // Simulate async work
        return new List<SessionResponse> { new SessionResponse() };
    }

}
