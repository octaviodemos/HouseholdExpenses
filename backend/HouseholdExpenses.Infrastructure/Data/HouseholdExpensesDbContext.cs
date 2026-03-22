using HouseholdExpenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Infrastructure.Data;

/// <summary>
/// Contexto do Entity Framework Core para o banco HouseholdExpenses (PostgreSQL).
/// </summary>
public class HouseholdExpensesDbContext : DbContext
{
    /// <summary>
    /// Inicializa o contexto com as opções configuradas (provedor, connection string, etc.).
    /// </summary>
    /// <param name="options">Opções do <see cref="DbContext"/>.</param>
    public HouseholdExpensesDbContext(DbContextOptions<HouseholdExpensesDbContext> options)
        : base(options)
    {
    }

    /// <summary>Pessoas cadastradas.</summary>
    public DbSet<Person> Persons => Set<Person>();

    /// <summary>Categorias de transações.</summary>
    public DbSet<Category> Categories => Set<Category>();

    /// <summary>Transações financeiras.</summary>
    public DbSet<Transaction> Transactions => Set<Transaction>();

    /// <summary>Usuários para autenticação JWT.</summary>
    public DbSet<User> Users => Set<User>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HouseholdExpensesDbContext).Assembly);
    }
}
