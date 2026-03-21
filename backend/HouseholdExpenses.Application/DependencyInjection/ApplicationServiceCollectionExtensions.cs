using HouseholdExpenses.Application.UseCases.Category.Create;
using HouseholdExpenses.Application.UseCases.Category.GetAll;
using HouseholdExpenses.Application.UseCases.Category.GetTotals;
using HouseholdExpenses.Application.UseCases.Person.Create;
using HouseholdExpenses.Application.UseCases.Person.Delete;
using HouseholdExpenses.Application.UseCases.Person.GetAll;
using HouseholdExpenses.Application.UseCases.Person.GetTotals;
using HouseholdExpenses.Application.UseCases.Person.Update;
using HouseholdExpenses.Application.UseCases.Transaction.Create;
using HouseholdExpenses.Application.UseCases.Transaction.GetAll;
using Microsoft.Extensions.DependencyInjection;

namespace HouseholdExpenses.Application.DependencyInjection;

/// <summary>
/// Extensões para registrar serviços da camada de aplicação no contêiner de DI.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Registra os casos de uso como <see cref="ServiceLifetime.Scoped"/>. Repositórios e unidade de trabalho não são registrados aqui.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    /// <returns>A mesma coleção para encadeamento.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreatePersonUseCase, CreatePersonUseCase>();
        services.AddScoped<IUpdatePersonUseCase, UpdatePersonUseCase>();
        services.AddScoped<IDeletePersonUseCase, DeletePersonUseCase>();
        services.AddScoped<IGetAllPersonsUseCase, GetAllPersonsUseCase>();
        services.AddScoped<IGetPersonTotalsUseCase, GetPersonTotalsUseCase>();

        services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
        services.AddScoped<IGetAllCategoriesUseCase, GetAllCategoriesUseCase>();
        services.AddScoped<IGetCategoryTotalsUseCase, GetCategoryTotalsUseCase>();

        services.AddScoped<ICreateTransactionUseCase, CreateTransactionUseCase>();
        services.AddScoped<IGetAllTransactionsUseCase, GetAllTransactionsUseCase>();

        return services;
    }
}
