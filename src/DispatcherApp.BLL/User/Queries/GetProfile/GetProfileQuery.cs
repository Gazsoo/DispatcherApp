using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.User.Queries.GetProfile;
// Include properties to be used as input for the query
public record GetProfileQuery() : IRequest<UserInfoResponse>;
