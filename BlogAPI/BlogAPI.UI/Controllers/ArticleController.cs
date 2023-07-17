using System.Threading.Tasks;
using BlogAPI.BL.DTOs;
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

    [HttpPost]
    [Route("CreateArticle")]
    public async Task<IActionResult> CreateNewTask([FromBody] ArticleDtoCreate request)
    {
        return Ok();
    }

    [HttpGet]
    [Route("SecurityEndPoint")]
    public IActionResult GetAll()
    {
        return Ok("gg");
    }
}