using FrenchRevolution.Application.Factions.Queries;
using FrenchRevolution.IntegrationTests.Fixtures;

namespace FrenchRevolution.IntegrationTests.Factions;

public class GetAllFactionsHandlerTests(
    DatabaseFixture databaseFixture
) : FactionTestBase(databaseFixture)
{
    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoFactionsExist()
    {
        // Arrange
        var handler = new GetAllFactionsHandler(FactionRepository);
        var query = new GetAllFactionsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ReturnsAllFactions_WhenFactionsExist()
    {
        // Arrange
        await SetupFactions(
            (Jacobins, JacobinsDescription),
            (Girondins, GirondinsDescription),
            (Cordeliers, CordeliersDescription)
        );

        var handler = new GetAllFactionsHandler(FactionRepository);
        var query = new GetAllFactionsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, f => f.Title == Jacobins);
        Assert.Contains(result, f => f.Title == Girondins);
        Assert.Contains(result, f => f.Title == Cordeliers);
    }
}
