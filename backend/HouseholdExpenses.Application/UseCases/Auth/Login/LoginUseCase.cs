using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Application.Services;
using HouseholdExpenses.Communication.Requests.Auth;
using HouseholdExpenses.Communication.Responses.Auth;
using HouseholdExpenses.Exception;

namespace HouseholdExpenses.Application.UseCases.Auth.Login;

/// <summary>
/// Implementação do login: mensagens genéricas em falha para não revelar se o e-mail existe.
/// </summary>
public sealed class LoginUseCase : ILoginUseCase
{
    private const string InvalidCredentialsMessage = "Email ou senha inválidos.";

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    /// <summary>Inicializa o caso de uso com repositório e serviços de segurança.</summary>
    public LoginUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    /// <inheritdoc />
    public async Task<AuthResponse> ExecuteAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrEmpty(request.Password))
            throw new ErrorOnValidationException(new List<string> { InvalidCredentialsMessage });

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail);

        if (user is null)
            throw new ErrorOnValidationException(new List<string> { InvalidCredentialsMessage });

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new ErrorOnValidationException(new List<string> { InvalidCredentialsMessage });

        var token = _jwtService.GenerateToken(user);
        return new AuthResponse(token, user.Email);
    }
}
