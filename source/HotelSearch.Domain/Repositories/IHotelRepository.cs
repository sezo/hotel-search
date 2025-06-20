using HotelSearch.Domain.Entities;

namespace HotelSearch.Domain.Repositories;

public interface IHotelRepository
{
    
    /// <summary>
    /// Deletes hotel from repository.
    /// </summary>
    /// <param name="hotelId"></param>
    void Delete(Guid hotelId);
    
    /// <summary>
    /// Gets hotel entity by it's id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Hotel Get(Guid id);
    
    void Upsert(Hotel hotel);
}