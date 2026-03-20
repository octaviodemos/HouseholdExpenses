using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Communication.Responses.Transaction;

/// <summary>
/// Representação de uma transação retornada pela API, incluindo dados desnormalizados de categoria e pessoa para exibição.
/// Apenas dados de saída, sem lógica de negócio.
/// </summary>
/// <param name="Id">Identificador único da transação.</param>
/// <param name="Description">Descrição da transação.</param>
/// <param name="Amount">Valor monetário.</param>
/// <param name="Type">Tipo da transação (despesa ou receita).</param>
/// <param name="CategoryId">Identificador da categoria.</param>
/// <param name="CategoryDescription">Descrição da categoria (para listagens sem join adicional no cliente).</param>
/// <param name="PersonId">Identificador da pessoa.</param>
/// <param name="PersonName">Nome da pessoa (para listagens sem join adicional no cliente).</param>
public record TransactionResponse(
    Guid Id,
    string Description,
    decimal Amount,
    TransactionType Type,
    Guid CategoryId,
    string CategoryDescription,
    Guid PersonId,
    string PersonName);
