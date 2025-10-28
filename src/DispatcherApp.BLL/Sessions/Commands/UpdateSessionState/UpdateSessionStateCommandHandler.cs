using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.UpdateSessionState;
internal sealed class UpdateSessionStateCommandHandler : IRequestHandler<UpdateSessionStateCommand, SessionResponse>
{
    public Task<SessionResponse> Handle(UpdateSessionStateCommand request, CancellationToken cancellationToken)
    {
        // Implement your logic here
        throw new NotImplementedException();
    }
}
