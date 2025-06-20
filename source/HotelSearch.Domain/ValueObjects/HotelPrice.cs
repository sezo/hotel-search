namespace HotelSearch.Domain.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <param name="RegularPrice"></param>
/// <param name="Discount">Discount applied to price in range from 1 to 100 %. Null represents no discount</param>
public record HotelPrice(decimal RegularPrice, int? Discount = null)
{
    /// <summary>
    /// Price for one person per night with applied discount.
    /// </summary>
    public decimal PerNight => Discount.HasValue ? RegularPrice * (1 - (Discount.GetValueOrDefault(100) / 100M)) : RegularPrice;
}