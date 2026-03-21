using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Application.UseCases.Category.Create;
using HouseholdExpenses.Communication.Requests.Category;
using HouseholdExpenses.Domain.Enums;
using HouseholdExpenses.Exception;
using Moq;
using DomainCategory = HouseholdExpenses.Domain.Entities.Category;

namespace HouseholdExpenses.Tests.UseCases.Category;

/// <summary>
/// Testes do caso de uso de cadastro de categoria.
/// </summary>
public sealed class CreateCategoryUseCaseTests
{
    [Fact]
    public async Task Execute_WhenValid_ReturnsCategoryResponse()
    {
        // Arrange
        var categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        categoryRepo
            .Setup(r => r.AddAsync(It.IsAny<DomainCategory>()))
            .Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var sut = new CreateCategoryUseCase(categoryRepo.Object, unitOfWork.Object);
        var request = new CreateCategoryRequest("Alimentação", CategoryPurpose.Both);

        // Act
        var response = await sut.ExecuteAsync(request);

        // Assert
        Assert.Equal("Alimentação", response.Description);
        Assert.Equal(CategoryPurpose.Both, response.Purpose);
        Assert.NotEqual(Guid.Empty, response.Id);
        categoryRepo.Verify(
            r => r.AddAsync(It.Is<DomainCategory>(c =>
                c.Description == "Alimentação" && c.Purpose == CategoryPurpose.Both)),
            Times.Once);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenDescriptionIsEmpty_ThrowsErrorOnValidationException()
    {
        // Arrange
        var categoryRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var sut = new CreateCategoryUseCase(categoryRepo.Object, unitOfWork.Object);
        var request = new CreateCategoryRequest("  ", CategoryPurpose.Expense);

        // Act
        var ex = await Assert.ThrowsAsync<ErrorOnValidationException>(() => sut.ExecuteAsync(request));

        // Assert
        Assert.Contains("A descrição é obrigatória.", ex.Errors);
        categoryRepo.Verify(
            r => r.AddAsync(It.IsAny<DomainCategory>()),
            Times.Never);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}
