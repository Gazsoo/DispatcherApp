using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using MediatR;

namespace DispatcherApp.BLL.Tutorials.Commands.DeleteTutorial;

public sealed class DeleteTutorialCommandHandler : IRequestHandler<DeleteTutorialCommand, Unit>
{
    private readonly ITutorialService _tutorialService;

    public DeleteTutorialCommandHandler(ITutorialService tutorialService)
    {
        _tutorialService = tutorialService;
    }

    public async Task<Unit> Handle(DeleteTutorialCommand request, CancellationToken cancellationToken)
    {
        await _tutorialService.DeleteTutorialAsync(request.TutorialId, cancellationToken);
        return Unit.Value;
    }
}
