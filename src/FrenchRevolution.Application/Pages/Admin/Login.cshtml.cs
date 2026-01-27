using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FrenchRevolution.Application.Constants;
using FrenchRevolution.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenchRevolution.Application.Pages.Admin;

public class LoginModel(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager
    ) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/Admin");
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/Admin");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await userManager.FindByEmailAsync(Input.Email);
        if (user is null)
        {
            ErrorMessage = "Invalid email or password.";
            return Page();
        }

        var isAdmin = await userManager.IsInRoleAsync(user, Roles.Admin);
        if (!isAdmin)
        {
            ErrorMessage = "Access denied. Admin privileges required.";
            return Page();
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, Input.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                ErrorMessage = "Account locked. Please try again later.";
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
            }
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? user.Email!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Role, Roles.Admin)
        };

        var identity = new ClaimsIdentity(claims, AuthenticationType.AdminCookie);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(AuthenticationType.AdminCookie, principal, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        });

        return LocalRedirect(returnUrl);
    }
}
