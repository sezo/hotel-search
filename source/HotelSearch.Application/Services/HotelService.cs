using HotelSearch.Domain;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Exceptions;
using HotelSearch.Domain.Repositories;
using HotelSearch.Domain.Services;
using Microsoft.Extensions.Logging;

namespace HotelSearch.Application.Services;

public class HotelService: IHotelService
{
    private readonly ILogger<HotelService> _logger;
    private readonly IHotelRepository _hotelRepository;
    public HotelService(IHotelRepository hotelRepository, ILogger<HotelService> logger)
    {
        _hotelRepository = hotelRepository;
        _logger = logger;
    }
    
    public OperationResult<Guid> Upsert(HotelUpsertCommand command)
    {
        if (command is null)
        {
            _logger.LogError("Upsert command is null");
            throw new ArgumentNullException();
        }
        
        var hotel = command.Id.HasValue ? GetHotel(command.Id.Value) : new Hotel();
        hotel.Upsert(command);
        try
        {
            _hotelRepository.Upsert(hotel);
            _logger.LogInformation("Hotel {Name} with id {Id} upserted", hotel.Name, hotel.Id);
        
            return OperationResult<Guid>.Success(hotel.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hotel upsert for {Name} with id {Id} failed", hotel.Name, hotel.Id);
            throw new HotelUpsertFailedException(hotel.Id);
        }
    }

    /// <summary>
    /// Gets the hotel entity by id. In case if hotel doesnt exists in repository throws HotelNotFoundException.
    /// </summary>
    /// <param name="hotelId"></param>
    /// <returns></returns>
    /// <exception cref="HotelNotFoundException"></exception>
    private Hotel GetHotel(Guid hotelId)
    {
        var hotel = _hotelRepository.Get(hotelId);
        if (hotel is null)
        {
            throw new HotelNotFoundException(hotelId);
        }

        return hotel;
    }
}