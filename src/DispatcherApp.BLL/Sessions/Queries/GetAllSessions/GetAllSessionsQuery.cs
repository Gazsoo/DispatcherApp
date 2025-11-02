using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Queries.GetAllSessions;
// Include properties to be used as input for the query
public record GetAllSessionsQuery() : IRequest<IEnumerable<SessionResponse>>;
