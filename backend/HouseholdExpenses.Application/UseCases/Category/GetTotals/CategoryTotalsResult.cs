using HouseholdExpenses.Communication.Responses;
using HouseholdExpenses.Communication.Responses.Category;

namespace HouseholdExpenses.Application.UseCases.Category.GetTotals;

/// <summary>
/// Resultado da consulta de totais por categoria, incluindo o resumo geral.
/// </summary>
/// <param name="Categories">Totais individuais por categoria.</param>
/// <param name="Summary">Soma das receitas, despesas e saldo considerando todas as categorias.</param>
public record CategoryTotalsResult(
    List<CategoryTotalsResponse> Categories,
    SummaryTotalsResponse Summary);
