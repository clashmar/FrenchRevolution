using System.ComponentModel.DataAnnotations;

namespace FrenchRevolution.Contracts.Models.Pages;

public class CharacterInputModel
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string Profession { get; set; } = string.Empty;

    [Required]
    public DateTime Born { get; set; }

    [Required]
    public DateTime Died { get; set; }

    [Url]
    public string? PortraitUrl { get; set; }

    public List<OfficeInput>? Offices { get; set; }

    public List<FactionInput>? Factions { get; set; }
}

public class OfficeInput
{
    public string Title { get; set; } = string.Empty;
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class FactionInput
{
    public string Title { get; set; } = string.Empty;
}
