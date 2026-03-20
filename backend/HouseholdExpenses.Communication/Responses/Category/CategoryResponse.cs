using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Communication.Responses.Category;

/// <summary>
/// Representação de uma categoria retornada pela API. Apenas dados de saída, sem lógica de negócio.
/// </summary>
/// <param name="Id">Identificador único da categoria.</param>
/// <param name="Description">Descrição da categoria.</param>
/// <param name="Purpose">Finalidade da categoria em relação ao tipo de transação.</param>
public record CategoryResponse(Guid Id, string Description, CategoryPurpose Purpose);
