using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Primitives;
using FrenchRevolution.Domain.Repositories;

namespace FrenchRevolution.IntegrationTests.Helpers;

public class TestDataBuilder(
    IOfficeRepository officeRepository,
    ICharacterRepository characterRepository,
    IFactionRepository factionRepository,
    IUnitOfWork unitOfWork)
{
    public OfficeBuilder CreateOffice() => new(officeRepository);

    public CharacterBuilder CreateCharacter() => new(characterRepository, officeRepository, factionRepository);

    public FactionBuilder CreateFaction() => new(factionRepository);

    public async Task SaveAsync() => await unitOfWork.SaveChangesAsync();
}

public class OfficeBuilder(IOfficeRepository repository)
{
    private Office? _office;

    public OfficeBuilder WithTitle(string title)
    {
        _office = new Office(title);
        return this;
    }

    public Office Build()
    {
        if (_office is null)
        {
            throw new InvalidOperationException("Office not configured. Call WithTitle first.");
        }

        repository.Add(_office);
        return _office;
    }
}

public class FactionBuilder(IFactionRepository repository)
{
    private Faction? _faction;

    public FactionBuilder WithTitleAndDescription(string title, string description)
    {
        _faction = new Faction(title, description);
        return this;
    }

    public Faction Build()
    {
        if (_faction is null)
        {
            throw new InvalidOperationException("Faction not configured. Call WithTitleAndDescription first.");
        }

        repository.Add(_faction);
        return _faction;
    }
}

public class CharacterBuilder(
    ICharacterRepository characterRepository,
    IOfficeRepository officeRepository,
    IFactionRepository factionRepository)
{
    private string _name = "Test Character";
    private string _profession = "Test Profession";
    private DateTime _born = new(1750, 1, 1);
    private DateTime _died = new(1800, 1, 1);
    private Portrait _portrait = new("https://upload.wikimedia.org/wikipedia/commons/5/57/Anonymous_-_Prise_de_la_Bastille.jpg");
    private readonly List<(string officeTitle, DateTime from, DateTime to)> _offices = [];
    private readonly List<string> _factions = [];

    public CharacterBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CharacterBuilder WithProfession(string profession)
    {
        _profession = profession;
        return this;
    }

    public CharacterBuilder WithDates(DateTime born, DateTime died)
    {
        _born = born;
        _died = died;
        return this;
    }

    public CharacterBuilder WithPortrait(Portrait portrait)
    {
        _portrait = portrait;
        return this;
    }

    public CharacterBuilder WithOffice(string officeTitle, DateTime from, DateTime to)
    {
        _offices.Add((officeTitle, from, to));
        return this;
    }

    public CharacterBuilder WithFaction(string factionTitle)
    {
        _factions.Add(factionTitle);
        return this;
    }

    public Character Build()
    {
        var character = new Character(_name, _profession, _born, _died, _portrait);

        foreach (var (officeTitle, from, to) in _offices)
        {
            var office = officeRepository.GetByTitleAsync(officeTitle).GetAwaiter().GetResult();
            if (office is null)
            {
                office = new Office(officeTitle);
                officeRepository.Add(office);
            }

            character.AssignOffice(office, from, to);
        }

        foreach (var factionTitle in _factions)
        {
            var faction = factionRepository.GetByTitleAsync(factionTitle).GetAwaiter().GetResult();
            if (faction is null)
            {
                faction = new Faction(factionTitle, string.Empty);
                factionRepository.Add(faction);
            }

            character.AssignFaction(faction);
        }

        characterRepository.Add(character);
        return character;
    }
}