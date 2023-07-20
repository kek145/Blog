using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.BL.DTOs.EditUserDto;
using BlogAPI.BL.AccountService;
using BlogAPI.BL.ArticleService;
using BlogAPI.BL.DTOs.AuthenticationDto;
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
    [Route("GetAuthorArticles/{userId:int}")]
    public async Task<IActionResult> GetAuthorArticles(int userId)
    {
        var response = await _articleService.GetAllArticlesByUserAsync(userId);

        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { response.Description });

        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { response.Description });
        
        return Ok(new { response.Data });
    }

    [HttpPut]
    [Route("User-Info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
    public async Task<IActionResult> EditUserInfo([FromBody] UpdateUserDto request)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _accountService.UpdateUserInfoAsync(request, token);
        

        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { error = response.Description });

        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { error = response.Description });


        return Ok(new { response.Description });
    }
    
    [HttpPut]
    [Route("Auth-Info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
    public async Task<IActionResult> EditAuthenticationInfo([FromBody] UpdateAuthenticationDto request)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _accountService.UpdateAuthenticationInfoAsync(request, token);
        
        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { error = response.Description });

        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { error = response.Description });
        
        return Ok(new { response.Description });
    }

    [HttpDelete]
    [Route("DeleteAccount")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
    public async Task<IActionResult> DeleteUserAccount()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _accountService.DeleteUserAccountAsync(token);
        
        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { error = response.Description });

        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { error = response.Description });
        
        return Ok(new { response.Description });
    }
}