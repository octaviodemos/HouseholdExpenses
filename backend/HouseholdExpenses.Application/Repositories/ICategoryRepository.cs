using HouseholdExpenses.Domain.Entities;

namespace HouseholdExpenses.Application.Repositories;

/// <summary>
/// Contrato de persistência para <see cref="Category"/>. A implementação fica na camada de infraestrutura.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>Obtém todas as categorias.</summary>
    Task<List<Category>> GetAllAsync();

    /// <summary>Obtém uma categoria pelo identificador.</summary>
    Task<Category?> GetByIdAsync(Guid id);

    /// <summary>Adiciona uma nova categoria ao contexto de persistência.</summary>
    Task AddAsync(Category category);

    /// <summary>Indica se existe categoria com o identificador informado.</summary>
    Task<bool> ExistsAsync(Guid id);
}
