using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.API.Filters;
using MusicEvents.Dto.Request;
using MusicEvents.Dto.Response;
using MusicEvents.Services.Interfaces;

namespace MusicEvents.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[TypeFilter(typeof(MusicFilterErrors))]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponseGeneric<string>), 200)]
    [ProducesResponseType(typeof(BaseResponseGeneric<string>), 400)]
    public async Task<IActionResult> Register(DtoRegisterUser request)
    {
        var response = await _service.RegisterAsync(request);

        return response.Success ? Ok(response) : BadRequest(response);
    }


    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(DtoLoginResponse), 200)]
    [ProducesResponseType(typeof(DtoLoginResponse), 403)]
    public async Task<IActionResult> Login(DtoLogin request)
    {
        var response = await _service.LoginAsync(request);

        return response.Success ? Ok(response) : Unauthorized(response);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SendTokenToResetPassword([FromBody] DtoResetPassword request)
    {
        return Ok(await _service.SendTokenToResetPasswordAsync(request));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(DtoConfirmReset request)
    {
        return Ok(await _service.ResetPassword(request));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(DtoChangePassword request)
    {
        return Ok(await _service.ChangePassword(request));
    }
}