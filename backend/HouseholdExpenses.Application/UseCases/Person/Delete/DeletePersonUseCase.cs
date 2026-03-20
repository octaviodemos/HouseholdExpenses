using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Exception;

namespace HouseholdExpenses.Application.UseCases.Person.Delete;

/// <summary>
/// Exclui a pessoa após carregá-la com transações; a remoção em cascata das transações é configurada no EF Core.
/// </summary>
public sealed class DeletePersonUseCase : IDeletePersonUseCase
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>Inicializa o caso de uso com repositório e unidade de trabalho.</summary>
    public DeletePersonUseCase(IPersonRepository personRepository, IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(Guid id)
    {
        var person = await _personRepository.GetByIdWithTransactionsAsync(id)
            ?? throw new NotFoundException("Pessoa não encontrada.");

        await _personRepository.DeleteAsync(person);
        await _unitOfWork.CommitAsync();
    }
}
