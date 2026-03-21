using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Communication.Responses;
using HouseholdExpenses.Communication.Responses.Category;
using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Application.UseCases.Category.GetTotals;

/// <summary>
/// Agrega transações por categoria e produz totais individuais e <see cref="SummaryTotalsResponse"/>.
/// </summary>
public sealed class GetCategoryTotalsUseCase : IGetCategoryTotalsUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    /// <summary>Inicializa o caso de uso com repositórios de categorias e transações.</summary>
    public GetCategoryTotalsUseCase(
        ICategoryRepository categoryRepository,
        ITransactionRepository transactionRepository)
    {
        _categoryRepository = categoryRepository;
        _transactionRepository = transactionRepository;
    }

    /// <inheritdoc />
    public async Task<CategoryTotalsResult> ExecuteAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var allTransactions = await _transactionRepository.GetAllAsync();

        var byCategory = allTransactions
            .GroupBy(t => t.CategoryId)
            .ToDictionary(g => g.Key, g => g.ToList());

        decimal grandIncome = 0;
        decimal grandExpense = 0;

        var list = new List<CategoryTotalsResponse>(categories.Count);

        foreach (var category in categories)
        {
            byCategory.TryGetValue(category.Id, out var txs);
            txs ??= [];

            var income = txs.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var expense = txs.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            var balance = income - expense;

            grandIncome += income;
            grandExpense += expense;

            list.Add(new CategoryTotalsResponse(
                category.Id,
                category.Description,
                income,
                expense,
                balance));
        }

        var summary = new SummaryTotalsResponse(
            grandIncome,
            grandExpense,
            grandIncome - grandExpense);

        return new CategoryTotalsResult(list, summary);
    }
}
