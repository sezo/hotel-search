namespace HotelSearch.Domain.Commands;
public record HotelUpsertCommand: BaseCommand<Guid?>
{
    /// Hotel longitude (X).
    public double Longitude { get; init; }

    /// Hotel latitude (Y).
    public double Latitude { get; init; }

    /// Hotel display name.
    public string Name { get; init; }

    public decimal Price { get; init; }

    /// Discount applied to price used to calculate price per night.
    public int? Discount { get; init; }

    public HotelUpsertCommand(Guid? id, string name, double longitude, double latitude, decimal price, int? discount)
        : base(id)
    {
        Longitude = longitude;
        Latitude = latitude;
        Name = name;
        Price = price;
        Discount = discount;
    }
}