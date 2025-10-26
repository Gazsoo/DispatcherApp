using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.User.Commands.UpdateUser;
internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserInfoResponse>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserInfoResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdateAsync(request.Request, cancellationToken);
    }
}
