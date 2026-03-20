namespace HouseholdExpenses.Application.Repositories;

/// <summary>
/// Unidade de trabalho para confirmar transações de persistência após operações nos repositórios.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>Persiste as alterações pendentes no armazenamento.</summary>
    Task CommitAsync();
}
