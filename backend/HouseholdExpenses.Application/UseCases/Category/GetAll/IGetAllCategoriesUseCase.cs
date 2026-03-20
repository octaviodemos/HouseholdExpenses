using HouseholdExpenses.Communication.Responses.Category;

namespace HouseholdExpenses.Application.UseCases.Category.GetAll;

/// <summary>
/// Lista todas as categorias cadastradas.
/// </summary>
public interface IGetAllCategoriesUseCase
{
    /// <summary>Retorna as categorias em formato de resposta.</summary>
    Task<List<CategoryResponse>> ExecuteAsync();
}
