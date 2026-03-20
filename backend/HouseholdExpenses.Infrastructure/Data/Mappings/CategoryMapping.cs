using HouseholdExpenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseholdExpenses.Infrastructure.Data.Mappings;

/// <summary>
/// Mapeamento da entidade <see cref="Category"/> para a tabela <c>categories</c>.
/// </summary>
public sealed class CategoryMapping : IEntityTypeConfiguration<Category>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(c => c.Purpose)
            .IsRequired()
            .HasConversion<int>();

        builder.HasMany(c => c.Transactions)
            .WithOne(t => t.Category)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
