using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Queries;
using HotelSearch.Domain.Views;

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
    
    List<HotelView> GetAll(int page = 0, int pageSize = 10);
    
    HotelSearchView Search(HotelSearchQuery query);
    
    void Upsert(Hotel hotel);
}