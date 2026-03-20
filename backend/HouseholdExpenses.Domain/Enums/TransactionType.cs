namespace HouseholdExpenses.Domain.Enums;

/// <summary>
/// Classifica o tipo de movimentação financeira (despesa ou receita).
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Despesa (gasto).
    /// </summary>
    Expense = 1,

    /// <summary>
    /// Receita (entrada).
    /// </summary>
    Income = 2
}
