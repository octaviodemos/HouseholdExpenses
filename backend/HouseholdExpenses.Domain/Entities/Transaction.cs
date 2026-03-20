using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Domain.Entities;

/// <summary>
/// Registro de uma movimentação financeira (despesa ou receita) vinculada a uma pessoa e a uma categoria.
/// </summary>
/// <remarks>
/// <para>
/// A categoria deve ser compatível com <see cref="Type"/> (ver <see cref="Category.IsCompatibleWith"/>).
/// </para>
/// <para>
/// Pessoas menores de 18 anos só devem possuir transações do tipo <see cref="TransactionType.Expense"/> (regra a ser aplicada na camada de aplicação ao validar <see cref="PersonId"/>).
/// </para>
/// </remarks>
public class Transaction
{
    /// <summary>
    /// Construtor para materialização pelo Entity Framework Core.
    /// </summary>
    protected Transaction()
    {
        Description = string.Empty;
    }

    /// <summary>
    /// Cria uma nova transação com <see cref="Id"/> gerado automaticamente.
    /// </summary>
    /// <param name="description">Descrição (máximo 400 caracteres).</param>
    /// <param name="amount">Valor estritamente positivo.</param>
    /// <param name="type">Tipo da transação (despesa ou receita).</param>
    /// <param name="categoryId">Identificador da categoria.</param>
    /// <param name="personId">Identificador da pessoa.</param>
    public Transaction(
        string description,
        decimal amount,
        TransactionType type,
        Guid categoryId,
        Guid personId)
        : this()
    {
        Id = Guid.NewGuid();
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A descrição é obrigatória.", nameof(description));
        if (description.Length > 400)
            throw new ArgumentException("A descrição não pode exceder 400 caracteres.", nameof(description));
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "O valor deve ser positivo.");

        Description = description;
        Amount = amount;
        Type = type;
        CategoryId = categoryId;
        PersonId = personId;
    }

    public Guid Id { get; private set; }

    /// <summary>
    /// Descrição da transação (máximo 400 caracteres).
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Valor monetário (sempre positivo).
    /// </summary>
    public decimal Amount { get; private set; }

    public TransactionType Type { get; private set; }

    public Guid CategoryId { get; private set; }

    public Guid PersonId { get; private set; }

    /// <summary>
    /// Categoria da transação (preenchida pelo EF Core quando a navegação for carregada).
    /// </summary>
    public Category? Category { get; private set; }

    /// <summary>
    /// Pessoa titular da transação (preenchida pelo EF Core quando a navegação for carregada).
    /// </summary>
    public Person? Person { get; private set; }
}
