using FluentValidation;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Entities;

namespace HotelSearch.Domain.Validators;

public class BaseCommandValidator<T>: AbstractValidator<BaseCommand<T>>
{ public BaseCommandValidator()
    {
        if (typeof(T) == typeof(int?))
        {
            RuleFor(x => x.Id)
                .Must(id => Convert.ToInt32(id) >= 0)
                .When(x => x.Id is not null)
                .WithMessage("Id must be greater than zero (0).");
        }
        
        if (typeof(T) == typeof(string))
        {
            RuleFor(x => x.Id)
                .Must(id => !string.IsNullOrWhiteSpace(id.ToString()))
                .When(x => x.Id is not null)
                .WithMessage("Id cannot be null or empty string.");
        }
        
        if (typeof(T) == typeof(Guid))
        {
            RuleFor(x => x.Id)
                .Must(x => Guid.TryParse(x.ToString(), out _))
                .When(x => x.Id is not null)
                .WithMessage("Id must be a valid GUID.");
        }
    }
}