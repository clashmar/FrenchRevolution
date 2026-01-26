using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Application.Characters.Handlers;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.IntegrationTests.Fixtures;

namespace FrenchRevolution.IntegrationTests.Characters;

public class UpdateCharacterHandlerTests(
    DatabaseFixture databaseFixture
) : CharacterTestBase(databaseFixture)
{
    [Fact]
    public async Task Handle_ReturnsFalse_WhenCharacterDoesNotExist()
    {
        // Arrange
        var handler = new UpdateCharacterHandler(
            CharacterRepository,
            UnitOfWork,
            OfficeRepository,
            FactionRepository);

        var request = new CharacterRequestDto(
            GeorgesDanton,
            Lawyer,
            From,
            To,
            DefaultPortraitUrl,
            [],
            []);

        var command = new UpdateCharacterCommand(Guid.NewGuid(), request);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_UpdatesBasicProperties_WhenCharacterExists()
    {
        // Arrange
        var character = await SetupCharacter(MaximilienRobespierre);

        var handler = new UpdateCharacterHandler(
            CharacterRepository,
            UnitOfWork,
            OfficeRepository,
            FactionRepository);

        const string updatedName = "Maximilien de Robespierre";
        const string updatedProfession = "Politician";

        var request = new CharacterRequestDto(
            updatedName,
            updatedProfession,
            From,
            To,
            DefaultPortraitUrl,
            [],
            []);

        var command = new UpdateCharacterCommand(character.Id, request);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        var updatedCharacter = await CharacterRepository.GetByIdAsync(character.Id, CancellationToken.None);
        Assert.NotNull(updatedCharacter);
        Assert.Equal(updatedName, updatedCharacter.Name);
        Assert.Equal(updatedProfession, updatedCharacter.Profession);
    }

    [Fact]
    public async Task Handle_ThrowsDuplicateCharacterException_WhenNameAlreadyExists()
    {
        // Arrange
        await SetupCharacter(GeorgesDanton);
        var character = await SetupCharacter(CamilleDesmoulins);

        var handler = new UpdateCharacterHandler(
            CharacterRepository,
            UnitOfWork,
            OfficeRepository,
            FactionRepository);

        var request = new CharacterRequestDto(
            GeorgesDanton,
            Journalist,
            From,
            To,
            DefaultPortraitUrl,
            [],
            []);

        var command = new UpdateCharacterCommand(character.Id, request);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateCharacterException>(
            () => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_AllowsSameName_WhenUpdatingSameCharacter()
    {
        // Arrange
        var character = await SetupCharacter(MaximilienRobespierre);

        var handler = new UpdateCharacterHandler(
            CharacterRepository,
            UnitOfWork,
            OfficeRepository,
            FactionRepository);

        const string updatedProfession = "Revolutionary";

        var request = new CharacterRequestDto(
            MaximilienRobespierre,
            updatedProfession,
            From,
            To,
            DefaultPortraitUrl,
            [],
            []);

        var command = new UpdateCharacterCommand(character.Id, request);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        var updatedCharacter = await CharacterRepository.GetByIdAsync(character.Id, CancellationToken.None);
        Assert.NotNull(updatedCharacter);
        Assert.Equal(MaximilienRobespierre, updatedCharacter.Name);
        Assert.Equal(updatedProfession, updatedCharacter.Profession);
    }
}
