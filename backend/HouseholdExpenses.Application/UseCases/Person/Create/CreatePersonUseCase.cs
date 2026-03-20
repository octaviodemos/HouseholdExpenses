using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Communication.Requests.Person;
using HouseholdExpenses.Communication.Responses.Person;
using HouseholdExpenses.Exception;
using DomainPerson = HouseholdExpenses.Domain.Entities.Person;

namespace HouseholdExpenses.Application.UseCases.Person.Create;

/// <summary>
/// Implementação do cadastro de pessoa com validações de nome e idade.
/// </summary>
public sealed class CreatePersonUseCase : ICreatePersonUseCase
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>Inicializa o caso de uso com repositório e unidade de trabalho.</summary>
    public CreatePersonUseCase(IPersonRepository personRepository, IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<PersonResponse> ExecuteAsync(CreatePersonRequest request)
    {
        Validate(request);

        var person = new DomainPerson(request.Name, request.Age);
        await _personRepository.AddAsync(person);
        await _unitOfWork.CommitAsync();

        return ToResponse(person);
    }

    private static void Validate(CreatePersonRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add("O nome é obrigatório.");
        else if (request.Name.Length > 200)
            errors.Add("O nome não pode exceder 200 caracteres.");

        if (request.Age <= 0)
            errors.Add("A idade deve ser maior que zero.");

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }

    private static PersonResponse ToResponse(DomainPerson person) =>
        new(person.Id, person.Name, person.Age);
}
