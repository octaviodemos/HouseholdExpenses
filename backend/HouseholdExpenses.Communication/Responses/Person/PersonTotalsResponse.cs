namespace HouseholdExpenses.Communication.Responses.Person;

/// <summary>
/// Totais financeiros agregados por pessoa (receitas, despesas e saldo). Usado em consultas que expõem resumo por pessoa.
/// </summary>
/// <param name="Id">Identificador único da pessoa.</param>
/// <param name="Name">Nome da pessoa.</param>
/// <param name="TotalIncome">Soma das receitas da pessoa.</param>
/// <param name="TotalExpense">Soma das despesas da pessoa.</param>
/// <param name="Balance">Saldo (receitas menos despesas, conforme definido na camada de aplicação).</param>
public record PersonTotalsResponse(
    Guid Id,
    string Name,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance);
