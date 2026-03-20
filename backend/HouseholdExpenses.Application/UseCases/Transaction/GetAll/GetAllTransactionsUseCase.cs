using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Communication.Responses.Transaction;

namespace HouseholdExpenses.Application.UseCases.Transaction.GetAll;

/// <summary>
/// Obtém todas as transações e monta <see cref="TransactionResponse"/> com dados desnormalizados.
/// </summary>
public sealed class GetAllTransactionsUseCase : IGetAllTransactionsUseCase
{
    private readonly ITransactionRepository _transactionRepository;

    /// <summary>Inicializa o caso de uso com o repositório de transações.</summary>
    public GetAllTransactionsUseCase(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    /// <inheritdoc />
    public async Task<List<TransactionResponse>> ExecuteAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();

        return transactions.ConvertAll(t => new TransactionResponse(
            t.Id,
            t.Description,
            t.Amount,
            t.Type,
            t.CategoryId,
            t.Category?.Description ?? string.Empty,
            t.PersonId,
            t.Person?.Name ?? string.Empty));
    }
}
