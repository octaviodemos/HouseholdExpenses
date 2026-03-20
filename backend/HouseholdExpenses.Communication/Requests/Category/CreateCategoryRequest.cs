using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Communication.Requests.Category;

/// <summary>
/// Dados enviados pela API para cadastrar uma nova categoria. Contém apenas transporte de dados, sem regras de negócio.
/// </summary>
/// <param name="Description">Descrição da categoria.</param>
/// <param name="Purpose">Finalidade da categoria em relação ao tipo de transação (despesa, receita ou ambos).</param>
public record CreateCategoryRequest(string Description, CategoryPurpose Purpose);
