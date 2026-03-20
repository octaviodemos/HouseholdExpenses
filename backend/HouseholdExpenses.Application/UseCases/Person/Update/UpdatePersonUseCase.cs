using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Communication.Requests.Person;
using HouseholdExpenses.Communication.Responses.Person;
using HouseholdExpenses.Exception;

namespace HouseholdExpenses.Application.UseCases.Person.Update;

/// <summary>
/// Atualiza nome e idade após localizar a pessoa e validar os dados.
/// </summary>
public sealed class UpdatePersonUseCase : IUpdatePersonUseCase
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>Inicializa o caso de uso com repositório e unidade de trabalho.</summary>
    public UpdatePersonUseCase(IPersonRepository personRepository, IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<PersonResponse> ExecuteAsync(Guid id, UpdatePersonRequest request)
    {
        var person = await _personRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Pessoa não encontrada.");

        Validate(request);

        person.Update(request.Name, request.Age);
        await _personRepository.UpdateAsync(person);
        await _unitOfWork.CommitAsync();

        return new PersonResponse(person.Id, person.Name, person.Age);
    }

    private static void Validate(UpdatePersonRequest request)
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
}
