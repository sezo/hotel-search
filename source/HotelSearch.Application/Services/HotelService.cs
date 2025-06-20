using HotelSearch.Domain;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Services;

namespace HotelSearch.Application.Services;

public class HotelService: IHotelService
{
    public OperationResult<Guid> Upsert(HotelUpsertCommand command)
    {
        throw new NotImplementedException();
    }
}