using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Communication.Requests.Transaction;
using HouseholdExpenses.Communication.Responses.Transaction;
using HouseholdExpenses.Domain.Enums;
using HouseholdExpenses.Exception;
using DomainCategory = HouseholdExpenses.Domain.Entities.Category;
using DomainPerson = HouseholdExpenses.Domain.Entities.Person;
using DomainTransaction = HouseholdExpenses.Domain.Entities.Transaction;

namespace HouseholdExpenses.Application.UseCases.Transaction.Create;

/// <summary>
/// Cria transação com validações de domínio (menor de idade, categoria compatível, descrição e valor).
/// </summary>
public sealed class CreateTransactionUseCase : ICreateTransactionUseCase
{
    private readonly IPersonRepository _personRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>Inicializa o caso de uso com repositórios e unidade de trabalho.</summary>
    public CreateTransactionUseCase(
        IPersonRepository personRepository,
        ICategoryRepository categoryRepository,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _categoryRepository = categoryRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<TransactionResponse> ExecuteAsync(CreateTransactionRequest request)
    {
        var person = await _personRepository.GetByIdAsync(request.PersonId)
            ?? throw new NotFoundException("Pessoa não encontrada.");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
            ?? throw new NotFoundException("Categoria não encontrada.");

        ValidateBusinessRules(person, category, request);
        ValidateInput(request);

        var transaction = new DomainTransaction(
            request.Description,
            request.Amount,
            request.Type,
            request.CategoryId,
            request.PersonId);

        await _transactionRepository.AddAsync(transaction);
        await _unitOfWork.CommitAsync();

        return new TransactionResponse(
            transaction.Id,
            transaction.Description,
            transaction.Amount,
            transaction.Type,
            transaction.CategoryId,
            category.Description,
            transaction.PersonId,
            person.Name);
    }

    private static void ValidateBusinessRules(
        DomainPerson person,
        DomainCategory category,
        CreateTransactionRequest request)
    {
        var errors = new List<string>();

        if (person.IsMinor() && request.Type == TransactionType.Income)
            errors.Add("Menores de 18 anos só podem registrar transações do tipo despesa.");

        if (!category.IsCompatibleWith(request.Type))
            errors.Add("A categoria não é compatível com o tipo da transação.");

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }

    private static void ValidateInput(CreateTransactionRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Description))
            errors.Add("A descrição é obrigatória.");
        else if (request.Description.Length > 400)
            errors.Add("A descrição não pode exceder 400 caracteres.");

        if (request.Amount <= 0)
            errors.Add("O valor deve ser maior que zero.");

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }
}
