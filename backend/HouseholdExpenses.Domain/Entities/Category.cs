using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Domain.Entities;

/// <summary>
/// Categoria utilizada para classificar transações.
/// </summary>
/// <remarks>
/// A categoria deve ser compatível com o tipo da transação (<see cref="IsCompatibleWith"/>).
/// </remarks>
public class Category
{
    /// <summary>
    /// Construtor para materialização pelo Entity Framework Core.
    /// </summary>
    protected Category()
    {
        Description = string.Empty;
        Transactions = new List<Transaction>();
    }

    /// <summary>
    /// Cria uma nova categoria com <see cref="Id"/> gerado automaticamente.
    /// </summary>
    /// <param name="description">Descrição (máximo 400 caracteres).</param>
    /// <param name="purpose">Finalidade da categoria em relação ao tipo de transação.</param>
    public Category(string description, CategoryPurpose purpose)
        : this()
    {
        Id = Guid.NewGuid();
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A descrição é obrigatória.", nameof(description));
        if (description.Length > 400)
            throw new ArgumentException("A descrição não pode exceder 400 caracteres.", nameof(description));

        Description = description;
        Purpose = purpose;
    }

    public Guid Id { get; private set; }

    /// <summary>
    /// Descrição da categoria (máximo 400 caracteres).
    /// </summary>
    public string Description { get; private set; }

    public CategoryPurpose Purpose { get; private set; }

    /// <summary>
    /// Transações classificadas nesta categoria.
    /// </summary>
    public ICollection<Transaction> Transactions { get; private set; }

    /// <summary>
    /// Verifica se a categoria pode ser usada com o tipo de transação informado.
    /// Retorna <see langword="true"/> quando <see cref="Purpose"/> é <see cref="CategoryPurpose.Both"/>,
    /// quando é <see cref="CategoryPurpose.Expense"/> e o tipo é <see cref="TransactionType.Expense"/>,
    /// ou quando é <see cref="CategoryPurpose.Income"/> e o tipo é <see cref="TransactionType.Income"/>.
    /// </summary>
    /// <param name="type">Tipo da transação.</param>
    /// <returns><see langword="true"/> se a combinação for válida.</returns>
    public bool IsCompatibleWith(TransactionType type) =>
        Purpose == CategoryPurpose.Both
        || (Purpose == CategoryPurpose.Expense && type == TransactionType.Expense)
        || (Purpose == CategoryPurpose.Income && type == TransactionType.Income);
}
