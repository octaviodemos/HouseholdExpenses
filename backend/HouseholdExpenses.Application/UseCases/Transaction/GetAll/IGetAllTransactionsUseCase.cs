using HouseholdExpenses.Communication.Responses.Transaction;

namespace HouseholdExpenses.Application.UseCases.Transaction.GetAll;

/// <summary>
/// Lista todas as transações com dados de categoria e pessoa para exibição.
/// </summary>
public interface IGetAllTransactionsUseCase
{
    /// <summary>Retorna transações com descrições de categoria e nome da pessoa.</summary>
    Task<List<TransactionResponse>> ExecuteAsync();
}
