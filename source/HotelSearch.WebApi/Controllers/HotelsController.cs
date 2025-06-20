using System.Net;
using HotelSearch.Domain;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.Queries;
using HotelSearch.Domain.Services;
using HotelSearch.Domain.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelSearch.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class HotelsController : Controller
{
    private readonly IHotelService _hotelService;
    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }
    
    
    [HttpGet]
    [SwaggerOperation(Summary = "Gets all hotels")]
    [ProducesResponseType(typeof(HotelView), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
    public ActionResult<List<HotelView>> Index(int? page = 1, int? pageSize = 10)
    {
        return _hotelService.GetAll(page.GetValueOrDefault(), pageSize.GetValueOrDefault());
    }
    
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets specific hotel by id")]
    [ProducesResponseType(typeof(HotelView), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
    public ActionResult<HotelView> GetById(Guid id)
    {
        return _hotelService.Get(id);
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Upserts hotel")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
    public ActionResult<OperationResult<Guid>> Post([FromBody] HotelUpsertCommand command)
    {
        return _hotelService.Upsert(command);
    }
    
    [HttpDelete]
    [SwaggerOperation(Summary = "Deletes hotels")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
    public ActionResult<OperationResult<Guid>> Delete([FromBody] HotelDeleteCommand command)
    {
        return _hotelService.Delete(command);
    }
    
    [HttpGet("search")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Search hotels near given location orderd by price and distance")]
    [ProducesResponseType(typeof(HotelSearchView), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
    public ActionResult<HotelSearchView> Search([FromQuery] HotelSearchQuery query)
    {
        return _hotelService.Search(query);
    }
}