namespace HouseholdExpenses.Application.UseCases.Person.Delete;

/// <summary>
/// Remove uma pessoa do sistema. As transações associadas devem ser tratadas em cascata na persistência.
/// </summary>
public interface IDeletePersonUseCase
{
    /// <summary>Remove a pessoa identificada por <paramref name="id"/>.</summary>
    Task ExecuteAsync(Guid id);
}
