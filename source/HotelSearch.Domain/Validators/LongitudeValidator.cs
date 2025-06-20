using FluentValidation;

namespace HotelSearch.Domain.Validators;
public class LongitudeValidator : AbstractValidator<double>
{
    public LongitudeValidator()
    {
        RuleFor(x => x)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180 degrees.");

    }
}