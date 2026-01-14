using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.Infrastructure.Repositories;
using FrenchRevolution.IntegrationTests.Fixtures;
using FrenchRevolution.IntegrationTests.Helpers;
using FrenchRevolution.IntegrationTests.Infrastructure;

namespace FrenchRevolution.IntegrationTests;

[Collection(nameof(DatabaseCollection))]
public class IntegrationTestBase(DatabaseFixture databaseFixture) : IAsyncLifetime
{
    private TestAppDbContext _dbContext = null!;
    protected TestDataBuilder TestData = null!;
    
    protected ICharacterRepository CharacterRepository = null!;
    private IRoleRepository _roleRepository = null!;
    private IUnitOfWork _unitOfWork = null!;

    public async Task InitializeAsync()
    {
        await databaseFixture.ResetDatabaseAsync();
        _dbContext = await databaseFixture.CreateDbContextAsync();
        
        CharacterRepository = new CharacterRepository(_dbContext);
        _roleRepository = new RoleRepository(_dbContext);
        _unitOfWork = new UnitOfWork(_dbContext);
        TestData = new TestDataBuilder(_roleRepository, CharacterRepository, _unitOfWork);
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}