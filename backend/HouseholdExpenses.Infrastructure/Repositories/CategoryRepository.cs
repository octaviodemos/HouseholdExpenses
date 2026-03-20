using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Domain.Entities;
using HouseholdExpenses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Infrastructure.Repositories;

/// <summary>
/// Implementação de <see cref="ICategoryRepository"/> usando o <see cref="HouseholdExpensesDbContext"/>.
/// </summary>
public sealed class CategoryRepository : ICategoryRepository
{
    private readonly HouseholdExpensesDbContext _context;

    /// <summary>Inicializa o repositório com o contexto EF Core.</summary>
    /// <param name="context">Contexto de dados.</param>
    public CategoryRepository(HouseholdExpensesDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetAllAsync() =>
        await _context.Categories.AsNoTracking().ToListAsync();

    /// <inheritdoc />
    public async Task<Category?> GetByIdAsync(Guid id) =>
        await _context.Categories.FindAsync(id);

    /// <inheritdoc />
    public async Task AddAsync(Category category) =>
        await _context.Categories.AddAsync(category);

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Guid id) =>
        await _context.Categories.AnyAsync(c => c.Id == id);
}
