using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Infrastructure.Data;

namespace HouseholdExpenses.Infrastructure.UnitOfWork;

/// <summary>
/// Confirma alterações pendentes no <see cref="HouseholdExpensesDbContext"/>.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly HouseholdExpensesDbContext _context;

    /// <summary>Inicializa a unidade de trabalho com o contexto EF Core.</summary>
    /// <param name="context">Contexto de dados.</param>
    public UnitOfWork(HouseholdExpensesDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public Task CommitAsync() =>
        _context.SaveChangesAsync();
}
