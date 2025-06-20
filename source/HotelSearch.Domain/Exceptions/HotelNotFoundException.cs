namespace HotelSearch.Domain.Exceptions;

public class HotelNotFoundException: Exception
{
    public HotelNotFoundException(Guid hotelId):base($"Hotel with id {hotelId} not found.")
    {
        
    }
}