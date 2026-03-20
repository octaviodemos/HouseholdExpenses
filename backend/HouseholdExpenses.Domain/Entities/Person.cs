using HouseholdExpenses.Domain.Enums;

namespace HouseholdExpenses.Domain.Entities;

/// <summary>
/// Representa uma pessoa que pode possuir transações financeiras.
/// </summary>
/// <remarks>
/// <para>
/// Regra de negócio: pessoa menor de idade (<see cref="IsMinor"/>) só pode possuir transações do tipo <see cref="TransactionType.Expense"/>.
/// A aplicação deve garantir essa restrição ao criar ou alterar transações.
/// </para>
/// <para>
/// Ao excluir uma pessoa, todas as suas transações devem ser removidas em cascata.
/// Essa política será configurada no Entity Framework Core (ex.: <c>OnDelete(DeleteBehavior.Cascade)</c> na relação com <see cref="Transaction"/>).
/// </para>
/// </remarks>
public class Person
{
    /// <summary>
    /// Construtor para materialização pelo Entity Framework Core.
    /// </summary>
    protected Person()
    {
        Name = string.Empty;
        Transactions = new List<Transaction>();
    }

    /// <summary>
    /// Cria uma nova pessoa com <see cref="Id"/> gerado automaticamente.
    /// </summary>
    /// <param name="name">Nome (máximo 200 caracteres).</param>
    /// <param name="age">Idade em anos.</param>
    public Person(string name, int age)
        : this()
    {
        Id = Guid.NewGuid();
        Update(name, age);
    }

    public Guid Id { get; private set; }

    /// <summary>
    /// Nome da pessoa (máximo 200 caracteres).
    /// </summary>
    public string Name { get; private set; }

    public int Age { get; private set; }

    /// <summary>
    /// Transações associadas a esta pessoa.
    /// </summary>
    public ICollection<Transaction> Transactions { get; private set; }

    /// <summary>
    /// Atualiza nome e idade.
    /// </summary>
    /// <param name="name">Nome (máximo 200 caracteres).</param>
    /// <param name="age">Idade em anos.</param>
    public void Update(string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome é obrigatório.", nameof(name));
        if (name.Length > 200)
            throw new ArgumentException("O nome não pode exceder 200 caracteres.", nameof(name));

        Name = name;
        Age = age;
    }

    /// <summary>
    /// Indica se a pessoa é menor de 18 anos.
    /// </summary>
    /// <returns><see langword="true"/> se <see cref="Age"/> for menor que 18; caso contrário, <see langword="false"/>.</returns>
    public bool IsMinor() => Age < 18;
}
