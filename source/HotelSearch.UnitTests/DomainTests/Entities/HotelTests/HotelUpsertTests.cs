using AutoFixture;
using FluentAssertions;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Exceptions;
using NUnit.Framework;

namespace HotelSearch.UnitTests.DomainTests.Entities.HotelTests;

class HotelUpsertTests: BaseTest
{
    HotelUpsertCommand _hotelUpsertCommand;
    private Hotel _hotel;

    [SetUp]
    public void Setup()
    {
        _hotel = Fixture.Create<Hotel>();
    }
    
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(101)]
    public void Upsert_DiscountIsNotInRange_ShouldThrowValidationException(int? discount)
    {
        // Arrange
        _hotelUpsertCommand = Fixture.Build<HotelUpsertCommand>()
            .With(x => x.Discount, discount)
            .Create();
        
        var action = () => _hotel.Upsert(_hotelUpsertCommand);

        // Act and assert
        action.Should()
            .ThrowExactly<ValidationFailedException>()
            .Which
            .ValidationErrors
            .Should()
            .Contain(x => x.Key == nameof(_hotelUpsertCommand.Discount) && x.Value.Contains("Discount must be a value between 1 and 100."));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Upsert_NameIsEmpty_ShouldThrowValidationException(string name)
    {
        // Arrange
        _hotelUpsertCommand = Fixture.Build<HotelUpsertCommand>()
            .With(x => x.Name, name)
            .Create();
        
        var action = () => _hotel.Upsert(_hotelUpsertCommand);

        // Act and assert
        action.Should()
            .ThrowExactly<ValidationFailedException>()
            .Which
            .ValidationErrors
            .Should()
            .Contain(x => x.Key == nameof(_hotelUpsertCommand.Name) 
                    && x.Value.Contains("Name cannot be empty."));
    }
    
    [TestCase("Hotel name!")]
    [TestCase("Hotel-name")]
    [TestCase("Hotel name :D")]
    [TestCase("Hotel name *****")]
    [TestCase("____Hotel_name____")]
    public void Upsert_NameContainsNonAlphanumericCharacters_ShouldThrowValidationException(string name)
    {
        // Arrange
        _hotelUpsertCommand = Fixture.Build<HotelUpsertCommand>()
            .With(x => x.Name, name)
            .Create();
        
        var action = () => _hotel.Upsert(_hotelUpsertCommand);

        // Act and assert
        action.Should()
            .ThrowExactly<ValidationFailedException>()
            .Which
            .ValidationErrors
            .Should()
            .Contain(x => x.Key == nameof(_hotelUpsertCommand.Name)
                && x.Value.Contains("Name must contain only alphanumeric characters and spaces."));
    }
    
    [TestCase("h")]
    [TestCase("ho")]
    [TestCase("hot")]
    [TestCase("hote")]
    [TestCase("Grand Royal Imperial International Seaside & Luxury Palace Resort & Conference Center Dubrovnik  2025"),Description("101 characters")]
    public void Upsert_NameLengthIsNotInRange_ShouldThrowValidationException(string name)
    {
        // Arrange
        _hotelUpsertCommand = Fixture.Build<HotelUpsertCommand>()
            .With(x => x.Name, name)
            .Create();
        
        var action = () => _hotel.Upsert(_hotelUpsertCommand);

        // Act and assert
        action.Should()
            .ThrowExactly<ValidationFailedException>()
            .Which
            .ValidationErrors
            .Should()
            .Contain(x => x.Key == nameof(_hotel.Name) 
                && x.Value.Contains("Name must be between 5 and 100 characters long."));
    }
    
    [TestCase(180.1)]
    [TestCase(-180.1)]
    public void Upsert_LongitudeIsNotInRange_ShouldThrowValidationException(double longitude)
    {
        // Arrange
        _hotelUpsertCommand = Fixture.Build<HotelUpsertCommand>()
            .With(x => x.Longitude, longitude)
            .Create();
        
        var action = () => _hotel.Upsert(_hotelUpsertCommand);

        // Act and assert
        action.Should()
            .ThrowExactly<ValidationFailedException>()
            .Which
            .ValidationErrors
            .Should()
            .Contain(x => x.Key == nameof(_hotelUpsertCommand.Longitude) 
                          && x.Value.Contains("Longitude must be between -180 and 180 degrees."));
    }
    
    [TestCase(90.1)]
    [TestCase(-90.1)]
    public void Upsert_LatitudeIsNotInRange_ShouldThrowValidationException(double latitude)
    {
        // Arrange
        _hotelUpsertCommand = Fixture.Build<HotelUpsertCommand>()
            .With(x => x.Latitude, latitude)
            .Create();
        
        var action = () => _hotel.Upsert(_hotelUpsertCommand);

        // Act and assert
        action.Should()
            .ThrowExactly<ValidationFailedException>()
            .Which
            .ValidationErrors
            .Should()
            .Contain(x => x.Key == nameof(_hotelUpsertCommand.Latitude) 
                    && x.Value.Contains("Latitude must be between -90 and 90 degrees."));
    }
    
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-0.000000001)]
    public void Upsert_PriceIsNotGreaterThanZero_ShouldThrowValidationException(decimal price)
    {
        // Arrange
        _hotelUpsertCommand = Fixture.Build<HotelUpsertCommand>()
            .With(x => x.Price, price)
            .Create();
        
        var action = () => _hotel.Upsert(_hotelUpsertCommand);

        // Act and assert
        action.Should()
            .ThrowExactly<ValidationFailedException>()
            .Which
            .ValidationErrors
            .Should()
            .Contain(x => x.Key == nameof(_hotelUpsertCommand.Price) 
                    && x.Value.Contains("Price must be greater than 0."));
    }
    
    [TestCase(16.11, 45.12, "Hotel name", 100, null)]
    [TestCase(20, 40, "Second hotel name", 50, 10)]
    public void Upsert_OrdinaryCase_ShouldUpsertProperties(
        double longitude,
        double latitude,
        string name,
        decimal price,
        int? discount
        )
    {
        // Arrange
        _hotelUpsertCommand = Fixture.Build<HotelUpsertCommand>()
            .With(x => x.Longitude, longitude)
            .With(x => x.Latitude, latitude)
            .With(x => x.Name, name)
            .With(x => x.Price, price)
            .With(x => x.Discount, discount)
            .Create();

        // Act
        _hotel.Upsert(_hotelUpsertCommand);
        
        // Assert
        _hotel.Location.X.Should().Be(longitude);
        _hotel.Location.Y.Should().Be(latitude);
        _hotel.Name.Should().Be(name);
        _hotel.Price.RegularPrice.Should().Be(price);
        _hotel.Price.Discount.Should().Be(discount);
    }
    
    [TestCase(100, null, 100)]
    [TestCase(100, 1, 99)]
    [TestCase(100, 10, 90)]
    [TestCase(100, 50, 50)]
    [TestCase(100, 100, 0)]
    public void Upsert_DiscountApplied_ShouldCalculateDiscountedCorrectPrice(
        decimal price,
        int? discount,
        decimal expectedPricePerNight
    )
    {
        // Arrange
        _hotelUpsertCommand = Fixture.Build<HotelUpsertCommand>()
            .With(x => x.Price, price)
            .With(x => x.Discount, discount)
            .With(x => x.Latitude, 0)
            .With(x => x.Longitude, 0)
            .With(x => x.Name, "Hotel name")
            .Create();

        // Act
        _hotel.Upsert(_hotelUpsertCommand);
        
        // Assert
        _hotel.Price.PerNight.Should().Be(expectedPricePerNight);
    }
}