using System.Collections.Generic;
using System.Threading;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Tutorials.Commands.CreateTutorial;
using DispatcherApp.BLL.Tutorials.Commands.DeleteTutorial;
using DispatcherApp.BLL.Tutorials.Commands.UpdateTutorial;
using DispatcherApp.BLL.Tutorials.Queries.GetTutorial;
using DispatcherApp.BLL.Tutorials.Queries.GetTutorialList;
using DispatcherApp.Models.DTOs.Tutorial;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TutorialController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{tutorialId}")]
    [ProducesResponseType(typeof(TutorialResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TutorialResponse>> GetTutorial(int tutorialId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetTutorialQuery(tutorialId), cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TutorialResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TutorialResponse>>> GetTutorials(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTutorialListQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateTutorialResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateTutorialResponse>> CreateTutorial([FromBody] CreateTutorialRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateTutorialCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetTutorial), new { tutorialId = result.Id }, result);
    }

    [HttpPut("{tutorialId}")]
    [ProducesResponseType(typeof(TutorialResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TutorialResponse>> UpdateTutorial(int tutorialId, [FromBody] UpdateTutorialRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateTutorialCommand(tutorialId, request), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{tutorialId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTutorial(int tutorialId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTutorialCommand(tutorialId), cancellationToken);
        return NoContent();
    }
}
