using System.Net.Mail;
using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Application.Services;
using HouseholdExpenses.Communication.Requests.Auth;
using HouseholdExpenses.Communication.Responses.Auth;
using HouseholdExpenses.Domain.Entities;
using HouseholdExpenses.Exception;

namespace HouseholdExpenses.Application.UseCases.Auth.Register;

/// <summary>
/// Implementação do cadastro: valida dados, garante e-mail único, persiste hash e devolve JWT.
/// </summary>
public sealed class RegisterUseCase : IRegisterUseCase
{
    private const int MinPasswordLength = 6;

    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    /// <summary>Inicializa o caso de uso com dependências de persistência e segurança.</summary>
    public RegisterUseCase(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    /// <inheritdoc />
    public async Task<AuthResponse> ExecuteAsync(RegisterRequest request)
    {
        Validate(request);

        var normalizedEmail = NormalizeEmail(request.Email);

        if (await _userRepository.ExistsAsync(normalizedEmail))
            throw new ErrorOnValidationException(new List<string> { "E-mail já cadastrado." });

        var hash = _passwordHasher.Hash(request.Password);
        var user = User.Create(normalizedEmail, hash);

        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        var token = _jwtService.GenerateToken(user);
        return new AuthResponse(token, user.Email);
    }

    private static void Validate(RegisterRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Email))
            errors.Add("O e-mail é obrigatório.");
        else if (!IsValidEmailFormat(request.Email.Trim()))
            errors.Add("O e-mail informado é inválido.");

        if (string.IsNullOrEmpty(request.Password))
            errors.Add("A senha é obrigatória.");
        else if (request.Password.Length < MinPasswordLength)
            errors.Add($"A senha deve ter no mínimo {MinPasswordLength} caracteres.");

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }

    private static bool IsValidEmailFormat(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
