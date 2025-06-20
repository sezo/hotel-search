namespace HotelSearch.Domain.Entities;

public abstract class IdEntity<T>
{
    public T Id { get; private set; }
}