using HouseholdExpenses.Domain.Entities;
using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Tests.Domain;

/// <summary>
/// Testes unitários da entidade <see cref="Transaction"/>: invariantes do construtor (descrição e valor).
/// </summary>
public sealed class TransactionTests
{
    [Fact]
    public void Constructor_WhenValid_SetsProperties()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        // Act
        var transaction = new Transaction("Compra teste", 42.5m, TransactionType.Expense, categoryId, personId);

        // Assert
        Assert.Equal("Compra teste", transaction.Description);
        Assert.Equal(42.5m, transaction.Amount);
        Assert.Equal(TransactionType.Expense, transaction.Type);
        Assert.Equal(categoryId, transaction.CategoryId);
        Assert.Equal(personId, transaction.PersonId);
    }

    [Fact]
    public void Constructor_WhenDescriptionIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Transaction("  ", 10m, TransactionType.Expense, categoryId, personId));
    }

    [Fact]
    public void Constructor_WhenAmountIsNotPositive_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Transaction("Ok", 0m, TransactionType.Expense, categoryId, personId));
    }
}
