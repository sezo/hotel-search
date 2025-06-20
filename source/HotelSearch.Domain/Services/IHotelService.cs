using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Queries;
using HotelSearch.Domain.Views;

namespace HotelSearch.Domain.Services;

public interface IHotelService
{
   /// <summary>
   /// Deletes hotel based on command data.
   /// </summary>
   /// <param name="command"></param>
   /// <returns></returns>
   OperationResult<Guid> Delete(HotelDeleteCommand command);
   
   /// <summary>
   /// returns hotel view.
   /// </summary>
   /// <param name="id"></param>
   /// <returns></returns>
   HotelView Get(Guid id);
   
   
   /// <summary>
   /// Gets all hotels by name.
   /// </summary>
   /// <param name="page"></param>
   /// <param name="pageSize"></param>
   /// <returns></returns>
   List<HotelView> GetAll(int page = 1, int pageSize = 10);

   /// <summary>
   /// Returns hotel views list based on query request.
   /// </summary>
   /// <param name="query"></param>
   /// <returns></returns>
   List<HotelView> Search(HotelSearchQuery query);
   
   /// <summary>
   /// Creates or updates hotel entity based on command data.
   /// </summary>
   /// <param name="command"></param>
   /// <returns></returns>
   OperationResult<Guid> Upsert(HotelUpsertCommand command);
   

}