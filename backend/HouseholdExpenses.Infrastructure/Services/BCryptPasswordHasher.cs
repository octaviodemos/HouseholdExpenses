using HouseholdExpenses.Application.Services;

namespace HouseholdExpenses.Infrastructure.Services;

/// <summary>
/// Hash e verificação de senhas com BCrypt.
/// </summary>
public sealed class BCryptPasswordHasher : IPasswordHasher
{
    /// <inheritdoc />
    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

    /// <inheritdoc />
    public bool Verify(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
