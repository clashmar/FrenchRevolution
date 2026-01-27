using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenchRevolution.Application.Pages.Error;

[AllowAnonymous]
public class _404Model : PageModel
{
    public void OnGet()
    {
    }
}
