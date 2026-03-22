namespace HouseholdExpenses.Communication.Responses.Auth;

/// <summary>
/// Resposta de autenticação com token JWT.
/// </summary>
/// <param name="Token">JWT de acesso.</param>
/// <param name="Email">E-mail do usuário autenticado.</param>
public record AuthResponse(string Token, string Email);
