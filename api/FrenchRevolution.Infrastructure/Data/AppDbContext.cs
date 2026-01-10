using FrenchRevolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Character> Characters => Set<Character>();
}