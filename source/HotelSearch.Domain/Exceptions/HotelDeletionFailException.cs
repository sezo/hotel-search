namespace HotelSearch.Domain.Exceptions;

public class HotelDeletionFailException: Exception
{
    public HotelDeletionFailException(Guid hotelId):base($"Hotel with id {hotelId} deletion fail.")
    {
        
    }
}