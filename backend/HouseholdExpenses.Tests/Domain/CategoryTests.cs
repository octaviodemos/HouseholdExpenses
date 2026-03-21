using HouseholdExpenses.Domain.Entities;
using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Tests.Domain;

/// <summary>
/// Testes unitários da entidade <see cref="Category"/>, em especial a compatibilidade com o tipo de transação.
/// </summary>
public sealed class CategoryTests
{
    [Fact]
    public void IsCompatibleWith_WhenPurposeIsBoth_ReturnsTrue()
    {
        // Arrange
        var category = new Category("Mista", CategoryPurpose.Both);

        // Act
        var expenseOk = category.IsCompatibleWith(TransactionType.Expense);
        var incomeOk = category.IsCompatibleWith(TransactionType.Income);

        // Assert
        Assert.True(expenseOk);
        Assert.True(incomeOk);
    }

    [Fact]
    public void IsCompatibleWith_WhenPurposeIsExpense_ReturnsTrueForExpense()
    {
        // Arrange
        var category = new Category("Só despesa", CategoryPurpose.Expense);

        // Act
        var result = category.IsCompatibleWith(TransactionType.Expense);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsCompatibleWith_WhenPurposeIsExpense_ReturnsFalseForIncome()
    {
        // Arrange
        var category = new Category("Só despesa", CategoryPurpose.Expense);

        // Act
        var result = category.IsCompatibleWith(TransactionType.Income);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsCompatibleWith_WhenPurposeIsIncome_ReturnsTrueForIncome()
    {
        // Arrange
        var category = new Category("Só receita", CategoryPurpose.Income);

        // Act
        var result = category.IsCompatibleWith(TransactionType.Income);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsCompatibleWith_WhenPurposeIsIncome_ReturnsFalseForExpense()
    {
        // Arrange
        var category = new Category("Só receita", CategoryPurpose.Income);

        // Act
        var result = category.IsCompatibleWith(TransactionType.Expense);

        // Assert
        Assert.False(result);
    }
}
