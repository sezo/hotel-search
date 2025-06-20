using System.Collections.Concurrent;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Repositories;

namespace HotelSearch.DataAccess.Repositories;

public class HotelRepository: IHotelRepository
{
    private readonly ConcurrentDictionary<Guid, Hotel> _hotels = new();
    public void Delete(Guid id)
    {
        _hotels.TryRemove(id, out Hotel hotel);
    }

    public Hotel Get(Guid id)
    {
        _hotels.TryGetValue(id, out var hotel); 
        return hotel;
    }
    
    public IEnumerable<Hotel> GetAll(int page = 1, int pageSize = 10)
    {
        page = page < 1 ? 1 : page;
        
        return _hotels
            .Values
            .OrderBy(x => x.Name)
            .Skip(page * pageSize)
            .Take(pageSize);
    }

    public void Upsert(Hotel hotel)
    {
        _hotels[hotel.Id] = hotel;
    }
}