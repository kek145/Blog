using System.Threading.Tasks;
using BlogAPI.BL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController : ControllerBase
{
    public RegistrationController()
    {
        
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> RegistrationAccount([FromBody] RegistrationDto request)
    {
        return Ok();
    }
}