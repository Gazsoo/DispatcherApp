using MediatR;

namespace DispatcherApp.BLL.Tutorials.Commands.DeleteTutorial;

public sealed record DeleteTutorialCommand(int TutorialId) : IRequest<Unit>;
