using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Queries.GetActiveSessions;
// Include properties to be used as input for the query
public record GetActiveSessionsQuery() : IRequest<IEnumerable<SessionResponse>>;
