using HouseholdExpenses.Communication.Requests.Person;
using HouseholdExpenses.Communication.Responses.Person;

namespace HouseholdExpenses.Application.UseCases.Person.Create;

/// <summary>
/// Cadastra uma nova pessoa após validar os dados de entrada.
/// </summary>
public interface ICreatePersonUseCase
{
    /// <summary>Executa o caso de uso de criação de pessoa.</summary>
    Task<PersonResponse> ExecuteAsync(CreatePersonRequest request);
}
