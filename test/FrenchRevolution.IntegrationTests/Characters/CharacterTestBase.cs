using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Cache;
using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.Infrastructure.Repositories;
using FrenchRevolution.IntegrationTests.Fixtures;
using FrenchRevolution.IntegrationTests.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FrenchRevolution.IntegrationTests.Characters;

[Collection(nameof(DatabaseCollection))]
public class CharacterTestBase(DatabaseFixture databaseFixture) : IAsyncLifetime
{
    protected ICacheAside CacheAside = null!;
    protected ICharacterRepository CharacterRepository = null!;
    
    private AppDbContext _dbContext = null!;
    private TestDataBuilder _testData = null!;
    
    private IDistributedCache _distributedCache = null!;
    private IOfficeRepository _officeRepository = null!;
    private IFactionRepository _factionRepository = null!;
    private IUnitOfWork _unitOfWork = null!;
    
    // Character names
    protected const string GeorgesDanton = "Georges Jacques Danton";
    protected const string MaximilienRobespierre = "Maximilen Robespierre";
    protected const string CamilleDesmoulins = "Camille Desmoulins";
    
    // Character professions
    protected const string Lawyer = "Lawyer";
    protected const string Journalist = "Journalist";
    
    // Character role titles
    protected const string MinisterOfJustice = "Minister of Justice";
    protected const string President = "President of the National Convention";
    protected const string Deputy = "Deputy of the National Convention";

    // Dates
    protected static readonly DateTime From = new(1791, 5, 6);
    protected static readonly DateTime To = new(1794, 5, 6);

    public async Task InitializeAsync()
    {
        await databaseFixture.ResetDatabaseAsync();
        _dbContext = databaseFixture.CreateDbContext();
        
        var memoryCacheOptions = Options.Create(new MemoryDistributedCacheOptions());
        _distributedCache = new MemoryDistributedCache(memoryCacheOptions);

        var mockCacheLogger = new Mock<ILogger<CacheAside>>();
        CacheAside = new CacheAside(_distributedCache, mockCacheLogger.Object);
        
        CharacterRepository = new CharacterRepository(_dbContext);
        _officeRepository = new OfficeRepository(_dbContext);
        _factionRepository = new FactionRepository(_dbContext);
        _unitOfWork = new UnitOfWork(_dbContext);
        _testData = new TestDataBuilder(_officeRepository, CharacterRepository, _factionRepository, _unitOfWork);
    }
    
    /// Builds and adds a character with the given name
    /// and optional profession and role to the test db.
    protected async Task SetupCharacter(
        string name,
        string profession = Lawyer,
        string roleTitle = Deputy
        )
    {
        _testData.CreateCharacter()
            .WithName(name)
            .WithProfession(profession)
            .WithDates(From, To)
            .WithOffice(roleTitle, 
                From, 
                To)
            .Build();

        await _testData.SaveAsync();
    }
    
    /// Batch setup for multiple characters
    protected async Task SetupCharacters(
        params (string Name, string Profession, string RoleTitle)[] characters
        )
    {
        foreach (var (name, profession, roleTitle) in characters)
        {
            _testData.CreateCharacter()
                .WithName(name)
                .WithProfession(profession)
                .WithDates(From, To)
                .WithOffice(roleTitle, From, To)
                .Build();
        }
        
        await _testData.SaveAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}