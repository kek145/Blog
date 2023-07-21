﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.BL.ArticleService;
using BlogAPI.BL.DTOs.ArticleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> CreateArticle([FromBody] ArticleCreateDto request)
    {
        string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _articleService.CreateNewArticleAsync(request, token);

        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { response.Description });

        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { response.Description });

        return Ok(new { response.Description });
    }

    [HttpPost]
    [Route("Article/{articleId:int}/AddComment")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public async Task<IActionResult> AddComment()
    {
        return Ok();
    }

    [HttpGet]
    [Route("AllArticles")]
    public async Task<IActionResult> GetAllArticles()
    {
        var response = await _articleService.GetAllArticlesAsync();
        if (response == null!)
            return BadRequest(new { error = "Article is not found!"});

        return Ok(new { response.Data });
    }

    [HttpGet]
    [Route("GetAllArticle/{query}")]
    public async Task<IActionResult> GetAllArticlesBySearch(string query)
    {
        var response = await _articleService.GetArticleBySearchAsync(query);
        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { error = response.Description });

        return Ok(new { response.Data });
    }
    
    [HttpGet]
    [Route("GetArticleById/{articleId:int}")]
    public async Task<IActionResult> GetArticleById(int articleId)
    {
        var response = await _articleService.GetArticleByIdAsync(articleId);

        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { error = response.Description });

        return Ok(new { response.Data });
    }
    
    [HttpGet]
    [Route("GetAllArticlesByCategory/{category}")]
    public async Task<IActionResult> GetAllArticlesByCategory(string category)
    {
        var response = await _articleService.GetAllArticlesByCategoryAsync(category);

        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { error = response.Description });
        
        if(response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { error = response.Description });

        return Ok(new { response.Data });
    }

    [HttpGet]
    [Route("GetArticleById/{articleId:int}/Comments")]
    public async Task<IActionResult> GetAllComments()
    {
        return Ok();
    }

    [HttpPut]
    [Route("UpdateArticle/{articleId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author")]
    public async Task<IActionResult> UpdateArticle([FromBody] ArticleUpdateDto request, int articleId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _articleService.UpdateArticleAsync(request, token, articleId);

        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { error = response.Description });
        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { error = response.Description });

        return Ok(new { response.Description });
    }

    [HttpPut]
    [Route("UpdateArticle/{articleId:int}/Comment/{commentId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public async Task<IActionResult> UpdateComment()
    {
        return Ok();
    }


    [HttpDelete]
    [Route("DeleteArticle/{articleId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author")]
    public async Task<IActionResult> DeleteArticle(int articleId)
    {
        string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _articleService.DeleteArticleAsync(token, articleId);

        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { response.Description });
        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { error = response.Description });

        return Ok(new { response.Description });
    }

    [HttpDelete]
    [Route("Article/{articleId:int}/Comment/{commentId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public async Task<IActionResult> DeleteComment()
    {
        return Ok();
    }
}