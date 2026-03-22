namespace HouseholdExpenses.Application.Services;

/// <summary>
/// Serviço de hash e verificação de senhas (implementação tipicamente com BCrypt na infraestrutura).
/// </summary>
public interface IPasswordHasher
{
    /// <summary>Gera hash seguro da senha em texto plano.</summary>
    string Hash(string password);

    /// <summary>Verifica se a senha confere com o hash armazenado.</summary>
    bool Verify(string password, string passwordHash);
}
