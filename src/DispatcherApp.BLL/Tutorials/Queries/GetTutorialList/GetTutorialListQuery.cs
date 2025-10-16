using System.Collections.Generic;
using DispatcherApp.Models.DTOs.Tutorial;
using MediatR;

namespace DispatcherApp.BLL.Tutorials.Queries.GetTutorialList;

public sealed record GetTutorialListQuery : IRequest<List<TutorialResponse>>;
