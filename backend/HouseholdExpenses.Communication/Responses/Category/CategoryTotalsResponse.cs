namespace HouseholdExpenses.Communication.Responses.Category;

/// <summary>
/// Totais financeiros agregados por categoria (receitas, despesas e saldo). Usado em consultas que expõem resumo por categoria.
/// </summary>
/// <param name="Id">Identificador único da categoria.</param>
/// <param name="Description">Descrição da categoria.</param>
/// <param name="TotalIncome">Soma das receitas classificadas nesta categoria.</param>
/// <param name="TotalExpense">Soma das despesas classificadas nesta categoria.</param>
/// <param name="Balance">Saldo (receitas menos despesas, conforme definido na camada de aplicação).</param>
public record CategoryTotalsResponse(
    Guid Id,
    string Description,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance);
