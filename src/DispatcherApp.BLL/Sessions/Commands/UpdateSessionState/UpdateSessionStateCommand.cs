using DispatcherApp.Common.Constants;
using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.UpdateSessionState;
// Include properties to be used as input for the command
public record UpdateSessionStateCommand(string sessionId, DispatcherSessionStatus dss) : IRequest<SessionResponse>;
