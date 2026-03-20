using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Communication.Requests.Transaction;

/// <summary>
/// Dados enviados pela API para registrar uma nova transação. Contém apenas transporte de dados, sem regras de negócio.
/// </summary>
/// <param name="Description">Descrição da transação.</param>
/// <param name="Amount">Valor monetário.</param>
/// <param name="Type">Tipo da transação (despesa ou receita).</param>
/// <param name="CategoryId">Identificador da categoria.</param>
/// <param name="PersonId">Identificador da pessoa.</param>
public record CreateTransactionRequest(
    string Description,
    decimal Amount,
    TransactionType Type,
    Guid CategoryId,
    Guid PersonId);
