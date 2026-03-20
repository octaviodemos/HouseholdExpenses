using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Domain.Entities;
using HouseholdExpenses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Infrastructure.Repositories;

/// <summary>
/// Implementação de <see cref="ITransactionRepository"/> usando o <see cref="HouseholdExpensesDbContext"/>.
/// </summary>
public sealed class TransactionRepository : ITransactionRepository
{
    private readonly HouseholdExpensesDbContext _context;

    /// <summary>Inicializa o repositório com o contexto EF Core.</summary>
    /// <param name="context">Contexto de dados.</param>
    public TransactionRepository(HouseholdExpensesDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<List<Transaction>> GetAllAsync() =>
        await _context.Transactions
            .AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.Person)
            .ToListAsync();

    /// <inheritdoc />
    public async Task<List<Transaction>> GetByPersonIdAsync(Guid personId) =>
        await _context.Transactions
            .AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.Person)
            .Where(t => t.PersonId == personId)
            .ToListAsync();

    /// <inheritdoc />
    public async Task AddAsync(Transaction transaction) =>
        await _context.Transactions.AddAsync(transaction);
}
