using HotelSearch.Domain;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Exceptions;
using HotelSearch.Domain.Queries;
using HotelSearch.Domain.Repositories;
using HotelSearch.Domain.Services;
using HotelSearch.Domain.Validators;
using HotelSearch.Domain.Views;
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

    public OperationResult<Guid> Delete(HotelDeleteCommand command)
    {
        if (command is null)
        {
            _logger.LogError("Delete command is null");
            throw new ArgumentNullException();
        }
        
        var validationResult = new HotelDeleteCommandValidator().Validate(command);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Upsert command validation failed with errors {ValidationErrors}", validationResult.Errors);
            throw new ValidationFailedException("Hotel pre deletion validation failed.", validationResult);
        }

        try
        {
            _hotelRepository.Delete(command.Id);
            _logger.LogInformation("Hotel with id {Id} deleted", command.Id);
            return OperationResult<Guid>.Success(command.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hotel deletion with id {Id} fails", command.Id);
            throw new HotelDeletionFailException(command.Id);
        }
    }

    public HotelView Get(Guid id)
    {
        var hotel = GetHotel(id);
        return new HotelView()
        {
            Name = hotel.Name,
            Price = hotel.Price.PerNight
        };
    }

    public List<HotelView> GetAll(int page = 0, int pageSize = 10)
    {
        return _hotelRepository.GetAll(page, pageSize);
    }

    public HotelSearchView Search(HotelSearchQuery query)
    {
        if (query is null)
        {
            _logger.LogError("Search query is null");
            throw new ArgumentNullException();
        }

        try
        {
            return _hotelRepository.Search(query);
        }
        catch (Exception ex)
        {
           _logger.LogError(ex, "Hotel search fails for query {Query}", query);
            throw;
        }
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
        catch (ValidationFailedException ex)
        {
            _logger.LogError(ex,"Hotel upsert validation failed");
            throw;
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