using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Models.DTOs.Tutorial;
using MediatR;

namespace DispatcherApp.BLL.Tutorials.Queries.GetTutorialList;

public sealed class GetTutorialListQueryHandler : IRequestHandler<GetTutorialListQuery, List<TutorialResponse>>
{
    private readonly ITutorialService _tutorialService;
    private readonly IMapper _mapper;

    public GetTutorialListQueryHandler(ITutorialService tutorialService, IMapper mapper)
    {
        _tutorialService = tutorialService;
        _mapper = mapper;
    }

    public async Task<List<TutorialResponse>> Handle(GetTutorialListQuery request, CancellationToken cancellationToken)
    {
        var tutorials = await _tutorialService.GetTutorialListAsync(cancellationToken);
        return _mapper.Map<List<TutorialResponse>>(tutorials);
    }
}
