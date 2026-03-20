using System.Net;
using System.Net.Mime;
using HouseholdExpenses.Exception;

namespace HouseholdExpenses.Api.Middleware;

/// <summary>
/// Intercepta exceções não tratadas e devolve JSON padronizado com a propriedade <c>errors</c>.
/// </summary>
public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    /// <summary>Inicializa o middleware com o próximo delegado e o logger.</summary>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>Executa o pipeline e trata exceções conhecidas do domínio.</summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (System.Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        switch (exception)
        {
            case ErrorOnValidationException validation:
                _logger.LogWarning(exception, "Falha de validação.");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new { errors = validation.Errors });
                break;

            case NotFoundException notFound:
                _logger.LogInformation(exception, "Recurso não encontrado.");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsJsonAsync(new { errors = notFound.Errors });
                break;

            default:
                _logger.LogError(exception, "Erro não tratado.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { errors = new[] { "Erro interno do servidor." } });
                break;
        }
    }
}
