namespace HouseholdExpenses.Application.UseCases.Person.GetTotals;

/// <summary>
/// Calcula receitas, despesas e saldo por pessoa e o resumo global.
/// </summary>
public interface IGetPersonTotalsUseCase
{
    /// <summary>Executa o cálculo agregado com base nas transações persistidas.</summary>
    Task<PersonTotalsResult> ExecuteAsync();
}
