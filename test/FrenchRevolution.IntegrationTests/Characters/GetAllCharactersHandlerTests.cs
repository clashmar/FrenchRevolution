using FrenchRevolution.Application.Characters.Handlers;
using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.IntegrationTests.Fixtures;

namespace FrenchRevolution.IntegrationTests.Characters; 

public class GetAllCharactersHandlerTests( 
    DatabaseFixture databaseFixture
    ) : CharacterTestBase(databaseFixture)
{
    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoCharactersExist()
    {
        // Arrange
        var handler = new GetAllCharactersHandler(CacheAside, CharacterRepository);
        var query = new GetAllCharactersQuery();

        // Act
        var pagedList = await handler.Handle(query, CancellationToken.None);
        var items = pagedList.Items;

        // Assert
        Assert.NotNull(items);
        Assert.Empty(items);
    }
    
    [Fact]
    public async Task Handle_ReturnsAllCharacters_WhenCharactersExist()
    {
        // Arrange
        await SetupCharacter(MaximilienRobespierre, Lawyer, President);
        await SetupCharacter(CamilleDesmoulins, Journalist, Deputy);

        var handler = new GetAllCharactersHandler(CacheAside, CharacterRepository);
        var query = new GetAllCharactersQuery();

        // Act
        var pagedList = await handler.Handle(query, CancellationToken.None);
        var items = pagedList.Items;
        
        // Assert
        Assert.Equal(2, items.Count);
        
        var robespierre = items.First(c => c.Name == MaximilienRobespierre);
        Assert.Equal(Lawyer, robespierre.Profession);
        Assert.Equal(President, robespierre.Offices.First().Title);
        
        var desmoulins = items.First(c => c.Name == CamilleDesmoulins);
        Assert.Equal(Journalist, desmoulins.Profession);
        Assert.Equal(Deputy, desmoulins.Offices.First().Title);
    }
    
    [Fact]
    public async Task Handle_ReturnsCachedList_WhenCacheExists()
    {
        // Arrange
        await SetupCharacter(MaximilienRobespierre);

        var handler = new GetAllCharactersHandler(CacheAside, CharacterRepository);
        var query = new GetAllCharactersQuery();
        
        // Act
        var pagedList = await handler.Handle(query, CancellationToken.None);
        var items = pagedList.Items;
        
        // Assert the character was added correctly
        Assert.Single(items);
        
        // Add another character
        await SetupCharacter(GeorgesDanton);
        
        // Assert the cached single character is returned
        Assert.Single(items);
        Assert.DoesNotContain(items, c => c.Name == GeorgesDanton);
    }
}