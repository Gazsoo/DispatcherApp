using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Models.DTOs.Tutorial;
using MediatR;

namespace DispatcherApp.BLL.Tutorials.Commands.UpdateTutorial;

public sealed class UpdateTutorialCommandHandler : IRequestHandler<UpdateTutorialCommand, TutorialResponse>
{
    private readonly ITutorialService _tutorialService;
    private readonly IMapper _mapper;

    public UpdateTutorialCommandHandler(ITutorialService tutorialService, IMapper mapper)
    {
        _tutorialService = tutorialService;
        _mapper = mapper;
    }

    public async Task<TutorialResponse> Handle(UpdateTutorialCommand request, CancellationToken cancellationToken)
    {
        var tutorial = await _tutorialService.UpdateTutorialAsync(request.TutorialId, request.Request, cancellationToken);
        return _mapper.Map<TutorialResponse>(tutorial);
    }
}
