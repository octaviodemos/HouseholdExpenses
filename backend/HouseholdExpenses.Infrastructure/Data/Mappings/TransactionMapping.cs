using HouseholdExpenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseholdExpenses.Infrastructure.Data.Mappings;

/// <summary>
/// Mapeamento da entidade <see cref="Transaction"/> para a tabela <c>transactions</c>.
/// </summary>
public sealed class TransactionMapping : IEntityTypeConfiguration<Transaction>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(t => t.CategoryId)
            .IsRequired();

        builder.Property(t => t.PersonId)
            .IsRequired();
    }
}
