using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.Infrastructure.Repositories;
using FrenchRevolution.IntegrationTests.Fixtures;
using FrenchRevolution.IntegrationTests.Helpers;

namespace FrenchRevolution.IntegrationTests.Factions;

[Collection(nameof(DatabaseCollection))]
public class FactionTestBase(DatabaseFixture databaseFixture) : IAsyncLifetime
{
    protected IFactionRepository FactionRepository = null!;
    protected IUnitOfWork UnitOfWork = null!;

    private AppDbContext _dbContext = null!;
    private TestDataBuilder _testData = null!;

    protected const string Jacobins = "Jacobins";
    protected const string Girondins = "Girondins";
    protected const string Cordeliers = "Cordeliers";

    protected const string JacobinsDescription = "Jacobins Description";
    protected const string GirondinsDescription = "Girondins Description";
    protected const string CordeliersDescription = "Cordeliers Description";

    public async Task InitializeAsync()
    {
        await databaseFixture.ResetDatabaseAsync();
        _dbContext = databaseFixture.CreateDbContext();

        var characterRepository = new CharacterRepository(_dbContext);
        var officeRepository = new OfficeRepository(_dbContext);
        FactionRepository = new FactionRepository(_dbContext);
        UnitOfWork = new UnitOfWork(_dbContext);
        _testData = new TestDataBuilder(officeRepository, characterRepository, FactionRepository, UnitOfWork);
    }

    protected async Task<Faction> SetupFaction(string title, string description)
    {
        var faction = _testData.CreateFaction()
            .WithTitleAndDescription(title, description)
            .Build();

        await _testData.SaveAsync();
        _dbContext.ChangeTracker.Clear();
        return faction;
    }

    protected async Task SetupFactions(params (string Title, string Description)[] factions)
    {
        foreach (var (title, description) in factions)
        {
            _testData.CreateFaction()
                .WithTitleAndDescription(title, description)
                .Build();
        }

        await _testData.SaveAsync();
        _dbContext.ChangeTracker.Clear();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}
