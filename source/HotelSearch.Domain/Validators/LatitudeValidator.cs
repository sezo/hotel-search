using FluentValidation;

namespace HotelSearch.Domain.Validators;
public class LatitudeValidator : AbstractValidator<double>
{
    public LatitudeValidator()
    {
        RuleFor(x => x)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90 degrees.");

    }
}