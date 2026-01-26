using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Application.Characters.Handlers;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.IntegrationTests.Fixtures;

namespace FrenchRevolution.IntegrationTests.Characters;

public class CreateCharacterHandlerTests(
    DatabaseFixture databaseFixture
) : CharacterTestBase(databaseFixture)
{
    [Fact]
    public async Task Handle_CreatesCharacter_WithBasicProperties()
    {
        // Arrange
        var handler = new CreateCharacterHandler(
            CharacterRepository,
            OfficeRepository,
            FactionRepository,
            UnitOfWork);

        var request = new CharacterRequestDto(
            GeorgesDanton,
            Lawyer,
            From,
            To,
            DefaultPortraitUrl,
            [],
            []);

        var command = new CreateCharacterCommand(request);

        // Act
        var characterId = await handler.Handle(command, CancellationToken.None);

        // Assert
        var character = await CharacterRepository.GetByIdAsync(characterId, CancellationToken.None);
        Assert.NotNull(character);
        Assert.Equal(GeorgesDanton, character.Name);
        Assert.Equal(Lawyer, character.Profession);
    }

    [Fact]
    public async Task Handle_CreatesCharacter_WithOffices()
    {
        // Arrange
        var handler = new CreateCharacterHandler(
            CharacterRepository,
            OfficeRepository,
            FactionRepository,
            UnitOfWork);

        var offices = new List<OfficeRequestDto>
        {
            new(MinisterOfJustice, From, To)
        };

        var request = new CharacterRequestDto(
            MaximilienRobespierre,
            Lawyer,
            From,
            To,
            DefaultPortraitUrl,
            offices,
            []);

        var command = new CreateCharacterCommand(request);

        // Act
        var characterId = await handler.Handle(command, CancellationToken.None);

        // Assert
        var character = await CharacterRepository.GetByIdAsync(characterId, CancellationToken.None);
        Assert.NotNull(character);
        Assert.Single(character.CharacterOffices);
        Assert.Equal(MinisterOfJustice, character.CharacterOffices.First().Office.Title);
    }

    [Fact]
    public async Task Handle_ThrowsDuplicateCharacterException_WhenNameExists()
    {
        // Arrange
        await SetupCharacter(CamilleDesmoulins);

        var handler = new CreateCharacterHandler(
            CharacterRepository,
            OfficeRepository,
            FactionRepository,
            UnitOfWork);

        var request = new CharacterRequestDto(
            CamilleDesmoulins,
            Journalist,
            From,
            To,
            DefaultPortraitUrl,
            [],
            []);

        var command = new CreateCharacterCommand(request);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateCharacterException>(
            () => handler.Handle(command, CancellationToken.None));
    }
}
