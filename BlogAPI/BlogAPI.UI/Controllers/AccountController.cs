using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.BL.AccountService;
using BlogAPI.BL.ArticleService;
using BlogAPI.Domain.DTOs.AuthenticationDto;
using BlogAPI.Domain.DTOs.EditUserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IArticleService _articleService;

    public AccountController(IAccountService accountService, IArticleService articleService)
    {
        _accountService = accountService;
        _articleService = articleService;
    }

    [HttpGet]
    [Route("GetUserInfo")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
    public async Task<IActionResult> GetUserInfo()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _accountService.GetUserInfo(token);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { response.Description }),
            _ => Ok(new { response.Data })
        };
    }

    [HttpGet]
    [Route("GetAuthorArticles/{userId:int}")]
    public async Task<IActionResult> GetAuthorArticles(int userId)
    {
        var response = await _articleService.GetAllArticlesByUserAsync(userId);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => NotFound(new { response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { response.Description }),
            _ => Ok(new { response.Data })
        };
    }

    [HttpPut]
    [Route("User-Info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
    public async Task<IActionResult> EditUserInfo([FromBody] UpdateUserDto request)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _accountService.UpdateUserInfoAsync(request, token);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => NotFound(new { error = response.Description }),
            Domain.Enum.StatusCode.Unauthorized => Unauthorized(new { error = response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { error = response.Description }),
            _ => Ok(new { response.Description })
        };
    }
    
    [HttpPut]
    [Route("Auth-Info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
    public async Task<IActionResult> EditAuthenticationInfo([FromBody] UpdateAuthenticationDto request)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _accountService.UpdateAuthenticationInfoAsync(request, token);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => BadRequest(new { error = response.Description }),
            Domain.Enum.StatusCode.Conflict => Conflict(new { error = response.Description }),
            Domain.Enum.StatusCode.Unauthorized => Unauthorized(new { error = response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { error = response.Description }),
            _ => Ok(new { response.Description })
        };
    }

    [HttpDelete]
    [Route("DeleteAccount")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
    public async Task<IActionResult> DeleteUserAccount()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _accountService.DeleteUserAccountAsync(token);

        return response.StatusCode switch
        {
            Domain.Enum.StatusCode.NotFound => BadRequest(new { error = response.Description }),
            Domain.Enum.StatusCode.Unauthorized => Unauthorized(new { error = response.Description }),
            Domain.Enum.StatusCode.InternalServerError => StatusCode(500, new { error = response.Description }),
            _ => NoContent()
        };
    }
}