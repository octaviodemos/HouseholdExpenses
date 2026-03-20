using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Communication.Requests.Category;
using HouseholdExpenses.Communication.Responses.Category;
using HouseholdExpenses.Domain.Enums;
using HouseholdExpenses.Exception;
using DomainCategory = HouseholdExpenses.Domain.Entities.Category;

namespace HouseholdExpenses.Application.UseCases.Category.Create;

/// <summary>
/// Cria categoria com validação de descrição e de valor definido em <see cref="CategoryPurpose"/>.
/// </summary>
public sealed class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>Inicializa o caso de uso com repositório e unidade de trabalho.</summary>
    public CreateCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<CategoryResponse> ExecuteAsync(CreateCategoryRequest request)
    {
        Validate(request);

        var category = new DomainCategory(request.Description, request.Purpose);
        await _categoryRepository.AddAsync(category);
        await _unitOfWork.CommitAsync();

        return new CategoryResponse(category.Id, category.Description, category.Purpose);
    }

    private static void Validate(CreateCategoryRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Description))
            errors.Add("A descrição é obrigatória.");
        else if (request.Description.Length > 400)
            errors.Add("A descrição não pode exceder 400 caracteres.");

        if (!Enum.IsDefined(request.Purpose))
            errors.Add("O valor de finalidade da categoria é inválido.");

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }
}
