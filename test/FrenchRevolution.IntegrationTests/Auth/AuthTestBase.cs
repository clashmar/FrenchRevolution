using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FrenchRevolution.IntegrationTests.Auth;

[Collection(nameof(DatabaseCollection))]
public class AuthTestBase(DatabaseFixture databaseFixture) : IAsyncLifetime
{
    protected UserManager<ApplicationUser> UserManager = null!;
    protected RoleManager<IdentityRole> RoleManager = null!;
    protected SignInManager<ApplicationUser> SignInManager = null!;

    private AppDbContext _dbContext = null!;
    private IServiceScope _scope = null!;

    public async Task InitializeAsync()
    {
        await databaseFixture.ResetDatabaseAsync();
        _dbContext = databaseFixture.CreateDbContext();

        var services = new ServiceCollection();

        services.AddLogging();
        services.AddSingleton(_dbContext);

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddRoles<IdentityRole>()
            .AddSignInManager()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton(Mock.Of<IAuthenticationSchemeProvider>());

        var serviceProvider = services.BuildServiceProvider();
        _scope = serviceProvider.CreateScope();

        UserManager = _scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        RoleManager = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        SignInManager = _scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

        await SeedRolesAsync();
    }

    private async Task SeedRolesAsync()
    {
        string[] roles = [Roles.Admin, Roles.Member];
        foreach (var roleName in roles)
        {
            if (!await RoleManager.RoleExistsAsync(roleName))
            {
                await RoleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    protected async Task CreateTestUserAsync(
        string email = "test@example.com",
        string password = "TestPass123!",
        string? displayName = null)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = false,
            DisplayName = displayName
        };

        await UserManager.CreateAsync(user, password);
        await UserManager.AddToRoleAsync(user, Roles.Member);
    }

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _dbContext.DisposeAsync();
    }
}
