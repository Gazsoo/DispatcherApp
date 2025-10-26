using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.DeleteAssignment;

public sealed record DeleteAssignmentCommand(int AssignmentId) : IRequest<Unit>;
