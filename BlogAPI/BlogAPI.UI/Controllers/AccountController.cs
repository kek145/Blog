using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.BL.DTOs.AuthenticationDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
public class AccountController : ControllerBase
{
    [HttpPost]
    [Route("Auth-Info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> EditAuthenticationInfo([FromBody] EditAuthenticationDto request)
    {
        return Ok();
    }
}