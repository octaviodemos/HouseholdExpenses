using System.Collections.Generic;

namespace HouseholdExpenses.Exception;

/// <summary>
/// Indica falha de validação: dados inválidos, regras de negócio violadas ou inconsistências detectadas antes de persistir ou expor o recurso.
/// </summary>
/// <remarks>
/// Deve ser lançada quando entradas ou estado não satisfazem as regras acordadas (ex.: campo obrigatório, formato inválido,
/// combinação categoria/tipo de transação incompatível, menor de idade com transação de receita, etc.).
/// </remarks>
public sealed class ErrorOnValidationException : HouseholdExpensesException
{
    /// <summary>
    /// Cria uma exceção de validação com a lista de mensagens retornadas ao cliente ou registradas.
    /// </summary>
    /// <param name="errors">Mensagens descritivas de cada falha; repassadas à classe base.</param>
    public ErrorOnValidationException(IList<string> errors)
        : base(errors)
    {
    }
}
