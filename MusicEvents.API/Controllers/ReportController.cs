using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Services.Interfaces;

namespace MusicEvents.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly ISaleService _service;

    public ReportController(ISaleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Sales(int genreId, string dateInit, string dateEnd)
    {
        return Ok(await _service.GetReportSales(genreId, dateInit, dateEnd));
    }
}