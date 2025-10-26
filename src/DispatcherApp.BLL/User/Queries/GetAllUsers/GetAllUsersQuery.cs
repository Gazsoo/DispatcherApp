using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.User.Queries.GetAllUsers;
// Include properties to be used as input for the query
public record GetAllUsersQuery() : IRequest<GetAllUsersResponse>;
