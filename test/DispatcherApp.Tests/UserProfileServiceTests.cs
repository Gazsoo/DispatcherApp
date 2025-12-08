using AutoMapper;
using Castle.Core.Logging;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Common.Services;
using DispatcherApp.Common.Abstractions.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.OpenApi.Validations;
using Moq;

namespace DispatcherApp.Tests;

public class UserProfileServiceTests
{
    private readonly Mock<UserManager<IdentityUser>> _userManager;
    private readonly Mock<IUserRepository> _userRepo;
    private readonly Mock<ILogger<UserProfileService>> _logger;
    private readonly UserProfileService _sut;

    public UserProfileServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        _userManager = new Mock<UserManager<IdentityUser>>(
            userStoreMock.Object,
            null!, null!, null!, null!, null!, null!, null!, null!
        );
        _userRepo = new Mock<IUserRepository>();
        _logger = new Mock<ILogger<UserProfileService>>();
        _sut = new UserProfileService(_userManager.Object, _logger.Object, _userRepo.Object);
    }

    [Fact]
    public async Task UserProfileService_ShouldCallClaimsAndRoles_WhenRequestIsValid()
    {
        // Arrange
        var userId = "string_id";
        var user = new IdentityUser { Id = userId, Email = "test@mail.com" };

        _userManager.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(user);
        _userManager.Setup(x => x.GetClaimsAsync(user))
            .ReturnsAsync(new List<System.Security.Claims.Claim>());
        _userManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string>());

        // Act
        var result = await _sut.GetAsync(userId);

        // Assert
        _userManager.Verify(x => x.GetClaimsAsync(user), Times.Once);
        _userManager.Verify(x => x.GetRolesAsync(user), Times.Once);
        _userManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
        Assert.Equal("test@mail.com", result.Email);
        Assert.Equal(userId, result.Id);
    }
}
