namespace HouseholdExpenses.Application.UseCases.Category.GetTotals;

/// <summary>
/// Calcula receitas, despesas e saldo por categoria e o resumo global.
/// </summary>
public interface IGetCategoryTotalsUseCase
{
    /// <summary>Executa o cálculo agregado com base nas categorias e transações persistidas.</summary>
    Task<CategoryTotalsResult> ExecuteAsync();
}
