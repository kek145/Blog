using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.BL.DTOs.EditUserDto;
using BlogAPI.BL.AccountService;
using BlogAPI.BL.DTOs.AuthenticationDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    [Route("GetMyArticles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Author")]
    public async Task<IActionResult> GetMyArticles()
    {
        return Ok();
    }

    [HttpPut]
    [Route("User-Info")]
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