using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Dto.Request;
using MusicEvents.Dto.Response;
using MusicEvents.Entities;
using MusicEvents.Services.Interfaces;

namespace MusicEvents.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GenreController : ControllerBase
{
    private readonly IGenreService _service;

    public GenreController(IGenreService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<BaseResponseGeneric<ICollection<Genre>>>> Get()
    {
        return Ok(await _service.GetAsync());
    }

    [HttpGet("{id:int}")]
    //[HttpGet]
    //[Route("GetById")]
    // ?id=6&subid=23&parametrox=455
    public async Task<ActionResult<BaseResponseGeneric<Genre>>> Get(int id)
    {
        return Ok(await _service.GetAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponseGeneric<int>>> Post(DtoGenre request)
    {
        var response = await _service.CreateAsync(request);

        HttpContext.Response.Headers.Add("location", $"/api/genre/{response.Result}");

        return Ok(response);
    }


    // http://localhost/api/genre/5
    // body { "description":"valor nuevo" }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BaseResponseGeneric<int>>> Put(int id, DtoGenre request)
    {
        var response = await _service.UpdateAsync(id, request);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<BaseResponseGeneric<int>>> Delete(int id)
    {
        var response = await _service.DeleteAsync(id);

        return Ok(response);
    }

}