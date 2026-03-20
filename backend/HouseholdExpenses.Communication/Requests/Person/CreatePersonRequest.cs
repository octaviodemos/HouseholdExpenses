namespace HouseholdExpenses.Communication.Requests.Person;

/// <summary>
/// Dados enviados pela API para cadastrar uma nova pessoa. Contém apenas transporte de dados, sem regras de negócio.
/// </summary>
/// <param name="Name">Nome da pessoa.</param>
/// <param name="Age">Idade em anos.</param>
public record CreatePersonRequest(string Name, int Age);
