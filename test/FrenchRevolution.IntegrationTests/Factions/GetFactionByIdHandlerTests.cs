using FrenchRevolution.Application.Factions.Queries;
using FrenchRevolution.IntegrationTests.Fixtures;

namespace FrenchRevolution.IntegrationTests.Factions;

public class GetFactionByIdHandlerTests(
    DatabaseFixture databaseFixture
) : FactionTestBase(databaseFixture)
{
    [Fact]
    public async Task Handle_ReturnsNull_WhenFactionDoesNotExist()
    {
        // Arrange
        var handler = new GetFactionByIdHandler(FactionRepository);
        var query = new GetFactionByIdQuery(Guid.NewGuid());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ReturnsFaction_WhenExists()
    {
        // Arrange
        var faction = await SetupFaction(Jacobins, JacobinsDescription);

        var handler = new GetFactionByIdHandler(FactionRepository);
        var query = new GetFactionByIdQuery(faction.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(faction.Id, result.Id);
        Assert.Equal(Jacobins, result.Title);
        Assert.Equal(JacobinsDescription, result.Description);
    }
}
