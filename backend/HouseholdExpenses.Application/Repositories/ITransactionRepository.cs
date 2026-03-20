using HouseholdExpenses.Domain.Entities;

namespace HouseholdExpenses.Application.Repositories;

/// <summary>
/// Contrato de persistência para <see cref="Transaction"/>. A implementação fica na camada de infraestrutura.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Lista todas as transações. A implementação deve carregar categoria e pessoa quando necessário para projeções.
    /// </summary>
    Task<List<Transaction>> GetAllAsync();

    /// <summary>Obtém transações vinculadas à pessoa informada.</summary>
    Task<List<Transaction>> GetByPersonIdAsync(Guid personId);

    /// <summary>Adiciona uma nova transação ao contexto de persistência.</summary>
    Task AddAsync(Transaction transaction);
}
