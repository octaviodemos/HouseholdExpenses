namespace HouseholdExpenses.Communication.Responses.Person;

/// <summary>
/// Representação de uma pessoa retornada pela API após consulta ou criação. Apenas dados de saída, sem lógica de negócio.
/// </summary>
/// <param name="Id">Identificador único da pessoa.</param>
/// <param name="Name">Nome da pessoa.</param>
/// <param name="Age">Idade em anos.</param>
public record PersonResponse(Guid Id, string Name, int Age);
