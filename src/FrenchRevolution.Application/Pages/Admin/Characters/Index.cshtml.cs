using FrenchRevolution.Contracts.Mapping;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenchRevolution.Application.Pages.Admin.Characters;

public class IndexModel(ICharacterRepository repository) : PageModel
{
    public PagedList<CharacterResponseDto> Characters { get; set; } = null!;

    [BindProperty(SupportsGet = true)]
    public string? NameFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SortColumn { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SortOrder { get; set; }

    public async Task OnGetAsync([FromQuery] int page = 1)
    {
        var (items, totalCount) = await repository.GetAllAsync(
            name: NameFilter,
            sortColumn: SortColumn,
            sortOrder: SortOrder,
            page: page,
            pageSize: 10);

        var dtos = items.Select(c => c.ToResponseDto()).ToList();

        Characters = PagedList<CharacterResponseDto>.CreatePagedListAsync(dtos, page, 10, totalCount);
    }

    public string GetSortUrl(string column)
    {
        var newOrder = SortColumn == column && SortOrder == "asc" ? "desc" : "asc";
        var queryParams = new List<string> { $"sortColumn={column}", $"sortOrder={newOrder}" };

        if (!string.IsNullOrEmpty(NameFilter))
        {
            queryParams.Add($"nameFilter={Uri.EscapeDataString(NameFilter)}");
        }

        return "?" + string.Join("&", queryParams);
    }

    public string GetSortIndicator(string column)
    {
        if (SortColumn != column)
        {
            return "";
        }
        
        return SortOrder == "desc" ? " ▼" : " ▲";
    }

    public string GetPageUrl(int page)
    {
        var queryParams = new List<string> { $"page={page}" };

        if (!string.IsNullOrEmpty(NameFilter))
        {
            queryParams.Add($"nameFilter={Uri.EscapeDataString(NameFilter)}");
        }
        if (!string.IsNullOrEmpty(SortColumn))
        {
            queryParams.Add($"sortColumn={SortColumn}");
        }
        if (!string.IsNullOrEmpty(SortOrder))
        {
            queryParams.Add($"sortOrder={SortOrder}");
        }

        return "?" + string.Join("&", queryParams);
    }
}
