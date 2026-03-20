using HouseholdExpenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseholdExpenses.Infrastructure.Data.Mappings;

/// <summary>
/// Mapeamento da entidade <see cref="Person"/> para a tabela <c>persons</c>, incluindo exclusão em cascata das transações.
/// </summary>
public sealed class PersonMapping : IEntityTypeConfiguration<Person>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("persons");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Age)
            .IsRequired();

        builder.HasMany(p => p.Transactions)
            .WithOne(t => t.Person)
            .HasForeignKey(t => t.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
