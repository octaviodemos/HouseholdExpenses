using HouseholdExpenses.Domain.Entities;

namespace HouseholdExpenses.Tests.Domain;

/// <summary>
/// Testes unitários da entidade <see cref="Person"/>: idade mínima para maioridade e atualização de nome.
/// </summary>
public sealed class PersonTests
{
    [Fact]
    public void IsMinor_WhenAgeIsUnder18_ReturnsTrue()
    {
        // Arrange
        var person = new Person("Menor", 17);

        // Act
        var result = person.IsMinor();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsMinor_WhenAgeIs18_ReturnsFalse()
    {
        // Arrange
        var person = new Person("Maior", 18);

        // Act
        var result = person.IsMinor();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMinor_WhenAgeIsOver18_ReturnsFalse()
    {
        // Arrange
        var person = new Person("Adulto", 30);

        // Act
        var result = person.IsMinor();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Update_WhenNameIsValid_UpdatesName()
    {
        // Arrange
        var person = new Person("Nome Antigo", 20);
        const string novoNome = "Nome Novo Válido";

        // Act
        person.Update(novoNome, 21);

        // Assert
        Assert.Equal(novoNome, person.Name);
        Assert.Equal(21, person.Age);
    }
}
