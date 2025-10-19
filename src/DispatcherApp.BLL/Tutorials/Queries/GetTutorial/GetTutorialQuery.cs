using DispatcherApp.Common.DTOs.Tutorial;
using MediatR;

namespace DispatcherApp.BLL.Tutorials.Queries.GetTutorial;

public sealed record GetTutorialQuery(int TutorialId) : IRequest<TutorialResponse>;
