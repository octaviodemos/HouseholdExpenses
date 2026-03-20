using HouseholdExpenses.Application.UseCases.Transaction.Create;
using HouseholdExpenses.Application.UseCases.Transaction.GetAll;
using HouseholdExpenses.Communication.Requests.Transaction;
using HouseholdExpenses.Communication.Responses.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdExpenses.Api.Controllers;

/// <summary>
/// Endpoints para transações financeiras (despesas e receitas).
/// </summary>
[ApiController]
[Route("api/transactions")]
public sealed class TransactionController : ControllerBase
{
    private readonly ICreateTransactionUseCase _createTransactionUseCase;
    private readonly IGetAllTransactionsUseCase _getAllTransactionsUseCase;

    /// <summary>Inicializa o controller com os casos de uso de transação.</summary>
    public TransactionController(
        ICreateTransactionUseCase createTransactionUseCase,
        IGetAllTransactionsUseCase getAllTransactionsUseCase)
    {
        _createTransactionUseCase = createTransactionUseCase;
        _getAllTransactionsUseCase = getAllTransactionsUseCase;
    }

    /// <summary>Registra uma nova transação vinculada a pessoa e categoria.</summary>
    /// <param name="request">Dados da transação.</param>
    [HttpPost]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionResponse>> CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        var response = await _createTransactionUseCase.ExecuteAsync(request);
        return Created($"/api/transactions/{response.Id}", response);
    }

    /// <summary>Lista todas as transações com nome da pessoa e descrição da categoria.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TransactionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TransactionResponse>>> GetAllTransactions()
    {
        var list = await _getAllTransactionsUseCase.ExecuteAsync();
        return Ok(list);
    }
}
