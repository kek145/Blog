using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.BL.ArticleService;
using BlogAPI.BL.CommentService;
using BlogAPI.DAL.DTOs.ArticleDTOs;
using BlogAPI.DAL.DTOs.CommentDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController : ControllerBase
{
    private readonly IArticleService _articleService;
    private readonly ICommentService _commentService;

    public ArticleController(IArticleService articleService, ICommentService commentService)
    {
        _articleService = articleService;
        _commentService = commentService;
    }

    [HttpPost]
    [Route("CreateArticle")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Author")]
    public async Task<IActionResult> CreateArticle([FromBody] ArticleCreateDto request)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _articleService.CreateNewArticleAsync(request, token);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.Conflict => Conflict(new { response.Description }),
            Domain.Enum.StatusCode.Unauthorized => Unauthorized(new { error = response.Description }),
            Domain.Enum.StatusCode.NotFound => NotFound(new { error = response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { error = response.Description }),
            _ => StatusCode(201, new { response.Description })
        };
    }

    [HttpPost]
    [Route("Article/{articleId:int}/AddComment")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public async Task<IActionResult> AddComment([FromBody] CommentAddDto request, int articleId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _commentService.AddCommentAsync(request, token, articleId);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => NotFound(new { response.Description }),
            Domain.Enum.StatusCode.Unauthorized => Unauthorized(new { response.Description }),
            _ => StatusCode(201, new { response.Description })
        };
    }

    [HttpGet]
    [Route("AllArticles")]
    public async Task<IActionResult> GetAllArticles()
    {
        var response = await _articleService.GetAllArticlesAsync();
        return response.StatusCode == Domain.Enum.StatusCode.InternalServerError ? StatusCode(500, new { error = "Article is not found!"}) : Ok(new { response.Data });
    }

    [HttpGet]
    [Route("GetAllArticle/{query}")]
    public async Task<IActionResult> GetAllArticlesBySearch(string query)
    {
        var response = await _articleService.GetArticleBySearchAsync(query);
        return response.StatusCode == Domain.Enum.StatusCode.InternalServerError ? StatusCode(500, new { error = response.Description }) : Ok(new { response.Data });
    }
    
    [HttpGet]
    [Route("GetArticleById/{articleId:int}")]
    public async Task<IActionResult> GetArticleById(int articleId)
    {
        var response = await _articleService.GetArticleByIdAsync(articleId);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => NotFound(new { error = response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { error = response.Description }),
            _ => Ok(new { response.Data })
        };
    }
    
    [HttpGet]
    [Route("GetAllArticlesByCategory/{category}")]
    public async Task<IActionResult> GetAllArticlesByCategory(string category)
    {
        var response = await _articleService.GetAllArticlesByCategoryAsync(category);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => NotFound(new { error = response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { error = response.Description }),
            _ => Ok(new { response.Data })
        };
    }

    [HttpGet]
    [Route("GetArticleById/{articleId:int}/Comments")]
    public async Task<IActionResult> GetAllComments(int articleId)
    {
        var response = await _commentService.GetAllCommentsByArticleAsync(articleId);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => NotFound(new { response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { response.Description }),
            _ => Ok(new { response.Data })
        };
    }

    [HttpPut]
    [Route("UpdateArticle/{articleId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author")]
    public async Task<IActionResult> UpdateArticle([FromBody] ArticleUpdateDto request, int articleId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _articleService.UpdateArticleAsync(request, token, articleId);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.Unauthorized => Unauthorized(new { error = response.Description }),
            Domain.Enum.StatusCode.NotFound => NotFound(new { error = response.Description }),
            Domain.Enum.StatusCode.Conflict => Conflict(new { error = response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { response.Description }),
            _ => Ok(new { response.Data })
        };
    }

    [HttpPut]
    [Route("UpdateArticle/{articleId:int}/Comment/{commentId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateDto request, int articleId, int commentId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _commentService.UpdateCommentAsync(request, token, articleId, commentId);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => NotFound(new { error = response.Description }),
            Domain.Enum.StatusCode.Unauthorized => Unauthorized(new { error = response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { error = response.Description }),
            _ => Ok(new { response.Description })
        };
    }


    [HttpDelete]
    [Route("DeleteArticle/{articleId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author")]
    public async Task<IActionResult> DeleteArticle(int articleId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _articleService.DeleteArticleAsync(token, articleId);

        if (response.StatusCode == Domain.Enum.StatusCode.NotFound)
            return NotFound(new { response.Description });
        
        return response.StatusCode == Domain.Enum.StatusCode.InternalServerError ? StatusCode(500, new { error = response.Description }) : NoContent();
    }

    [HttpDelete]
    [Route("Article/{articleId:int}/Comment/{commentId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public async Task<IActionResult> DeleteComment(int articleId, int commentId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _commentService.DeleteCommentAsync(token, articleId, commentId);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => NotFound(new { response.Description }),
            Domain.Enum.StatusCode.Unauthorized => Unauthorized(new { error = response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { error = response.Description }),
            _ => NoContent()
        };
    }
}