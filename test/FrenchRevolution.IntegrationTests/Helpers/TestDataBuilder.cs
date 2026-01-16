using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;

namespace FrenchRevolution.IntegrationTests.Helpers;

public class TestDataBuilder(
    IOfficeRepository officeRepository,
    ICharacterRepository characterRepository,
    IUnitOfWork unitOfWork)
{
    public OfficeBuilder CreateOffice() => new(officeRepository);
    
    public CharacterBuilder CreateCharacter() => new(characterRepository, officeRepository);

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

public class CharacterBuilder(ICharacterRepository characterRepository, IOfficeRepository officeRepository)
{
    private string _name = "Test Character";
    private string _profession = "Test Profession";
    private DateTime _born = new(1750, 1, 1);
    private DateTime _died = new(1800, 1, 1);
    private readonly List<(string officeTitle, DateTime from, DateTime to)> _offices = [];

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

    public CharacterBuilder WithOffice(string officeTitle, DateTime from, DateTime to)
    {
        _offices.Add((officeTitle, from, to));
        return this;
    }

    public Character Build()
    {
        var character = new Character(_name, _profession, _born, _died);

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

        characterRepository.Add(character);
        return character;
    }
}