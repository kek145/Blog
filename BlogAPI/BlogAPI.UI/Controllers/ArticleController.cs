using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
public class ArticleController : ControllerBase
{
    public ArticleController()
    {
        
    }

    [HttpGet]
    [Route("SecurityEndPoint")]
    public IActionResult GetAll()
    {
        return Ok("gg");
    }
}