using HouseholdExpenses.Communication.Responses.Person;

namespace HouseholdExpenses.Application.UseCases.Person.GetAll;

/// <summary>
/// Lista todas as pessoas cadastradas.
/// </summary>
public interface IGetAllPersonsUseCase
{
    /// <summary>Retorna a lista de pessoas em formato de resposta.</summary>
    Task<List<PersonResponse>> ExecuteAsync();
}
