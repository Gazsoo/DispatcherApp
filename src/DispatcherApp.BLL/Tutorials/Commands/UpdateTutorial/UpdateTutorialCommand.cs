using DispatcherApp.Models.DTOs.Tutorial;
using MediatR;

namespace DispatcherApp.BLL.Tutorials.Commands.UpdateTutorial;

public sealed record UpdateTutorialCommand(int TutorialId, UpdateTutorialRequest Request) : IRequest<TutorialResponse>;
