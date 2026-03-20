using HouseholdExpenses.Communication.Requests.Person;
using HouseholdExpenses.Communication.Responses.Person;

namespace HouseholdExpenses.Application.UseCases.Person.Update;

/// <summary>
/// Atualiza os dados de uma pessoa existente.
/// </summary>
public interface IUpdatePersonUseCase
{
    /// <summary>Executa a atualização para o identificador informado.</summary>
    Task<PersonResponse> ExecuteAsync(Guid id, UpdatePersonRequest request);
}
