namespace HouseholdExpenses.Domain.Entities;

/// <summary>
/// Usuário da aplicação para autenticação (email e hash de senha).
/// </summary>
public class User
{
    /// <summary>
    /// Construtor para materialização pelo Entity Framework Core.
    /// </summary>
    protected User()
    {
        Email = string.Empty;
        PasswordHash = string.Empty;
    }

    /// <summary>
    /// Cria uma instância com identificador gerado e credenciais persistíveis.
    /// </summary>
    /// <param name="email">E-mail (normalizado fora da entidade, se necessário).</param>
    /// <param name="passwordHash">Hash da senha (ex.: BCrypt).</param>
    public User(string email, string passwordHash)
        : this()
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
    }

    /// <summary>
    /// Fábrica explícita alinhada ao padrão de criação do domínio.
    /// </summary>
    /// <param name="email">E-mail do usuário.</param>
    /// <param name="passwordHash">Hash da senha.</param>
    /// <returns>Novo <see cref="User"/>.</returns>
    public static User Create(string email, string passwordHash) =>
        new(email, passwordHash);

    /// <summary>Identificador único.</summary>
    public Guid Id { get; private set; }

    /// <summary>E-mail de login.</summary>
    public string Email { get; private set; }

    /// <summary>Hash seguro da senha.</summary>
    public string PasswordHash { get; private set; }
}
