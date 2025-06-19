namespace HotelSearch.Domain.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <param name="Price"></param>
/// <param name="Discount">Discount applied to price in range from 1 to 100 %. Null represents no discount</param>
public record HotelPrice(decimal Price, int? Discount = null)
{
    /// <summary>
    /// Price for one person per night with applied discount.
    /// </summary>
    public decimal PerNight => Price * Discount.GetValueOrDefault(100) / 100;
}