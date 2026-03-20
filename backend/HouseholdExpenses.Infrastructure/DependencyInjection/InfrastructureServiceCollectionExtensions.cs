using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Infrastructure.Data;
using HouseholdExpenses.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HouseholdExpenses.Infrastructure.DependencyInjection;

/// <summary>
/// Extensões para registrar persistência e repositórios da infraestrutura.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registra o <see cref="HouseholdExpensesDbContext"/> (PostgreSQL/Npgsql), repositórios e <see cref="IUnitOfWork"/> como scoped.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    /// <param name="configuration">Configuração da aplicação (connection string <c>DefaultConnection</c>).</param>
    /// <returns>A mesma coleção para encadeamento.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<HouseholdExpensesDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork, HouseholdExpenses.Infrastructure.UnitOfWork.UnitOfWork>();

        return services;
    }
}
