using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Application.Characters.Handlers;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using Moq;

namespace FrenchRevolution.Application.UnitTests;

public class CreateCharacterHandlerTests  
{
    private readonly Mock<ICharacterRepository> _mockCharacterRepository = new();
    private readonly Mock<IRoleRepository> _mockRoleRepository = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();

    private const string Name = "Maximilen Robespierre";
    private const string Profession = "Lawyer";
    private readonly DateTime _dateOfBirth = new(1999, 01, 01);
    private readonly DateTime _dateOfDeath = new(2000, 01, 01);
    
    [Fact]
    public async Task Handle_ReturnsSuccess_WhenCreated()
    {
        // Arrange
        var dto = new CharacterRequestDto(
            Name,
            Profession,
            _dateOfBirth,
            _dateOfDeath,
            []
        );
 
        var command = new CreateCharacterCommand(dto);
        var handler = SetupCreateCharacterHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        
        _mockCharacterRepository.Verify(
            m => m.Add(It.IsAny<Character>()),
            Times.Once);

        _mockUnitOfWork.Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    private CreateCharacterHandler SetupCreateCharacterHandler()
    {
        return new CreateCharacterHandler(
            _mockCharacterRepository.Object, 
            _mockRoleRepository.Object,
            _mockUnitOfWork.Object
        );
    }
}