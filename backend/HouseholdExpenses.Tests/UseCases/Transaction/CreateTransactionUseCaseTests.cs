using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Application.UseCases.Transaction.Create;
using HouseholdExpenses.Communication.Requests.Transaction;
using HouseholdExpenses.Domain.Enums;
using HouseholdExpenses.Exception;
using Moq;
using DomainCategory = HouseholdExpenses.Domain.Entities.Category;
using DomainPerson = HouseholdExpenses.Domain.Entities.Person;
using DomainTransaction = HouseholdExpenses.Domain.Entities.Transaction;

namespace HouseholdExpenses.Tests.UseCases.Transaction;

/// <summary>
/// Testes do caso de uso de criação de transação: menores, compatibilidade categoria/tipo e recursos existentes.
/// </summary>
public sealed class CreateTransactionUseCaseTests
{
    [Fact]
    public async Task Execute_WhenMinorTriesToCreateIncome_ThrowsErrorOnValidationException()
    {
        // Arrange
        var minor = new DomainPerson("Menor", 17);
        var category = new DomainCategory("Geral", CategoryPurpose.Both);
        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
        var transactionRepo = new Mock<ITransactionRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);

        personRepo.Setup(r => r.GetByIdAsync(minor.Id)).ReturnsAsync(minor);
        categoryRepo.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

        var sut = new CreateTransactionUseCase(
            personRepo.Object,
            categoryRepo.Object,
            transactionRepo.Object,
            unitOfWork.Object);

        var request = new CreateTransactionRequest(
            "Mesada",
            50m,
            TransactionType.Income,
            category.Id,
            minor.Id);

        // Act
        var ex = await Assert.ThrowsAsync<ErrorOnValidationException>(() => sut.ExecuteAsync(request));

        // Assert
        Assert.Contains("Menores de 18 anos só podem registrar transações do tipo despesa.", ex.Errors);
        transactionRepo.Verify(r => r.AddAsync(It.IsAny<DomainTransaction>()), Times.Never);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Execute_WhenCategoryIsIncompatible_ThrowsErrorOnValidationException()
    {
        // Arrange
        var adult = new DomainPerson("Adulto", 30);
        var expenseOnly = new DomainCategory("Só despesa", CategoryPurpose.Expense);
        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
        var transactionRepo = new Mock<ITransactionRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);

        personRepo.Setup(r => r.GetByIdAsync(adult.Id)).ReturnsAsync(adult);
        categoryRepo.Setup(r => r.GetByIdAsync(expenseOnly.Id)).ReturnsAsync(expenseOnly);

        var sut = new CreateTransactionUseCase(
            personRepo.Object,
            categoryRepo.Object,
            transactionRepo.Object,
            unitOfWork.Object);

        var request = new CreateTransactionRequest(
            "Salário",
            1000m,
            TransactionType.Income,
            expenseOnly.Id,
            adult.Id);

        // Act
        var ex = await Assert.ThrowsAsync<ErrorOnValidationException>(() => sut.ExecuteAsync(request));

        // Assert
        Assert.Contains("A categoria não é compatível com o tipo da transação.", ex.Errors);
        transactionRepo.Verify(r => r.AddAsync(It.IsAny<DomainTransaction>()), Times.Never);
    }

    [Fact]
    public async Task Execute_WhenValid_ReturnsTransactionResponse()
    {
        // Arrange
        var adult = new DomainPerson("Adulto", 25);
        var category = new DomainCategory("Moradia", CategoryPurpose.Expense);
        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
        var transactionRepo = new Mock<ITransactionRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);

        personRepo.Setup(r => r.GetByIdAsync(adult.Id)).ReturnsAsync(adult);
        categoryRepo.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);
        transactionRepo
            .Setup(r => r.AddAsync(It.IsAny<DomainTransaction>()))
            .Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var sut = new CreateTransactionUseCase(
            personRepo.Object,
            categoryRepo.Object,
            transactionRepo.Object,
            unitOfWork.Object);

        var request = new CreateTransactionRequest(
            "Aluguel",
            800m,
            TransactionType.Expense,
            category.Id,
            adult.Id);

        // Act
        var response = await sut.ExecuteAsync(request);

        // Assert
        Assert.Equal("Aluguel", response.Description);
        Assert.Equal(800m, response.Amount);
        Assert.Equal(TransactionType.Expense, response.Type);
        Assert.Equal(category.Id, response.CategoryId);
        Assert.Equal("Moradia", response.CategoryDescription);
        Assert.Equal(adult.Id, response.PersonId);
        Assert.Equal("Adulto", response.PersonName);
        transactionRepo.Verify(r => r.AddAsync(It.IsAny<DomainTransaction>()), Times.Once);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenPersonNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
        var transactionRepo = new Mock<ITransactionRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);

        personRepo.Setup(r => r.GetByIdAsync(personId)).ReturnsAsync((DomainPerson?)null);

        var sut = new CreateTransactionUseCase(
            personRepo.Object,
            categoryRepo.Object,
            transactionRepo.Object,
            unitOfWork.Object);

        var request = new CreateTransactionRequest(
            "Teste",
            10m,
            TransactionType.Expense,
            categoryId,
            personId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.ExecuteAsync(request));

        categoryRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        transactionRepo.Verify(r => r.AddAsync(It.IsAny<DomainTransaction>()), Times.Never);
    }

    [Fact]
    public async Task Execute_WhenCategoryNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var adult = new DomainPerson("Titular", 40);
        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
        var transactionRepo = new Mock<ITransactionRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);

        var missingCategoryId = Guid.NewGuid();
        personRepo.Setup(r => r.GetByIdAsync(adult.Id)).ReturnsAsync(adult);
        categoryRepo.Setup(r => r.GetByIdAsync(missingCategoryId)).ReturnsAsync((DomainCategory?)null);

        var sut = new CreateTransactionUseCase(
            personRepo.Object,
            categoryRepo.Object,
            transactionRepo.Object,
            unitOfWork.Object);

        var request = new CreateTransactionRequest(
            "Item",
            15m,
            TransactionType.Expense,
            missingCategoryId,
            adult.Id);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.ExecuteAsync(request));

        transactionRepo.Verify(r => r.AddAsync(It.IsAny<DomainTransaction>()), Times.Never);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}
