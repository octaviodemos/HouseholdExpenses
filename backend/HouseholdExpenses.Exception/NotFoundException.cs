using System.Collections.Generic;

namespace HouseholdExpenses.Exception;

/// <summary>
/// Indica que o recurso solicitado não foi encontrado no sistema.
/// </summary>
/// <remarks>
/// Deve ser lançada quando um identificador não corresponde a uma pessoa, categoria, transação ou outro agregado persistido.
/// </remarks>
public sealed class NotFoundException : HouseholdExpensesException
{
    /// <summary>
    /// Cria uma exceção com uma única mensagem, encapsulada em uma lista para a classe base.
    /// </summary>
    /// <param name="message">Descrição do recurso não encontrado (ex.: mensagem localizada para o cliente).</param>
    public NotFoundException(string message)
        : base(new List<string> { RequireMessage(message) })
    {
    }

    private static string RequireMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        return message;
    }
}
