using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.Common.DTOs.Tutorial;
using DispatcherApp.Common.Entities;

namespace DispatcherApp.BLL.Common.Interfaces;

public interface ITutorialService
{
    Task<Tutorial> GetTutorialAsync(int tutorialId, CancellationToken ct = default);
    Task<List<Tutorial>> GetTutorialListAsync(CancellationToken ct = default);
    Task<Tutorial> CreateTutorial(CreateTutorialRequest request, CancellationToken ct = default);
    Task<Tutorial> UpdateTutorialAsync(int tutorialId, UpdateTutorialRequest request, CancellationToken ct = default);
    Task DeleteTutorialAsync(int tutorialId, CancellationToken ct = default);
}
