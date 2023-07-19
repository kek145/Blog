using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.BL.AuthenticationService;
using BlogAPI.BL.DTOs.AuthenticationDto;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("SignIn")]
    public async Task<IActionResult> AuthenticationAccount([FromBody] AuthenticationDto request)
    {
        try
        {
            var response = await _authenticationService.AuthenticationAsync(request);

            if (response == null!)
                return BadRequest(new { error = "Wrong email or password!" });

            return Ok(new { token = response });
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}