using DispatcherApp.Common.DTOs.Tutorial;
using MediatR;

namespace DispatcherApp.BLL.Tutorials.Commands.CreateTutorial;

public sealed record CreateTutorialCommand(CreateTutorialRequest Request) : IRequest<CreateTutorialResponse>;
