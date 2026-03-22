namespace HouseholdExpenses.Communication.Requests.Auth;

/// <summary>
/// Dados para cadastro de novo usuário.
/// </summary>
/// <param name="Email">E-mail de acesso.</param>
/// <param name="Password">Senha em texto plano (será hasheada na aplicação).</param>
public record RegisterRequest(string Email, string Password);
