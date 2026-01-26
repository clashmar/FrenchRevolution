using FrenchRevolution.Application.Auth.Commands;
using FrenchRevolution.Application.Auth.Handlers;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.IntegrationTests.Fixtures;

namespace FrenchRevolution.IntegrationTests.Auth;

public class RegisterHandlerTests(
    DatabaseFixture databaseFixture
) : AuthTestBase(databaseFixture)
{
    private const string UserEmail = "newuser@example.com";
    private const string ValidPassword = "ValidPassword123!";
    private const string TestUser = "TestUser";
    private const string ShortPassword = "Short1!";
    
    [Fact]
    public async Task Handle_CreatesUser_WithValidRequest()
    {
        // Arrange
        var handler = new RegisterHandler(UserManager);

        var request = new RegisterRequestDto(
            UserEmail,
            ValidPassword,
            ValidPassword,
            TestUser);

        var command = new RegisterCommand(request);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(UserEmail, result.Value.Email);
        Assert.False(string.IsNullOrEmpty(result.Value.UserId));

        var user = await UserManager.FindByEmailAsync(UserEmail);
        Assert.NotNull(user);
        Assert.Equal(TestUser, user.DisplayName);
    }

    [Fact]
    public async Task Handle_AssignsMemberRole_OnRegistration()
    {
        // Arrange
        var handler = new RegisterHandler(UserManager);

        var request = new RegisterRequestDto(
            UserEmail,
            ValidPassword,
            ValidPassword,
            null);

        var command = new RegisterCommand(request);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        var user = await UserManager.FindByEmailAsync(UserEmail);
        Assert.NotNull(user);

        var roles = await UserManager.GetRolesAsync(user);
        Assert.Contains(Roles.Member, roles);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenEmailAlreadyExists()
    {
        // Arrange
        await CreateTestUserAsync(UserEmail);

        var handler = new RegisterHandler(UserManager);

        var request = new RegisterRequestDto(
            UserEmail,
            ValidPassword,
            ValidPassword,
            null);

        var command = new RegisterCommand(request);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenPasswordTooShort()
    {
        // Arrange
        var handler = new RegisterHandler(UserManager);

        var request = new RegisterRequestDto(
            UserEmail,
            ShortPassword,
            ShortPassword,
            null);

        var command = new RegisterCommand(request);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
    }
}
