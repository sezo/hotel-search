using HotelSearch.Domain.Entities;

namespace HotelSearch.Domain.Views;

public class BaseView<T>
{
    public T Id { get; set; }
}