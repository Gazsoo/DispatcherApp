using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Tutorial;
using MediatR;

namespace DispatcherApp.BLL.Tutorials.Commands.CreateTutorial;

public sealed class CreateTutorialCommandHandler : IRequestHandler<CreateTutorialCommand, CreateTutorialResponse>
{
    private readonly ITutorialService _tutorialService;
    private readonly IMapper _mapper;

    public CreateTutorialCommandHandler(ITutorialService tutorialService, IMapper mapper)
    {
        _tutorialService = tutorialService;
        _mapper = mapper;
    }

    public async Task<CreateTutorialResponse> Handle(CreateTutorialCommand request, CancellationToken cancellationToken)
    {
        var tutorial = await _tutorialService.CreateTutorial(request.Request, cancellationToken);
        return _mapper.Map<CreateTutorialResponse>(tutorial);
    }
}
