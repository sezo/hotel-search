using AutoFixture;
using FluentAssertions;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Exceptions;
using Moq;
using NUnit.Framework;

namespace HotelSearch.UnitTests.ApplicationTests.Services;

class Delete: HotelServiceBaseTests
{
    [Test]
    public void Delete_CommandParameterIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        HotelDeleteCommand command = null;
        var service = GetService();
        var action = () => service.Delete(command);

        // Act and assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Delete_CommandIdParameterIsInvalidGuid_ThrowsValidationFailedException()
    {
        // Arrange
        var command = Fixture.Create<HotelDeleteCommand>();
        command.SetPrivateProperty(nameof(command.Id), null);
        
        var service = GetService();
        var action = () => service.Delete(command);

        // Act and assert
        action.Should().ThrowExactly<ValidationFailedException>();
    }
    
    [Test]
    public void Delete_RepositoryDeleteFails_ThrowsHotelDeletionFailsException()
    {
        // Arrange
        var command = new HotelDeleteCommand(HotelId);
        MockedHotelRepository
            .Setup(x => x.Delete(command.Id))
            .Throws(new HotelDeletionFailException(command.Id));
        
        var service = GetService();
        var action = () => service.Delete(command);
        
        // Act and assert
        action.Should().ThrowExactly<HotelDeletionFailException>();
        
        MockedHotelRepository.Verify(x => x.Delete(It.Is<Guid>(id => id == HotelId)), Times.Once);
        MockedHotelRepository.VerifyNoOtherCalls();
    }
    
    [Test]
    public void Delete_OrdinaryCase_ReturnsOperationResultSuccess()
    {
        // Arrange
        var command = new HotelDeleteCommand(HotelId);
        var service = GetService();

        // Act
        var result = service.Delete(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(HotelId);
        MockedHotelRepository.Verify(x => x.Delete(It.Is<Guid>(id => id == HotelId)), Times.Once);
        MockedHotelRepository.VerifyNoOtherCalls();
    }
}