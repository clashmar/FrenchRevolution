using System.Runtime.InteropServices;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Cache;
using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.Infrastructure.Repositories;
using FrenchRevolution.IntegrationTests.Fixtures;
using FrenchRevolution.IntegrationTests.Helpers;
using FrenchRevolution.IntegrationTests.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FrenchRevolution.IntegrationTests;

[Collection(nameof(DatabaseCollection))]
public class CharacterTestBase(DatabaseFixture databaseFixture) : IAsyncLifetime
{
    private TestAppDbContext _dbContext = null!;
    protected TestDataBuilder TestData = null!;

    protected ICacheAside CacheAside = null!;
    protected ICharacterRepository CharacterRepository = null!;
    
    private IDistributedCache _distributedCache = null!;
    private IRoleRepository _roleRepository = null!;
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
    protected readonly DateTime From = new(1791, 5, 6);
    protected readonly DateTime To = new(1794, 5, 6);

    public async Task InitializeAsync()
    {
        await databaseFixture.ResetDatabaseAsync();
        _dbContext = await databaseFixture.CreateDbContextAsync();
        
        var memoryCacheOptions = Options.Create(new MemoryDistributedCacheOptions());
        _distributedCache = new MemoryDistributedCache(memoryCacheOptions);

        var mockCacheLogger = new Mock<ILogger<CacheAside>>();
        CacheAside = new CacheAside(_distributedCache, mockCacheLogger.Object);
        
        CharacterRepository = new CharacterRepository(_dbContext);
        _roleRepository = new RoleRepository(_dbContext);
        _unitOfWork = new UnitOfWork(_dbContext);
        TestData = new TestDataBuilder(_roleRepository, CharacterRepository, _unitOfWork);
    }
    
    /// Builds and adds a character with the given name to the test db.
    protected async Task SetupCharacter(
        string name,
        string profession = Lawyer,
        string roleTitle = Deputy
        )
    {
        TestData.CreateCharacter()
            .WithName(name)
            .WithProfession(profession)
            .WithDates(From, To)
            .WithRole(roleTitle, 
                From, 
                To)
            .Build();

        await TestData.SaveAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}