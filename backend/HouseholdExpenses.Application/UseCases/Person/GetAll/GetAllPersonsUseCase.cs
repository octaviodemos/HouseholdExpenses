using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Communication.Responses.Person;

namespace HouseholdExpenses.Application.UseCases.Person.GetAll;

/// <summary>
/// Obtém todas as pessoas e projeta para <see cref="PersonResponse"/>.
/// </summary>
public sealed class GetAllPersonsUseCase : IGetAllPersonsUseCase
{
    private readonly IPersonRepository _personRepository;

    /// <summary>Inicializa o caso de uso com o repositório de pessoas.</summary>
    public GetAllPersonsUseCase(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    /// <inheritdoc />
    public async Task<List<PersonResponse>> ExecuteAsync()
    {
        var persons = await _personRepository.GetAllAsync();
        return persons.ConvertAll(p => new PersonResponse(p.Id, p.Name, p.Age));
    }
}
