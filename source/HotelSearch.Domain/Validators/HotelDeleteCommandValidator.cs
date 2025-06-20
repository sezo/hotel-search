using FluentValidation;
using HotelSearch.Domain.Commands;

namespace HotelSearch.Domain.Validators;

public class HotelDeleteCommandValidator: AbstractValidator<HotelDeleteCommand>
{
    public HotelDeleteCommandValidator()
    {
        Include(new BaseCommandValidator<Guid>());
    }
}