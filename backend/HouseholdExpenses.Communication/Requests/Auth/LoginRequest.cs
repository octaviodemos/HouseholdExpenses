namespace HouseholdExpenses.Communication.Requests.Auth;

/// <summary>
/// Dados para autenticação do usuário.
/// </summary>
/// <param name="Email">E-mail cadastrado.</param>
/// <param name="Password">Senha em texto plano.</param>
public record LoginRequest(string Email, string Password);
