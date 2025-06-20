using FluentAssertions;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Exceptions;
using Moq;
using NUnit.Framework;

namespace HotelSearch.UnitTests.ApplicationTests.Services;

class Upsert: HotelServiceBaseTests
{
    [Test]
    public void Upsert_CommandParameterIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        HotelUpsertCommand upsertNULLCommand = null;
        var service = GetService();
        var action = () => service.Upsert(upsertNULLCommand);

        // Act and assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }
    
    // Detail unit tests for validation are in Hotel entity unit tests
    [TestCase(10000, 45.12, "Hotel name", 100, null, Description = "Incorrect longitude")]
    [TestCase(16.11, 100000, "Hotel name", 100, null, Description = "Incorrect latitude")]
    [TestCase(16.11, 45.12, "**!!", 100, null, Description = "Incorrect hotel name")]
    [TestCase(16.11, 45.12, "Hotel name", -10, null, Description = "Incorrect price")]
    [TestCase(16.11, 45.12, "Hotel name", 100, 10000, Description = "Incorrect discount")]
    public void Upsert_OneOfCommandPropertiesIsNotValid_ThrowsValidationFailsException(
        double longitude,
        double latitude,
        string name,
        decimal price,
        int? discount
    )
    {
        // Arrange
        var upsertCommand = GetValidHotelUpsertCommand(HotelId, name, longitude, latitude, price, discount);
        MockedHotelRepository
            .Setup(x => x.Get(HotelId))
            .Returns(Hotel);
        
        var service = GetService();
        var action = () => service.Upsert(upsertCommand);

        // Act and assert
        action.Should().ThrowExactly<ValidationFailedException>();
    }
    
    [Test]
    public void Upsert_RepositoryUpsertException_ThrowsHotelUpsertFailedException()
    {
        // Arrange
        var upsertCommand = GetValidHotelUpsertCommand(HotelId);
        MockedHotelRepository
            .Setup(x => x.Get(HotelId))
            .Returns(Hotel);
        
        MockedHotelRepository
            .Setup(x => x.Upsert(Hotel))
            .Throws<Exception>();
        
        var service = GetService();
        var action = () => service.Upsert(upsertCommand);

        // Act and assert
        action.Should().ThrowExactly<HotelUpsertFailedException>();
    }
    
    [Test]
    public void Upsert_HotelIdExistsInCommandButNotFoundInRepository_ThrowsHotelNotFoundException()
    {
        // Arrange
        var upsertCommand = GetValidHotelUpsertCommand(HotelId);
        MockedHotelRepository
            .Setup(x => x.Get(HotelId))
            .Returns(() => null);
        
        var service = GetService();
        var action = () => service.Upsert(upsertCommand);

        // Act and assert
        action.Should().ThrowExactly<HotelNotFoundException>();
    }
    
    [Test]
    public void Upsert_UpdateCase_ShouldReturnSuccess()
    {
        // Arrange
        Hotel.SetPrivateProperty(nameof(Hotel.Id), HotelId);
        var upsertCommand = GetValidHotelUpsertCommand(HotelId);
        
        MockedHotelRepository
            .Setup(x => x.Get(HotelId))
            .Returns(Hotel);
        
        var service = GetService();

        // Act
        var result = service.Upsert(upsertCommand);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(HotelId);
        MockedHotelRepository.Verify(x => x.Get(It.Is<Guid>(id => id == HotelId)), Times.Once);
        
        MockedHotelRepository.Verify(x => x.Upsert(It.Is<Hotel>(h =>
            h.Id == HotelId && 
            h.Name == upsertCommand.Name && 
            h.Location.X == upsertCommand.Longitude &&
            h.Location.Y == upsertCommand.Latitude && 
            h.Price.RegularPrice == upsertCommand.Price && 
            h.Price.Discount == upsertCommand.Discount)), Times.Once);
        
        MockedHotelRepository.VerifyNoOtherCalls();
    }
    
    [Test(Description = "Repository should not be called to get hotel entity when id (in command object) is not present")]
    public void Upsert_InsertCase_ShouldReturnSuccess()
    {
        // Arrange
        Hotel.SetPrivateProperty(nameof(Hotel.Id), HotelId);
        var upsertCommand = GetValidHotelUpsertCommand();
        
        var service = GetService();

        // Act
        var result = service.Upsert(upsertCommand);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        MockedHotelRepository.Verify(x => x.Upsert(It.Is<Hotel>(h =>
            h.Id == result.Value && 
            h.Name == upsertCommand.Name && 
            h.Location.X == upsertCommand.Longitude &&
            h.Location.Y == upsertCommand.Latitude && 
            h.Price.RegularPrice == upsertCommand.Price && 
            h.Price.Discount == upsertCommand.Discount)), Times.Once);
        
        MockedHotelRepository.VerifyNoOtherCalls();
    }
}