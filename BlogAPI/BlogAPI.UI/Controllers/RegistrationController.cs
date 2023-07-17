using BlogAPI.BL.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.BL.RegistrationService;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> RegistrationAccount([FromBody] RegistrationDto request)
    {
        var response = await _registrationService.RegistrationServiceAsync(request);

        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { error = response.Description });

        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { error = "Internal Server error" });

        return Ok(new { success = response.Description });
    }
}