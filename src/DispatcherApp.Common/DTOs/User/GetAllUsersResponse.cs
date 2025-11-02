namespace DispatcherApp.Common.DTOs.User;

public record GetAllUsersResponse(IReadOnlyList<UserInfoResponse> Users)
{
}
