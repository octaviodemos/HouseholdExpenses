using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Communication.Responses;
using HouseholdExpenses.Communication.Responses.Person;
using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Application.UseCases.Person.GetTotals;

/// <summary>
/// Agrega transações por pessoa e produz totais individuais e <see cref="SummaryTotalsResponse"/>.
/// </summary>
public sealed class GetPersonTotalsUseCase : IGetPersonTotalsUseCase
{
    private readonly IPersonRepository _personRepository;
    private readonly ITransactionRepository _transactionRepository;

    /// <summary>Inicializa o caso de uso com repositórios de pessoas e transações.</summary>
    public GetPersonTotalsUseCase(
        IPersonRepository personRepository,
        ITransactionRepository transactionRepository)
    {
        _personRepository = personRepository;
        _transactionRepository = transactionRepository;
    }

    /// <inheritdoc />
    public async Task<PersonTotalsResult> ExecuteAsync()
    {
        var persons = await _personRepository.GetAllAsync();
        var allTransactions = await _transactionRepository.GetAllAsync();

        var byPerson = allTransactions
            .GroupBy(t => t.PersonId)
            .ToDictionary(g => g.Key, g => g.ToList());

        decimal grandIncome = 0;
        decimal grandExpense = 0;

        var list = new List<PersonTotalsResponse>(persons.Count);

        foreach (var person in persons)
        {
            byPerson.TryGetValue(person.Id, out var txs);
            txs ??= [];

            var income = txs.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var expense = txs.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            var balance = income - expense;

            grandIncome += income;
            grandExpense += expense;

            list.Add(new PersonTotalsResponse(
                person.Id,
                person.Name,
                income,
                expense,
                balance));
        }

        var summary = new SummaryTotalsResponse(
            grandIncome,
            grandExpense,
            grandIncome - grandExpense);

        return new PersonTotalsResult(list, summary);
    }
}
