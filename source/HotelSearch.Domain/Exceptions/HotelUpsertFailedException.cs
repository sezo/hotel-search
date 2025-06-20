namespace HotelSearch.Domain.Exceptions;

public class HotelUpsertFailedException: Exception
{
    public HotelUpsertFailedException(Guid hotelId):base($"Hotel with id {hotelId} upsert fail.")
    {
        
    }
}