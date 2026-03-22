using HouseholdExpenses.Domain.Entities;

namespace HouseholdExpenses.Application.Services;

/// <summary>
/// Gera tokens JWT para usuários autenticados.
/// </summary>
public interface IJwtService
{
    /// <summary>Emite um JWT com claims de identificação e e-mail.</summary>
    /// <param name="user">Usuário autenticado.</param>
    /// <returns>Token serializado (Bearer).</returns>
    string GenerateToken(User user);
}
