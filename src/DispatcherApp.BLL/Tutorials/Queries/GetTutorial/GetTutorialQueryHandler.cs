using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Models.DTOs.Tutorial;
using MediatR;
using AutoMapper;

namespace DispatcherApp.BLL.Tutorials.Queries.GetTutorial;

public sealed class GetTutorialQueryHandler : IRequestHandler<GetTutorialQuery, TutorialResponse>
{
    private readonly ITutorialService _tutorialService;
    private readonly IMapper _mapper;

    public GetTutorialQueryHandler(ITutorialService tutorialService, IMapper mapper)
    {
        _tutorialService = tutorialService;
        _mapper = mapper;
    }

    public async Task<TutorialResponse> Handle(GetTutorialQuery request, CancellationToken cancellationToken)
    {
        var tutorial = await _tutorialService.GetTutorialAsync(request.TutorialId, cancellationToken);
        return _mapper.Map<TutorialResponse>(tutorial);
    }
}
