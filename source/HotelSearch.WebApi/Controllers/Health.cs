using Microsoft.AspNetCore.Mvc;

namespace HotelSearch.WebApi.Controllers;

[Route("api/[controller]")]
public class Health : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}