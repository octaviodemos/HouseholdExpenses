using HouseholdExpenses.Application.UseCases.Auth.Login;
using HouseholdExpenses.Application.UseCases.Auth.Register;
using HouseholdExpenses.Communication.Requests.Auth;
using HouseholdExpenses.Communication.Responses.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdExpenses.Api.Controllers;

/// <summary>
/// Endpoints públicos de cadastro e login (JWT).
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IRegisterUseCase _registerUseCase;
    private readonly ILoginUseCase _loginUseCase;

    /// <summary>Inicializa o controller com os casos de uso de autenticação.</summary>
    public AuthController(IRegisterUseCase registerUseCase, ILoginUseCase loginUseCase)
    {
        _registerUseCase = registerUseCase;
        _loginUseCase = loginUseCase;
    }

    /// <summary>Registra novo usuário e retorna token JWT.</summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        var response = await _registerUseCase.ExecuteAsync(request);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>Autentica usuário e retorna token JWT.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _loginUseCase.ExecuteAsync(request);
        return Ok(response);
    }
}
