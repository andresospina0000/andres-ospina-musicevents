using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Dto.Request;
using MusicEvents.Dto.Response;
using MusicEvents.Entities.Complex;
using MusicEvents.Services.Interfaces;

namespace MusicEvents.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class SaleController : ControllerBase
{
    private readonly ISaleService _service;

    public SaleController(ISaleService service)
    {
        _service = service;
    }


    [HttpPost]
    public async Task<ActionResult<BaseResponseGeneric<int>>> Create(DtoSale request)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid);

        if (userId == null) return Unauthorized();

        return Ok(await _service.CreateAsync(request, userId.Value));

    }


    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<BaseResponseGeneric<ICollection<SaleInfo>>>> GetById(int id)
    {
        return Ok(await _service.GetSaleById(id));
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponseGeneric<ICollection<SaleInfo>>>> GetCollection(int genreId = 1,
        string dateInit = "", string dateEnd = "")
    {
        return Ok(await _service.GetCollection(genreId, dateInit, dateEnd));
    }
    
    [HttpGet]
    public async Task<ActionResult<BaseResponseGeneric<ICollection<SaleInfo>>>> GetByUser()
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid);

        if (userId == null) return Unauthorized();

        return Ok(await _service.GetSaleByUserId(userId.Value));
    }
}