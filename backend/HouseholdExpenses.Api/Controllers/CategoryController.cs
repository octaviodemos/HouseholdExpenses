using HouseholdExpenses.Application.UseCases.Category.Create;
using HouseholdExpenses.Application.UseCases.Category.GetAll;
using HouseholdExpenses.Application.UseCases.Category.GetTotals;
using HouseholdExpenses.Communication.Requests.Category;
using HouseholdExpenses.Communication.Responses.Category;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdExpenses.Api.Controllers;

/// <summary>
/// Endpoints para categorias de transações.
/// </summary>
[ApiController]
[Route("api/categories")]
public sealed class CategoryController : ControllerBase
{
    private readonly ICreateCategoryUseCase _createCategoryUseCase;
    private readonly IGetAllCategoriesUseCase _getAllCategoriesUseCase;
    private readonly IGetCategoryTotalsUseCase _getCategoryTotalsUseCase;

    /// <summary>Inicializa o controller com os casos de uso de categoria.</summary>
    public CategoryController(
        ICreateCategoryUseCase createCategoryUseCase,
        IGetAllCategoriesUseCase getAllCategoriesUseCase,
        IGetCategoryTotalsUseCase getCategoryTotalsUseCase)
    {
        _createCategoryUseCase = createCategoryUseCase;
        _getAllCategoriesUseCase = getAllCategoriesUseCase;
        _getCategoryTotalsUseCase = getCategoryTotalsUseCase;
    }

    /// <summary>Cadastra uma nova categoria.</summary>
    /// <param name="request">Descrição e finalidade (despesa, receita ou ambos).</param>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var response = await _createCategoryUseCase.ExecuteAsync(request);
        return Created($"/api/categories/{response.Id}", response);
    }

    /// <summary>Lista todas as categorias.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllCategories()
    {
        var list = await _getAllCategoriesUseCase.ExecuteAsync();
        return Ok(list);
    }

    /// <summary>Retorna totais de receita, despesa e saldo por categoria e o resumo geral.</summary>
    [HttpGet("totals")]
    [ProducesResponseType(typeof(CategoryTotalsResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryTotalsResult>> GetCategoryTotals()
    {
        var result = await _getCategoryTotalsUseCase.ExecuteAsync();
        return Ok(result);
    }
}
