using FrenchRevolution.Domain.Repositories;

namespace FrenchRevolution.Infrastructure.Data;

public sealed class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}