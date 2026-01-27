using FrenchRevolution.Application.Auth.Commands;
using FrenchRevolution.Application.Auth.Handlers;
using FrenchRevolution.Application.Auth.Services;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.IntegrationTests.Fixtures;
using Moq;

namespace FrenchRevolution.IntegrationTests.Auth;

public class LoginHandlerTests(
    DatabaseFixture databaseFixture
) : AuthTestBase(databaseFixture)
{
    private const string UserEmail = "login@example.com";
    private const string ValidPassword = "ValidPassword123!";
    private const string WrongPassword = "WrongPassword123!";
    private const string NonExistentEmail = "nonexistent@example.com";
    private const string TestToken = "test-jwt-token";

    [Fact]
    public async Task Handle_ReturnsToken_WithValidCredentials()
    {
        // Arrange
        await CreateTestUserAsync(UserEmail, ValidPassword);

        var mockTokenService = new Mock<ITokenService>();
        mockTokenService
            .Setup(x => x.CreateTokenForUser(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
            .Returns(TestToken);

        var handler = new LoginHandler(SignInManager, UserManager, mockTokenService.Object);
        var request = new LoginRequestDto(UserEmail, ValidPassword);
        var command = new LoginCommand(request);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(TestToken, result.Value.Token);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenUserDoesNotExist()
    {
        // Arrange
        var mockTokenService = new Mock<ITokenService>();
        var handler = new LoginHandler(SignInManager, UserManager, mockTokenService.Object);
        var request = new LoginRequestDto(NonExistentEmail, ValidPassword);
        var command = new LoginCommand(request);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        mockTokenService.Verify(
            x => x.CreateTokenForUser(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WithWrongPassword()
    {
        // Arrange
        await CreateTestUserAsync(UserEmail, ValidPassword);
        var mockTokenService = new Mock<ITokenService>();
        var handler = new LoginHandler(SignInManager, UserManager, mockTokenService.Object);
        var request = new LoginRequestDto(UserEmail, WrongPassword);
        var command = new LoginCommand(request);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        mockTokenService.Verify(
            x => x.CreateTokenForUser(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_IncludesUserRoles_InTokenGeneration()
    {
        // Arrange
        await CreateTestUserAsync(UserEmail, ValidPassword);

        IList<string>? capturedRoles = null;
        var mockTokenService = new Mock<ITokenService>();
        mockTokenService
            .Setup(x => x.CreateTokenForUser(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
            .Callback<ApplicationUser, IList<string>>((_, roles) => capturedRoles = roles)
            .Returns(TestToken);

        var handler = new LoginHandler(SignInManager, UserManager, mockTokenService.Object);

        var request = new LoginRequestDto(UserEmail, ValidPassword);
        var command = new LoginCommand(request);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedRoles);
        Assert.Contains(Roles.Member, capturedRoles);
    }
}
