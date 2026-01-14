using FrenchRevolution.Application.Characters.Handlers;
using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.IntegrationTests.Fixtures;

namespace FrenchRevolution.IntegrationTests.Characters;

public class GetAllCharactersHandlerTests(
    DatabaseFixture databaseFixture
    ) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoCharactersExist()
    {
        // Arrange
        var handler = new GetAllCharactersHandler(CharacterRepository);
        var query = new GetAllCharactersQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task Handle_ReturnsAllCharacters_WhenCharactersExist()
    {
        // Arrange
        await SetupDefaultCharacters();

        var handler = new GetAllCharactersHandler(CharacterRepository);
        var query = new GetAllCharactersQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var characters = result.ToList();
        Assert.Equal(2, characters.Count);
        
        var robespierre = characters.First(c => c.Name == Robespierre);
        Assert.Equal(Lawyer, robespierre.Profession);
        Assert.Single(robespierre.Roles);
        
        var desmoulins = characters.First(c => c.Name == Desmoulins);
        Assert.Equal(Journalist, desmoulins.Profession);
        Assert.Empty(desmoulins.Roles);
    }
    
    private const string Robespierre = "Maximilen Robespierre";
    private const string Desmoulins = "Camille Desmoulins";
    private const string Lawyer = "Lawyer";
    private const string Journalist = "Journalist";
    private const string PresidentOfTheNationalConvention = "President of the National Convention";

    private readonly DateTime _from = new(1791, 5, 6);
    private readonly DateTime _to = new(1794, 5, 6);
    
    private async Task SetupDefaultCharacters()
    {
        // Arrange
        TestData.CreateCharacter()
            .WithName(Robespierre)
            .WithProfession(Lawyer)
            .WithDates(_from, _to)
            .WithRole(PresidentOfTheNationalConvention, 
                _from, 
                _to)
            .Build();

        TestData.CreateCharacter()
            .WithName(Desmoulins)
            .WithProfession(Journalist)
            .WithDates(_from, _to)
            .Build();

        await TestData.SaveAsync();
    }
}