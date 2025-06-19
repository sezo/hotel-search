namespace HotelSearch.Domain.Commands;

public abstract record BaseCommand<T>
{
    /// <summary>
    /// If Id is supplied update will occur, otherwise create action is trigger.
    /// </summary>
    public T? Id { get; init; }
    
    public BaseCommand(T? id)
    {
        Id = id;
    }
}