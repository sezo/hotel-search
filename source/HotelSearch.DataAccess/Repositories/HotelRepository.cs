using System.Collections.Concurrent;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Queries;
using HotelSearch.Domain.Repositories;
using HotelSearch.Domain.Views;
using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

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
    
    public List<HotelView> GetAll(int page = 1, int pageSize = 10)
    {
        page = page < 1 ? 0 : page - 1;
        pageSize = pageSize < 1 || pageSize > 100 ? 10 : pageSize;
        
        return _hotels
            .Values
            .OrderBy(x => x.Name)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(x => new HotelView()
            {
                Name = x.Name,
                Price = x.Price.PerNight
            })
            .ToList();
    }

    public List<HotelView> Search(HotelSearchQuery query)
    {
        var page = query.Page.GetValueOrDefault() < 1 ? 0 : query.Page.Value - 1;
        var pageSize = query.PageSize.GetValueOrDefault();
        pageSize = pageSize < 1 || pageSize > 100 ? 10 : pageSize;
        
        return _hotels
            .Values
            .OrderBy(x => x.Price.PerNight)
            .ThenBy(x => x.Location.Distance(new Point(query.Longitude, query.Latitude)))
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(x => new HotelView
            {
                Name = x.Name,
                Distance = DistanceInMeters( x.Location, new Point(query.Longitude, query.Latitude)),
                Price = x.Price.PerNight
            })
            .ToList();
    }

    public void Upsert(Hotel hotel)
    {
        _hotels[hotel.Id] = hotel;
    }
    
    private double DistanceInMeters(Point point1, Point point2)
    {
        // Set up coordinate systems
        var wgs84 = GeographicCoordinateSystem.WGS84;
        var webMercator = ProjectedCoordinateSystem.WebMercator;

        // Create a transformer
        var transform = new CoordinateTransformationFactory()
            .CreateFromCoordinateSystems(wgs84, webMercator);

        // Transform both points to meters
        var coord1 = transform.MathTransform.Transform(new[] { point1.X, point1.Y });
        var coord2 = transform.MathTransform.Transform(new[] { point2.X, point2.Y });

        var p1 = new Point(coord1[0], coord1[1]);
        var p2 = new Point(coord2[0], coord2[1]);

        return p1.Distance(p2); 
    }
}