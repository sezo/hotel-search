using FluentValidation;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Entities;

namespace HotelSearch.Domain.Validators;

internal class HotelUpsertCommandValidator: AbstractValidator<HotelUpsertCommand>
{
    public HotelUpsertCommandValidator()
    {
        Include(new BaseCommandValidator<Guid?>());
        
        RuleFor(x => x.Discount)
            .InclusiveBetween(1, 100)
            .When(x => x.Discount.HasValue)
            .WithMessage("Discount must be a value between 1 and 100.");
        
        RuleFor(x => x.Latitude)
            .SetValidator(new LatitudeValidator());

        RuleFor(x => x.Longitude)
            .SetValidator(new LongitudeValidator());
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .Length(5,100)
            .WithMessage("Name must be between 5 and 100 characters long.")
            .Matches(@"^[\p{L}0-9 ]+$")
            .WithMessage("Name must contain only alphanumeric characters and spaces.");
        
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }
}