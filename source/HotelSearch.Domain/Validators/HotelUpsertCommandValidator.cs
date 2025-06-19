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
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90 degrees.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180 degrees.");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .Length(5,100)
            .WithMessage("Name must be between 5 and 100 characters long.")
            .Matches(@"^[a-zA-Z0-9_\-\.]+$")
            .WithMessage("Name must contain only alphanumeric characters and spaces.");
        
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }
}