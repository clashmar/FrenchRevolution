namespace FrenchRevolution.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(DatabaseCollection))]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}