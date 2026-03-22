using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Domain.Entities;
using HouseholdExpenses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Infrastructure.Repositories;

/// <summary>
/// Implementação de <see cref="IUserRepository"/> com EF Core.
/// </summary>
public sealed class UserRepository : IUserRepository
{
    private readonly HouseholdExpensesDbContext _context;

    /// <summary>Inicializa o repositório com o contexto EF Core.</summary>
    public UserRepository(HouseholdExpensesDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

    /// <inheritdoc />
    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string email) =>
        await _context.Users.AnyAsync(u => u.Email == email);
}
