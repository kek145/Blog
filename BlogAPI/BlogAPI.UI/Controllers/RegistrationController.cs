using BlogAPI.BL.DTOs.RegistrationDto;
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
    [Route("SignUp")]
    public async Task<IActionResult> RegistrationAccount([FromBody] RegistrationDto request)
    {
        var response = await _registrationService.RegistrationServiceAsync(request);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => NotFound(new { error = response.Description }),
            Domain.Enum.StatusCode.Conflict => Conflict(new { error = response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { error = response.Description }),
            _ => StatusCode(201, new { success = response.Description })
        };
    }
}