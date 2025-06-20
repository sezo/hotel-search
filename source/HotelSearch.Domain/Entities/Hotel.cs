using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Exceptions;
using HotelSearch.Domain.Validators;
using HotelSearch.Domain.ValueObjects;
using NetTopologySuite.Geometries;

namespace HotelSearch.Domain.Entities;

public class Hotel: IdEntity<Guid>
{
    /// <summary>
    /// Hotel geographical location.
    /// </summary>
    public Point Location { get; private set; }
    
    /// <summary>
    /// Hotel display name. It is not unique.
    /// </summary>
    public string Name { get; private set; }
    
    /// <summary>
    /// Hotel price per
    /// </summary>
    public HotelPrice Price { get; private set; }
    
    
    public void Upsert(HotelUpsertCommand command)
    {
        var validationResult = new HotelUpsertCommandValidator().Validate(command);

        if (!validationResult.IsValid)
        {
            throw new ValidationFailedException("Hotel validation failed.", validationResult);
        }
        
        Location = new Point(command.Longitude, command.Latitude);
        Name = command.Name;
        Price = new HotelPrice(command.Price, command.Discount);
        
    }
}