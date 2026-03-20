using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Domain.Entities;
using HouseholdExpenses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Infrastructure.Repositories;

/// <summary>
/// Implementação de <see cref="IPersonRepository"/> usando o <see cref="HouseholdExpensesDbContext"/>.
/// </summary>
public sealed class PersonRepository : IPersonRepository
{
    private readonly HouseholdExpensesDbContext _context;

    /// <summary>Inicializa o repositório com o contexto EF Core.</summary>
    /// <param name="context">Contexto de dados.</param>
    public PersonRepository(HouseholdExpensesDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<List<Person>> GetAllAsync() =>
        await _context.Persons.AsNoTracking().ToListAsync();

    /// <inheritdoc />
    public async Task<Person?> GetByIdAsync(Guid id) =>
        await _context.Persons.FindAsync(id);

    /// <inheritdoc />
    public async Task<Person?> GetByIdWithTransactionsAsync(Guid id) =>
        await _context.Persons
            .Include(p => p.Transactions)
            .FirstOrDefaultAsync(p => p.Id == id);

    /// <inheritdoc />
    public async Task AddAsync(Person person) =>
        await _context.Persons.AddAsync(person);

    /// <inheritdoc />
    public Task UpdateAsync(Person person)
    {
        _context.Persons.Update(person);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(Person person)
    {
        _context.Persons.Remove(person);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Guid id) =>
        await _context.Persons.AnyAsync(p => p.Id == id);
}
