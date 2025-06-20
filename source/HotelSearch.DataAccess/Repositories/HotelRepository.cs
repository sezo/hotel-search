using System.Collections.Concurrent;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Queries;
using HotelSearch.Domain.Repositories;
using HotelSearch.Domain.Views;
using NetTopologySuite.Geometries;

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

    public List<HotelView> Search(HotelSearchQuery query)
    {
        var page = query.Page.GetValueOrDefault() < 1 ? 1 : query.Page.Value;
        var pageSize = query.Page.GetValueOrDefault();
        pageSize = pageSize < 1 || pageSize > 100 ? 100 : pageSize;
        
        return _hotels
            .Values
            .OrderBy(x => x.Price.PerNight)
            .ThenBy(x => x.Location.Distance(new Point(query.Longitude, query.Latitude)))
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(x => new HotelView
            {
                Id = x.Id,
                Name = x.Name,
                Latitude = x.Location.X,
                Longitude = x.Location.Y,
                PricePerNight = x.Price.PerNight
            })
            .ToList();
    }

    public void Upsert(Hotel hotel)
    {
        _hotels[hotel.Id] = hotel;
    }
}