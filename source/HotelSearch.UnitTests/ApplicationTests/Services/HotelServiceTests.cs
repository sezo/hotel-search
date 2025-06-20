using AutoFixture;
using FluentAssertions;
using HotelSearch.Application.Services;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Exceptions;
using HotelSearch.Domain.Repositories;
using HotelSearch.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace HotelSearch.UnitTests.ApplicationTests.Services;

class HotelServiceTests: BaseServiceTest<IHotelService>
{
    private Mock<ILogger<HotelService>> _mockedLogger;
    private Mock<IHotelRepository> _mockedHotelRepository;
    private Hotel _hotel;
    private Guid _hotelId;

    [SetUp]
    public void Setup()
    {
        InstantiateDependencies();
        _hotel = Fixture.Create<Hotel>();
        _hotelId = Guid.NewGuid();
    }

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
        var upsertCommand = GetValidHotelUpsertCommand(_hotelId, name, longitude, latitude, price, discount);
        _mockedHotelRepository
            .Setup(x => x.Get(_hotelId))
            .Returns(_hotel);
        
        var service = GetService();
        var action = () => service.Upsert(upsertCommand);

        // Act and assert
        action.Should().ThrowExactly<ValidationFailedException>();
    }
    
    [Test]
    public void Upsert_RepositoryUpsertException_ThrowsHotelUpsertFailedException()
    {
        // Arrange
        var upsertCommand = GetValidHotelUpsertCommand(_hotelId);
        _mockedHotelRepository
            .Setup(x => x.Get(_hotelId))
            .Returns(_hotel);
        
        _mockedHotelRepository
            .Setup(x => x.Upsert(_hotel))
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
        var upsertCommand = GetValidHotelUpsertCommand(_hotelId);
        _mockedHotelRepository
            .Setup(x => x.Get(_hotelId))
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
        _hotel.SetPrivateProperty(nameof(_hotel.Id), _hotelId);
        var upsertCommand = GetValidHotelUpsertCommand(_hotelId);
        
        _mockedHotelRepository
            .Setup(x => x.Get(_hotelId))
            .Returns(_hotel);
        
        var service = GetService();

        // Act
        var result = service.Upsert(upsertCommand);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(_hotelId);
        _mockedHotelRepository.Verify(x => x.Get(It.Is<Guid>(id => id == _hotelId)), Times.Once);
        
        _mockedHotelRepository.Verify(x => x.Upsert(It.Is<Hotel>(h =>
            h.Id == _hotelId && 
            h.Name == upsertCommand.Name && 
            h.Location.X == upsertCommand.Longitude &&
            h.Location.Y == upsertCommand.Latitude && 
            h.Price.RegularPrice == upsertCommand.Price && 
            h.Price.Discount == upsertCommand.Discount)), Times.Once);
        
        _mockedHotelRepository.VerifyNoOtherCalls();
    }
    
    [Test(Description = "Repository should not be called to get hotel entity when id (in command object) is not present")]
    public void Upsert_InsertCase_ShouldReturnSuccess()
    {
        // Arrange
        _hotel.SetPrivateProperty(nameof(_hotel.Id), _hotelId);
        var upsertCommand = GetValidHotelUpsertCommand();
        
        var service = GetService();

        // Act
        var result = service.Upsert(upsertCommand);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockedHotelRepository.Verify(x => x.Upsert(It.Is<Hotel>(h =>
            h.Id == result.Value && 
            h.Name == upsertCommand.Name && 
            h.Location.X == upsertCommand.Longitude &&
            h.Location.Y == upsertCommand.Latitude && 
            h.Price.RegularPrice == upsertCommand.Price && 
            h.Price.Discount == upsertCommand.Discount)), Times.Once);
        
        _mockedHotelRepository.VerifyNoOtherCalls();
    }

    protected override void InstantiateDependencies()
    {
        _mockedHotelRepository = new Mock<IHotelRepository>();
        _mockedLogger = new Mock<ILogger<HotelService>>();
    }

    protected override IHotelService GetService()
    {
        return new HotelService(_mockedHotelRepository.Object, _mockedLogger.Object);
    }
}