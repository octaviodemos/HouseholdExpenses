using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Communication.Responses.Category;

namespace HouseholdExpenses.Application.UseCases.Category.GetAll;

/// <summary>
/// Obtém todas as categorias e projeta para <see cref="CategoryResponse"/>.
/// </summary>
public sealed class GetAllCategoriesUseCase : IGetAllCategoriesUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>Inicializa o caso de uso com o repositório de categorias.</summary>
    public GetAllCategoriesUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    /// <inheritdoc />
    public async Task<List<CategoryResponse>> ExecuteAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.ConvertAll(c => new CategoryResponse(c.Id, c.Description, c.Purpose));
    }
}
