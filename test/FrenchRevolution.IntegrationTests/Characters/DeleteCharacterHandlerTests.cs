using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Application.Characters.Handlers;
using FrenchRevolution.IntegrationTests.Fixtures;

namespace FrenchRevolution.IntegrationTests.Characters;

public class DeleteCharacterHandlerTests(
    DatabaseFixture databaseFixture
) : CharacterTestBase(databaseFixture)
{
    [Fact]
    public async Task Handle_ReturnsFalse_WhenCharacterDoesNotExist()
    {
        // Arrange
        var handler = new DeleteCharacterHandler(CharacterRepository, UnitOfWork);
        var command = new DeleteCharacterCommand(Guid.NewGuid());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_ReturnsTrue_WhenCharacterDeleted()
    {
        // Arrange
        var character = await SetupCharacter(GeorgesDanton);

        var handler = new DeleteCharacterHandler(CharacterRepository, UnitOfWork);
        var command = new DeleteCharacterCommand(character.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Handle_RemovesCharacter_FromDatabase()
    {
        // Arrange
        var character = await SetupCharacter(CamilleDesmoulins);

        var handler = new DeleteCharacterHandler(CharacterRepository, UnitOfWork);
        var command = new DeleteCharacterCommand(character.Id);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedCharacter = await CharacterRepository.GetByIdAsync(character.Id, CancellationToken.None);
        Assert.Null(deletedCharacter);
    }
}
