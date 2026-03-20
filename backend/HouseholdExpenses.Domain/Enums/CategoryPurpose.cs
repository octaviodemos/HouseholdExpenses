namespace HouseholdExpenses.Domain.Enums;

/// <summary>
/// Define para que tipos de transação uma categoria pode ser utilizada.
/// Deve ser respeitado em conjunto com <see cref="TransactionType"/> (ver <see cref="HouseholdExpenses.Domain.Entities.Category.IsCompatibleWith"/>).
/// </summary>
public enum CategoryPurpose
{
    /// <summary>
    /// Categoria aplicável apenas a despesas.
    /// </summary>
    Expense = 1,

    /// <summary>
    /// Categoria aplicável apenas a receitas.
    /// </summary>
    Income = 2,

    /// <summary>
    /// Categoria aplicável tanto a despesas quanto a receitas.
    /// </summary>
    Both = 3
}
