using Microsoft.AspNetCore.Identity;

namespace FrenchRevolution.Infrastructure.Data;

public sealed class ApplicationUser : IdentityUser
{
    public bool EnableNotifications { get; set; }
    public string? DisplayName { get; set; }
}