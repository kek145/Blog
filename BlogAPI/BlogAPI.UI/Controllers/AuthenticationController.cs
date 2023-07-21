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

            return response.StatusCode switch
            {
                Domain.Enum.StatusCode.Unauthorized => Unauthorized(new { error = response.Description }),
                Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { error = response.Description }),
                _ => Ok(new { token = response.Data })
            };
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}