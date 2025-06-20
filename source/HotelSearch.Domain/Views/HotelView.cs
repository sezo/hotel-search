namespace HotelSearch.Domain.Views;

public record HotelView
{
    public string Name { get; init; }
    public double? Distance { get; init; }
    public decimal Price { get; init; }
}