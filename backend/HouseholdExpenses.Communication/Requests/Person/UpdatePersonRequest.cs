namespace HouseholdExpenses.Communication.Requests.Person;

/// <summary>
/// Dados enviados pela API para atualizar uma pessoa existente. Contém apenas transporte de dados, sem regras de negócio.
/// </summary>
/// <param name="Name">Nome da pessoa.</param>
/// <param name="Age">Idade em anos.</param>
public record UpdatePersonRequest(string Name, int Age);
