using HouseholdExpenses.Application.Repositories;
using HouseholdExpenses.Application.UseCases.Person.Create;
using HouseholdExpenses.Communication.Requests.Person;
using HouseholdExpenses.Exception;
using Moq;
using DomainPerson = HouseholdExpenses.Domain.Entities.Person;

namespace HouseholdExpenses.Tests.UseCases.Person;

/// <summary>
/// Testes do caso de uso de cadastro de pessoa: validações de entrada e persistência simulada.
/// </summary>
public sealed class CreatePersonUseCaseTests
{
    [Fact]
    public async Task Execute_WhenValid_ReturnPersonResponse()
    {
        // Arrange
        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        personRepo
            .Setup(r => r.AddAsync(It.IsAny<DomainPerson>()))
            .Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var sut = new CreatePersonUseCase(personRepo.Object, unitOfWork.Object);
        var request = new CreatePersonRequest("Maria Silva", 28);

        // Act
        var response = await sut.ExecuteAsync(request);

        // Assert
        Assert.Equal("Maria Silva", response.Name);
        Assert.Equal(28, response.Age);
        Assert.NotEqual(Guid.Empty, response.Id);
        personRepo.Verify(r => r.AddAsync(It.Is<DomainPerson>(p => p.Name == "Maria Silva" && p.Age == 28)), Times.Once);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenNameIsEmpty_ThrowsErrorOnValidationException()
    {
        // Arrange
        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var sut = new CreatePersonUseCase(personRepo.Object, unitOfWork.Object);
        var request = new CreatePersonRequest("   ", 20);

        // Act
        var ex = await Assert.ThrowsAsync<ErrorOnValidationException>(() => sut.ExecuteAsync(request));

        // Assert
        Assert.Contains("O nome é obrigatório.", ex.Errors);
        personRepo.Verify(r => r.AddAsync(It.IsAny<DomainPerson>()), Times.Never);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Execute_WhenNameExceedsMaxLength_ThrowsErrorOnValidationException()
    {
        // Arrange
        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var sut = new CreatePersonUseCase(personRepo.Object, unitOfWork.Object);
        var longName = new string('a', 201);
        var request = new CreatePersonRequest(longName, 20);

        // Act
        var ex = await Assert.ThrowsAsync<ErrorOnValidationException>(() => sut.ExecuteAsync(request));

        // Assert
        Assert.Contains("O nome não pode exceder 200 caracteres.", ex.Errors);
        personRepo.Verify(r => r.AddAsync(It.IsAny<DomainPerson>()), Times.Never);
    }

    [Fact]
    public async Task Execute_WhenAgeIsZero_ThrowsErrorOnValidationException()
    {
        // Arrange
        var personRepo = new Mock<IPersonRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var sut = new CreatePersonUseCase(personRepo.Object, unitOfWork.Object);
        var request = new CreatePersonRequest("João", 0);

        // Act
        var ex = await Assert.ThrowsAsync<ErrorOnValidationException>(() => sut.ExecuteAsync(request));

        // Assert
        Assert.Contains("A idade deve ser maior que zero.", ex.Errors);
        personRepo.Verify(r => r.AddAsync(It.IsAny<DomainPerson>()), Times.Never);
    }
}
