using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Dto.Request;
using MusicEvents.Dto.Response;
using MusicEvents.Entities;
using MusicEvents.Entities.Complex;
using MusicEvents.Services.Interfaces;

namespace MusicEvents.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventController : ControllerBase
{
    private readonly IEventService _service;

    public EventController(IEventService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    // filter = ""
    // page = 1 || 2
    // rows = 10 || 10
    public async Task<ActionResult<BaseCollectionResponse<ICollection<ConcertInfo>>>> Get(string filter = ""
        , int page = 1,
        int rows = 10)
    {
        return Ok(await _service.GetAsync(filter ?? string.Empty, page, rows));
    }


    [AllowAnonymous]
    [HttpGet]
    [Route("[action]/{genreId:int}")]
    public async Task<ActionResult<BaseResponseGeneric<ICollection<ConcertInfo>>>> GetByGenre(int genreId)
    {
        return Ok(await _service.GetByGenreAsync(genreId));
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("[action]/{genreId:int}")]
    public async Task<ActionResult<BaseResponseGeneric<ICollection<ConcertInfo>>>> GetMinimalByGenre(int genreId)
    {
        return Ok(await _service.GetMinimalByGenreAsync(genreId));
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<BaseResponseGeneric<Concert>>> Get(int id)
    {
        return Ok(await _service.GetAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponseGeneric<int>>> Post(DtoEvent request)
    {
        var response = await _service.CreateAsync(request);

        HttpContext.Response.Headers.Add("location", $"/api/event/{response.Result}");

        return Ok(response);
    }


    // http://localhost/api/event/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BaseResponseGeneric<int>>> Put(int id, DtoEvent request)
    {
        return Ok(await _service.UpdateAsync(id, request));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<BaseResponseGeneric<int>>> Delete(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Finalize(int id)
    {
        return Ok(await _service.FinalizeAsync(id));
    }
}