namespace HouseholdExpenses.Communication.Responses;

/// <summary>
/// Totais globais de receitas, despesas e saldo. Destinado ao rodapé ou resumo geral das consultas de totais.
/// </summary>
/// <param name="TotalIncome">Soma de todas as receitas consideradas na consulta.</param>
/// <param name="TotalExpense">Soma de todas as despesas consideradas na consulta.</param>
/// <param name="Balance">Saldo agregado (receitas menos despesas, conforme definido na camada de aplicação).</param>
public record SummaryTotalsResponse(decimal TotalIncome, decimal TotalExpense, decimal Balance);
