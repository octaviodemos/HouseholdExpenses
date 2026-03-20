using HouseholdExpenses.Domain.Entities;

namespace HouseholdExpenses.Application.Repositories;

/// <summary>
/// Contrato de persistência para <see cref="Person"/>. A implementação fica na camada de infraestrutura.
/// </summary>
public interface IPersonRepository
{
    /// <summary>Obtém todas as pessoas.</summary>
    Task<List<Person>> GetAllAsync();

    /// <summary>Obtém uma pessoa pelo identificador, sem garantia de carregar transações.</summary>
    Task<Person?> GetByIdAsync(Guid id);

    /// <summary>Obtém uma pessoa com transações carregadas (útil para exclusão e regras que dependem do histórico).</summary>
    Task<Person?> GetByIdWithTransactionsAsync(Guid id);

    /// <summary>Adiciona uma nova pessoa ao contexto de persistência.</summary>
    Task AddAsync(Person person);

    /// <summary>Persiste alterações em uma pessoa existente.</summary>
    Task UpdateAsync(Person person);

    /// <summary>Remove a pessoa do contexto de persistência.</summary>
    Task DeleteAsync(Person person);

    /// <summary>Indica se existe pessoa com o identificador informado.</summary>
    Task<bool> ExistsAsync(Guid id);
}
