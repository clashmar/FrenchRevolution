using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Domain.Entities;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class GetAllCharactersHandler(
    ICharacterRepository repository
) : IRequestHandler<GetAllCharactersQuery, IEnumerable<Character>>
{
    public async Task<IEnumerable<Character>> Handle(
        GetAllCharactersQuery query,
        CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync();
    }
}