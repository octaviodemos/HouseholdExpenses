using HouseholdExpenses.Communication.Responses;
using HouseholdExpenses.Communication.Responses.Person;

namespace HouseholdExpenses.Application.UseCases.Person.GetTotals;

/// <summary>
/// Resultado da consulta de totais por pessoa, incluindo o resumo geral.
/// </summary>
/// <param name="Persons">Totais individuais por pessoa.</param>
/// <param name="Summary">Soma das receitas, despesas e saldo considerando todas as pessoas.</param>
public record PersonTotalsResult(
    List<PersonTotalsResponse> Persons,
    SummaryTotalsResponse Summary);
