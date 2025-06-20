namespace HotelSearch.Domain.Views;

public class HotelView : BaseView<Guid>
{
    public string Name { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public decimal PricePerNight { get; set; }
}