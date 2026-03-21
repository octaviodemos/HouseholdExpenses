namespace HouseholdExpenses.Communication.Responses.Category;

/// <summary>
/// Totais financeiros agregados por categoria (receitas, despesas e saldo).
/// </summary>
/// <param name="Id">Identificador único da categoria.</param>
/// <param name="Description">Descrição da categoria.</param>
/// <param name="TotalIncome">Soma das receitas classificadas nesta categoria.</param>
/// <param name="TotalExpense">Soma das despesas classificadas nesta categoria.</param>
/// <param name="Balance">Saldo (receitas menos despesas) para a categoria.</param>
public record CategoryTotalsResponse(
    Guid Id,
    string Description,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance);
