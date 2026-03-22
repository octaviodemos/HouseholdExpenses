using HouseholdExpenses.Communication.Requests.Auth;
using HouseholdExpenses.Communication.Responses.Auth;

namespace HouseholdExpenses.Application.UseCases.Auth.Register;

/// <summary>
/// Cadastra usuário com validações e retorna token JWT.
/// </summary>
public interface IRegisterUseCase
{
    /// <summary>Executa o registro e autenticação inicial.</summary>
    Task<AuthResponse> ExecuteAsync(RegisterRequest request);
}
