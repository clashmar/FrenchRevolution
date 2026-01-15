using FrenchRevolution.Application.Characters.Handlers;
using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Contracts.Models;
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
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
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
        var characters = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(2, characters.Count);
        
        var robespierre = characters.First(c => c.Name == MaximilienRobespierre);
        Assert.Equal(Lawyer, robespierre.Profession);
        Assert.Equal(President, robespierre.Roles.First().Title);
        
        var desmoulins = characters.First(c => c.Name == CamilleDesmoulins);
        Assert.Equal(Journalist, desmoulins.Profession);
        Assert.Equal(Deputy, desmoulins.Roles.First().Title);
    }
    
    [Fact]
    public async Task Handle_ReturnsCachedList_WhenCacheExists()
    {
        // Arrange
        await SetupCharacter(MaximilienRobespierre);

        var handler = new GetAllCharactersHandler(CacheAside, CharacterRepository);
        var query = new GetAllCharactersQuery();
        
        // Act
        var characters = await handler.Handle(query, CancellationToken.None);
        
        // Assert the character was added correctly
        Assert.Single(characters);
        
        // Add another character
        await SetupCharacter(GeorgesDanton);
        
        // Assert the cached single character is returned
        Assert.Single(characters);
        Assert.DoesNotContain(characters, c => c.Name == GeorgesDanton);
    }
}