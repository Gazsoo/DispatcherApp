using Ardalis.GuardClauses;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.Entities;
using DispatcherApp.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace DispatcherApp.DAL.Repositories;
public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;

    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(DispatcherSession session, CancellationToken ct = default)
    {
        await _context.DispatcherSessions.AddAsync(session);
    }

    public Task<int> AddLogFile(int logFileId, string sessionId, CancellationToken ct = default)
    {
        var logFile = _context.Files.FirstOrDefault(x => x.Id == logFileId);
        Guard.Against.NotFound(logFileId, logFile);
        var session = _context.DispatcherSessions
            .Include(s => s.LogFile)
            .FirstOrDefault(s => s.GroupId == sessionId);
        Guard.Against.NotFound(sessionId, session);
        session.LogFile = logFile;
        _context.SaveChanges();
        return Task.FromResult(logFile.Id);

    }

    public async Task<DispatcherSession> AddParticipant(SessionParticipant user, string sessionId, CancellationToken ct = default)
    {
        var session =  await _context.DispatcherSessions
            .Include(s => s.Participants)
            .Where(s => s.GroupId == sessionId)
            .FirstOrDefaultAsync();
        Guard.Against.NotFound(sessionId, session);
        session.Participants.Add(user);
        return session;
    }

    public async Task<IEnumerable<DispatcherSession>> GetActiveSessions(CancellationToken ct = default)
    {
        return await _context.DispatcherSessions
            .AsNoTracking()
            .AsSplitQuery()
            .Where(s => s.Status == Common.Constants.DispatcherSessionStatus.Started)
            .Include(a => a.Participants)
                .ThenInclude(p => p.User)
            .Include(a => a.LogFile)
            .ToListAsync();
    }

    public async Task<IEnumerable<DispatcherSession>> GetAll(CancellationToken ct = default)
    {
        return await _context.DispatcherSessions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(a => a.Participants)
            .Include(a => a.LogFile)
            .ToListAsync();
    }

    public async Task<DispatcherSession?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        return await _context.DispatcherSessions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(a => a.Participants)
            .Include(a => a.LogFile)
            .FirstOrDefaultAsync(a => a.GroupId == id);
    }

    public async Task<DispatcherSession?> GetBySessionIdAsync(string id, CancellationToken ct = default)
    {
        return await _context.DispatcherSessions
            //.AsNoTracking()
            .Include(a => a.Participants)
            .ThenInclude(a => a.User)
            .FirstOrDefaultAsync(a => a.GroupId == id);
    }

    public async Task<IEnumerable<DispatcherSession>> GetSessionsByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _context.DispatcherSessions
            .Include(s => s.Participants)
            .Where(s => s.Participants.Any(p => p.UserId == userId))
            .ToListAsync();
    }

    public void Remove(DispatcherSession session)
    {
        _context.DispatcherSessions.Remove(session);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
       return await _context.SaveChangesAsync(ct);
    }

    public void Update(DispatcherSession session)
    {
        _context.DispatcherSessions.Update(session);
    }
}
