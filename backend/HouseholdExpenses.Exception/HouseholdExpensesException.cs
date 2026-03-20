using System.Collections.Generic;

namespace HouseholdExpenses.Exception;

/// <summary>
/// Classe base para todas as exceções do domínio HouseholdExpenses. Agrupa uma ou mais mensagens em <see cref="Errors"/>.
/// </summary>
/// <remarks>
/// Lance derivadas desta classe a partir da aplicação ou infraestrutura quando for necessário sinalizar falhas previsíveis
/// (validação, recurso ausente, etc.), em oposição a falhas inesperadas do runtime.
/// </remarks>
public abstract class HouseholdExpensesException : SystemException
{
    /// <summary>
    /// Mensagens de erro associadas a esta exceção (ex.: várias falhas de validação).
    /// </summary>
    public IList<string> Errors { get; }

    /// <summary>
    /// Inicializa a exceção com a lista de erros. A mensagem exibida pela propriedade <see cref="System.Exception.Message"/> é derivada dessa lista.
    /// </summary>
    /// <param name="errors">Lista de mensagens; não pode ser nula.</param>
    protected HouseholdExpensesException(IList<string> errors)
        : base(FormatMessage(errors))
    {
        ArgumentNullException.ThrowIfNull(errors);
        Errors = new List<string>(errors);
    }

    private static string FormatMessage(IList<string> errors)
    {
        return errors.Count == 0
            ? "Ocorreu um erro."
            : string.Join("; ", errors);
    }
}
