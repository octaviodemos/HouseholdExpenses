using HouseholdExpenses.Communication.Requests.Category;
using HouseholdExpenses.Communication.Responses.Category;

namespace HouseholdExpenses.Application.UseCases.Category.Create;

/// <summary>
/// Cadastra uma nova categoria após validar descrição e finalidade.
/// </summary>
public interface ICreateCategoryUseCase
{
    /// <summary>Executa o caso de uso de criação de categoria.</summary>
    Task<CategoryResponse> ExecuteAsync(CreateCategoryRequest request);
}
