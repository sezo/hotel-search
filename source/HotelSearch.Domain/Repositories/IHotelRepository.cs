using HotelSearch.Domain.Entities;

namespace HotelSearch.Domain.Repositories;

public interface IHotelRepository
{
    /// <summary>
    /// Gets hotel entity by it's id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Hotel Get(Guid id);
    
    void Upsert(Hotel hotel);
}