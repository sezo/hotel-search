using HotelSearch.Domain.Commands;

namespace HotelSearch.Domain.Services;

public interface IHotelService
{
   /// <summary>
   /// Creates or updates hotel entity based on command data.
   /// </summary>
   /// <param name="command"></param>
   /// <returns></returns>
   OperationResult<Guid> Upsert(HotelUpsertCommand command);
}