using FluentValidation;
using HotelSearch.Domain.Queries;

namespace HotelSearch.Domain.Validators;

public class HotelSearchQueryValidator: AbstractValidator<HotelSearchQuery>
{
    public HotelSearchQueryValidator()
    {
        RuleFor(x => x.Longitude)
            .SetValidator(new LongitudeValidator());
        
        RuleFor(x => x.Latitude)
            .SetValidator(new LatitudeValidator());
    }
}