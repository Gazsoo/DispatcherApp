using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.User.Queries.GetUser;
// Include properties to be used as input for the query
public record GetUserQuery(string Id) : IRequest<UserInfoResponse>;
