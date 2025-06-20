using AutoFixture;
using HotelSearch.Domain.Commands;

namespace HotelSearch.UnitTests;

internal class BaseTest
{
    public readonly Fixture Fixture;

    public BaseTest()
    {
        Fixture = new Fixture();
    }

    protected HotelUpsertCommand GetValidHotelUpsertCommand(
        Guid? id = null,
        string name = "Hotel name",
        double longitude = 90,
        double latitude = 90,
        decimal price = 100,
        int? discount = null
        )
    {
        return new HotelUpsertCommand(id, name, longitude, latitude, price, discount);
    }

}