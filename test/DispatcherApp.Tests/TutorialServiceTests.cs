using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Services;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.DTOs.Tutorial;
using DispatcherApp.Common.Entities;
using Moq;
using File = DispatcherApp.Common.Entities.File;

namespace DispatcherApp.Tests;
public class TutorialServiceTests
{
    private readonly Mock<ITutorialRepository> _tutorialRepoMock;
    private readonly Mock<IFileRepository> _fileRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<TimeProvider> _timeProviderMock;
    private readonly TutorialService _sut;
    public TutorialServiceTests()
    {
        _tutorialRepoMock = new Mock<ITutorialRepository>();
        _fileRepoMock = new Mock<IFileRepository>();
        _mapperMock = new Mock<IMapper>();
        _timeProviderMock = new Mock<TimeProvider>();
        _sut = new TutorialService(
            _tutorialRepoMock.Object,
            _fileRepoMock.Object,
            _mapperMock.Object,
            _timeProviderMock.Object);
    }

    [Fact]
    public async Task GetTutorialAsync_ShouldReturnTutorial_WhenExists()
    {
        // Arrange
        var tutorialId = 1;
        var expectedTutorial = new Tutorial { Id = tutorialId, Title = "Test Tutorial" };

        _tutorialRepoMock.Setup(x => x.GetByIdAsync(tutorialId, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTutorial);

        // Act
        var result = await _sut.GetTutorialAsync(tutorialId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTutorial.Id, result.Id);
        _tutorialRepoMock.Verify(x => x.GetByIdAsync(tutorialId, true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTutorialAsync_ShouldThrowException_WhenNotFound()
    {
        // Arrange
        var tutorialId = 99;
        _tutorialRepoMock.Setup(x => x.GetByIdAsync(tutorialId, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tutorial?)null);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetTutorialAsync(tutorialId));
    }

    [Fact]
    public async Task CreateTutorial_ShouldCreateAndSave_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateTutorialRequest { Title = "New Tutorial", FilesId = new List<int> { 1, 2 } };
        var mappedTutorial = new Tutorial { Title = "New Tutorial" };
        var files = new List<File> { new File { Id = 1 }, new File { Id = 2 } };
        var fixedTime = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);

        _mapperMock.Setup(x => x.Map<Tutorial>(request))
            .Returns(mappedTutorial);

        _fileRepoMock.Setup(x => x.GetByIdsAsync(request.FilesId))
            .ReturnsAsync(files);

        _timeProviderMock.Setup(x => x.GetUtcNow())
            .Returns(fixedTime);

        // Act
        var result = await _sut.CreateTutorial(request);

        // Assert
        Assert.Equal(fixedTime.UtcDateTime, result.CreatedAt);
        Assert.Equal(fixedTime.UtcDateTime, result.UpdatedAt);
        Assert.Equal(files.Count, result.Files.Count);

        _tutorialRepoMock.Verify(x => x.AddAsync(mappedTutorial, It.IsAny<CancellationToken>()), Times.Once);
        _tutorialRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTutorialAsync_ShouldThrow_WhenTutorialDoesNotExist()
    {
        // Arrange
        var tutorialId = 99;
        var request = new UpdateTutorialRequest { Title = "Update" };

        _tutorialRepoMock.Setup(x => x.GetByIdAsync(tutorialId, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tutorial?)null);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateTutorialAsync(tutorialId, request));
    }

    [Fact]
    public async Task DeleteTutorialAsync_ShouldRemoveAndSave_WhenFound()
    {
        // Arrange
        var tutorialId = 1;
        var existingTutorial = new Tutorial { Id = tutorialId };

        _tutorialRepoMock.Setup(x => x.GetByIdAsync(tutorialId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTutorial);

        // Act
        await _sut.DeleteTutorialAsync(tutorialId);

        // Assert
        _tutorialRepoMock.Verify(x => x.RemoveAsync(existingTutorial, It.IsAny<CancellationToken>()), Times.Once);
        _tutorialRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task DeleteTutorialAsync_ShouldThrow_WhenTutorialDoesNotExist()
    {
        // Arrange
        var tutorialId = 99;
        _tutorialRepoMock.Setup(x => x.GetByIdAsync(tutorialId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tutorial?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteTutorialAsync(tutorialId));
    }

    [Fact]
    public async Task GetTutorialListAsync_ShouldReturnList_WhenCalled()
    {
        // Arrange
        var tutorials = new List<Tutorial> { new Tutorial { Id = 1 }, new Tutorial { Id = 2 } };

        _tutorialRepoMock.Setup(x => x.GetAllAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tutorials);

        // Act
        var result = await _sut.GetTutorialListAsync();

        // Assert
        Assert.Equal(2, result.Count);
        _tutorialRepoMock.Verify(x => x.GetAllAsync(true, It.IsAny<CancellationToken>()), Times.Once);
    }

}
