using FrenchRevolution.Application.Characters.Handlers;
using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.IntegrationTests.Fixtures;

namespace FrenchRevolution.IntegrationTests.Characters;

public class GetCharacterByIdHandlerTests(
    DatabaseFixture databaseFixture
) : CharacterTestBase(databaseFixture)
{
    [Fact]
    public async Task Handle_ReturnsNull_WhenCharacterDoesNotExist()
    {
        // Arrange
        var handler = new GetCharacterByIdHandler(CacheAside, CharacterRepository);
        var query = new GetCharacterByIdQuery(Guid.NewGuid());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ReturnsCharacter_WhenExists()
    {
        // Arrange
        var character = await SetupCharacter(MaximilienRobespierre, Lawyer, President);

        var handler = new GetCharacterByIdHandler(CacheAside, CharacterRepository);
        var query = new GetCharacterByIdQuery(character.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(character.Id, result.Id);
        Assert.Equal(MaximilienRobespierre, result.Name);
        Assert.Equal(Lawyer, result.Profession);
        Assert.Equal(President, result.Offices.First().Title);
    }
}
