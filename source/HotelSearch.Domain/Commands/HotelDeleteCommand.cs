namespace HotelSearch.Domain.Commands;

/// <summary>
/// Delete hotel command. For provided hotel id, tries to execute hotel deletion.
/// </summary>
public record HotelDeleteCommand: BaseCommand<Guid>
{
    public HotelDeleteCommand(Guid id): base(id)
    {
    }
}
