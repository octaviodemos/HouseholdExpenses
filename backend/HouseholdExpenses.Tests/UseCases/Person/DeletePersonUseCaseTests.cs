using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Application.UseCases.Person.Delete;
using HouseholdExpenses.Exception;
using Moq;
using DomainPerson = HouseholdExpenses.Domain.Entities.Person;

namespace HouseholdExpenses.Tests.UseCases.Person;

/// <summary>
/// Testes do caso de uso de exclusão de pessoa: existência do recurso e chamadas ao repositório.
/// </summary>
public sealed class DeletePersonUseCaseTests
{
    [Fact]
    public async Task Execute_WhenPersonExists_DeletesPerson()
    {
        // Arrange
        var person = new DomainPerson("Para excluir", 40);
        var id = person.Id;

        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        personRepo.Setup(r => r.GetByIdWithTransactionsAsync(id)).ReturnsAsync(person);
        personRepo.Setup(r => r.DeleteAsync(person)).Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var sut = new DeletePersonUseCase(personRepo.Object, unitOfWork.Object);

        // Act
        await sut.ExecuteAsync(id);

        // Assert
        personRepo.Verify(r => r.GetByIdWithTransactionsAsync(id), Times.Once);
        personRepo.Verify(r => r.DeleteAsync(person), Times.Once);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenPersonNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        personRepo.Setup(r => r.GetByIdWithTransactionsAsync(id)).ReturnsAsync((DomainPerson?)null);

        var sut = new DeletePersonUseCase(personRepo.Object, unitOfWork.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.ExecuteAsync(id));

        personRepo.Verify(r => r.DeleteAsync(It.IsAny<DomainPerson>()), Times.Never);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}
