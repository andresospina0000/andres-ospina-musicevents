using Microsoft.AspNetCore.Mvc;
using MusicEvents.Dto.Response;
using MusicEvents.Services.Interfaces;

namespace MusicEvents.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IGenreService _genreService;

    public HomeController(IEventService eventService, IGenreService genreService)
    {
        _eventService = eventService;
        _genreService = genreService;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // Listamos ambas listas en un DTO Comun.
        var response = new DtoHomeResponse
        {
            Genres = await _genreService.GetAsync(),
            Events = await _eventService.GetAsync(string.Empty, 1, 100)
        };

        return Ok(response);

    }
}