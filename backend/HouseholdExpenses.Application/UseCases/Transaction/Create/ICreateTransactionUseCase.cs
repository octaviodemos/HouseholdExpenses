using HouseholdExpenses.Communication.Requests.Transaction;
using HouseholdExpenses.Communication.Responses.Transaction;

namespace HouseholdExpenses.Application.UseCases.Transaction.Create;

/// <summary>
/// Registra uma transação após validar pessoa, categoria, compatibilidade e regras para menores.
/// </summary>
public interface ICreateTransactionUseCase
{
    /// <summary>Executa o caso de uso de criação de transação.</summary>
    Task<TransactionResponse> ExecuteAsync(CreateTransactionRequest request);
}
