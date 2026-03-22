using HouseholdExpenses.Domain.Entities;

namespace HouseholdExpenses.Application.Repositories;

/// <summary>
/// Contrato de persistência para <see cref="User"/>.
/// </summary>
public interface IUserRepository
{
    /// <summary>Obtém usuário pelo e-mail (comparação exata ao valor persistido).</summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>Adiciona um novo usuário.</summary>
    Task AddAsync(User user);

    /// <summary>Indica se já existe usuário com o e-mail informado.</summary>
    Task<bool> ExistsAsync(string email);
}
