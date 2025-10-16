using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Common.Interfaces.Repository;
using DispatcherApp.Models.DTOs.Tutorial;
using DispatcherApp.Models.Entities;

namespace DispatcherApp.BLL.Common.Services;

public class TutorialService : ITutorialService
{
    private readonly ITutorialRepository _tutorialRepository;
    private readonly IMapper _mapper;

    public TutorialService(ITutorialRepository tutorialRepository, IMapper mapper)
    {
        _tutorialRepository = tutorialRepository;
        _mapper = mapper;
    }

    public async Task<Tutorial> GetTutorialAsync(int tutorialId, CancellationToken ct = default)
    {
        var tutorial = await _tutorialRepository.GetByIdAsync(tutorialId, includeFiles: true, ct);
        return Guard.Against.NotFound(tutorialId, tutorial);
    }

    public Task<List<Tutorial>> GetTutorialListAsync(CancellationToken ct = default) =>
        _tutorialRepository.GetAllAsync(includeFiles: true, ct);

    public async Task<Tutorial> CreateTutorial(CreateTutorialRequest request, CancellationToken ct = default)
    {
        var tutorial = _mapper.Map<Tutorial>(request);
        tutorial.CreatedAt = DateTime.UtcNow;
        tutorial.UpdatedAt = tutorial.CreatedAt;

        await _tutorialRepository.AddAsync(tutorial, ct);
        await _tutorialRepository.SaveChangesAsync(ct);

        return tutorial;
    }

    public async Task<Tutorial> UpdateTutorialAsync(int tutorialId, UpdateTutorialRequest request, CancellationToken ct = default)
    {
        var tutorial = await _tutorialRepository.GetByIdAsync(tutorialId, includeFiles: true, ct);
        Guard.Against.NotFound(tutorialId, tutorial);

        tutorial.Title = request.Title;
        tutorial.Description = request.Description ?? tutorial.Description;
        tutorial.Url = request.Url ?? tutorial.Url;
        tutorial.ContentType = request.ContentType ?? tutorial.ContentType;
        tutorial.UpdatedAt = DateTime.UtcNow;

        await _tutorialRepository.SaveChangesAsync(ct);

        return tutorial;
    }

    public async Task DeleteTutorialAsync(int tutorialId, CancellationToken ct = default)
    {
        var tutorial = await _tutorialRepository.GetByIdAsync(tutorialId, includeFiles: false, ct);
        Guard.Against.NotFound(tutorialId, tutorial);

        await _tutorialRepository.RemoveAsync(tutorial, ct);
        await _tutorialRepository.SaveChangesAsync(ct);
    }
}
