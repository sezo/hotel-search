using HotelSearch.Domain.Entities;

namespace HotelSearch.Domain.Repositories;

public interface IHotelRepository
{
    
    /// <summary>
    /// Deletes hotel from repository.
    /// </summary>
    /// <param name="id"></param>
    void Delete(Guid id);
    
    /// <summary>
    /// Gets hotel entity by it's id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Hotel Get(Guid id);
    
    IEnumerable<Hotel> GetAll(int page = 0, int pageSize = 10);
    
    void Upsert(Hotel hotel);
}