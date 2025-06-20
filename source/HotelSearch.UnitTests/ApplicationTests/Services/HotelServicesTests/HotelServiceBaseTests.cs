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

class HotelServiceBaseTests: BaseServiceTest<IHotelService>
{
    protected Mock<ILogger<HotelService>> MockedLogger;
    protected Mock<IHotelRepository> MockedHotelRepository;
    protected Hotel Hotel;
    protected Guid HotelId;

    [SetUp]
    public void Setup()
    {
        InstantiateDependencies();
        Hotel = Fixture.Create<Hotel>();
        HotelId = Guid.NewGuid();
    }

    protected override void InstantiateDependencies()
    {
        MockedHotelRepository = new Mock<IHotelRepository>();
        MockedLogger = new Mock<ILogger<HotelService>>();
    }

    protected override IHotelService GetService()
    {
        return new HotelService(MockedHotelRepository.Object, MockedLogger.Object);
    }
}