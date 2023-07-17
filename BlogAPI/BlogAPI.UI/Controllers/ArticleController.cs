using BlogAPI.BL.DTOs;
using System.Threading.Tasks;
using BlogAPI.BL.ArticleService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
public class ArticleController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticleController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpPost]
    [Route("CreateArticle")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Author")]
    public async Task<IActionResult> CreateNewTask([FromBody] ArticleDtoCreate request)
    {
        string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _articleService.CreateNewArticleAsync(request, token);

        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { response.Description });

        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { response.Description });

        return Ok(new { response.Description });
    }

    [HttpGet]
    [Route("SecurityEndPoint")]
    public IActionResult GetAll()
    {
        return Ok("gg");
    }
}