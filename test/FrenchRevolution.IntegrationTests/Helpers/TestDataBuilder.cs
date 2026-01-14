using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;

namespace FrenchRevolution.IntegrationTests.Helpers;

// TODO: Move to domain
public class TestDataBuilder(
    IRoleRepository roleRepository,
    ICharacterRepository characterRepository,
    IUnitOfWork unitOfWork)
{
    public RoleBuilder CreateRole() => new(roleRepository);
    
    public CharacterBuilder CreateCharacter() => new(characterRepository, roleRepository);

    public async Task SaveAsync() => await unitOfWork.SaveChangesAsync();
}

public class RoleBuilder(IRoleRepository repository)
{
    private Role? _role;

    public RoleBuilder WithTitle(string title)
    {
        _role = new Role(title);
        return this;
    }

    public Role Build()
    {
        if (_role is null)
        {
            throw new InvalidOperationException("Role not configured. Call WithTitle first.");
        }

        repository.Add(_role);
        return _role;
    }
}

public class CharacterBuilder(ICharacterRepository characterRepository, IRoleRepository roleRepository)
{
    private string _name = "Test Character";
    private string _profession = "Test Profession";
    private DateTime _dateOfBirth = new(1750, 1, 1);
    private DateTime _dateOfDeath = new(1800, 1, 1);
    private readonly List<(string roleTitle, DateTime from, DateTime to)> _roles = [];

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

    public CharacterBuilder WithDates(DateTime dateOfBirth, DateTime dateOfDeath)
    {
        _dateOfBirth = dateOfBirth;
        _dateOfDeath = dateOfDeath;
        return this;
    }

    public CharacterBuilder WithRole(string roleTitle, DateTime from, DateTime to)
    {
        _roles.Add((roleTitle, from, to));
        return this;
    }

    public Character Build()
    {
        var character = new Character(_name, _profession, _dateOfBirth, _dateOfDeath);

        foreach (var (roleTitle, from, to) in _roles)
        {
            var role = roleRepository.GetByTitleAsync(roleTitle).GetAwaiter().GetResult();
            if (role is null)
            {
                role = new Role(roleTitle);
                roleRepository.Add(role);
            }

            character.AssignRole(role, from, to);
        }

        characterRepository.Add(character);
        return character;
    }
}