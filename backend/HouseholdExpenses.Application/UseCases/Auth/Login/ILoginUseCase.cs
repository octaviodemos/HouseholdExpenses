using HouseholdExpenses.Communication.Requests.Auth;
using HouseholdExpenses.Communication.Responses.Auth;

namespace HouseholdExpenses.Application.UseCases.Auth.Login;

/// <summary>
/// Autentica usuário por e-mail e senha e retorna JWT.
/// </summary>
public interface ILoginUseCase
{
    /// <summary>Valida credenciais e emite token.</summary>
    Task<AuthResponse> ExecuteAsync(LoginRequest request);
}
