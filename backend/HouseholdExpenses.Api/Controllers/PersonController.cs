using HouseholdExpenses.Application.UseCases.Person.Create;
using HouseholdExpenses.Application.UseCases.Person.Delete;
using HouseholdExpenses.Application.UseCases.Person.GetAll;
using HouseholdExpenses.Application.UseCases.Person.GetTotals;
using HouseholdExpenses.Application.UseCases.Person.Update;
using HouseholdExpenses.Communication.Requests.Person;
using HouseholdExpenses.Communication.Responses.Person;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdExpenses.Api.Controllers;

/// <summary>
/// Endpoints para cadastro, atualização, exclusão e consulta de pessoas e totais por pessoa.
/// </summary>
[ApiController]
[Route("api/persons")]
public sealed class PersonController : ControllerBase
{
    private readonly ICreatePersonUseCase _createPersonUseCase;
    private readonly IUpdatePersonUseCase _updatePersonUseCase;
    private readonly IDeletePersonUseCase _deletePersonUseCase;
    private readonly IGetAllPersonsUseCase _getAllPersonsUseCase;
    private readonly IGetPersonTotalsUseCase _getPersonTotalsUseCase;

    /// <summary>Inicializa o controller com os casos de uso de pessoa.</summary>
    public PersonController(
        ICreatePersonUseCase createPersonUseCase,
        IUpdatePersonUseCase updatePersonUseCase,
        IDeletePersonUseCase deletePersonUseCase,
        IGetAllPersonsUseCase getAllPersonsUseCase,
        IGetPersonTotalsUseCase getPersonTotalsUseCase)
    {
        _createPersonUseCase = createPersonUseCase;
        _updatePersonUseCase = updatePersonUseCase;
        _deletePersonUseCase = deletePersonUseCase;
        _getAllPersonsUseCase = getAllPersonsUseCase;
        _getPersonTotalsUseCase = getPersonTotalsUseCase;
    }

    /// <summary>Cria uma nova pessoa.</summary>
    /// <param name="request">Nome e idade.</param>
    /// <returns>Pessoa criada com identificador gerado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonResponse>> CreatePerson([FromBody] CreatePersonRequest request)
    {
        var response = await _createPersonUseCase.ExecuteAsync(request);
        return Created($"/api/persons/{response.Id}", response);
    }

    /// <summary>Atualiza nome e idade de uma pessoa existente.</summary>
    /// <param name="id">Identificador da pessoa.</param>
    /// <param name="request">Novos dados.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonResponse>> UpdatePerson(Guid id, [FromBody] UpdatePersonRequest request)
    {
        var response = await _updatePersonUseCase.ExecuteAsync(id, request);
        return Ok(response);
    }

    /// <summary>Remove uma pessoa e, em cascata, suas transações (conforme configuração do EF).</summary>
    /// <param name="id">Identificador da pessoa.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        await _deletePersonUseCase.ExecuteAsync(id);
        return NoContent();
    }

    /// <summary>Lista todas as pessoas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PersonResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PersonResponse>>> GetAllPersons()
    {
        var list = await _getAllPersonsUseCase.ExecuteAsync();
        return Ok(list);
    }

    /// <summary>Retorna totais de receita, despesa e saldo por pessoa e o resumo geral.</summary>
    [HttpGet("totals")]
    [ProducesResponseType(typeof(PersonTotalsResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<PersonTotalsResult>> GetPersonTotals()
    {
        var result = await _getPersonTotalsUseCase.ExecuteAsync();
        return Ok(result);
    }
}
