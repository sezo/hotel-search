using AutoFixture;

namespace HotelSearch.UnitTests;

internal class BaseTest
{
    public readonly Fixture Fixture;

    public BaseTest()
    {
        Fixture = new Fixture();
    }
}