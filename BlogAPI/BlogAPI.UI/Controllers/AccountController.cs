﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.BL.DTOs.EditUserDto;
using BlogAPI.BL.EditAccountService;
using BlogAPI.BL.DTOs.AuthenticationDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlogAPI.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Author, User")]
public class AccountController : ControllerBase
{
    private readonly IEditAccountService _editAccountService;

    public AccountController(IEditAccountService editAccountService)
    {
        _editAccountService = editAccountService;
    }

    [HttpPut]
    [Route("User-Info")]
    public async Task<IActionResult> EditUserInfo([FromBody] EditUserDto request)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _editAccountService.UpdateUserInfoAsync(request, token);

        if (response.StatusCode == Domain.Enum.StatusCode.BadRequest)
            return BadRequest(new { error = response.Description });

        if (response.StatusCode == Domain.Enum.StatusCode.InternalServerError)
            return StatusCode(500, new { error = response.Description });


        return Ok(new { response.Description });
    }
    
    [HttpPut]
    [Route("Auth-Info")]
    public async Task<IActionResult> EditAuthenticationInfo([FromBody] EditAuthenticationDto request)
    {
        return Ok();
    }
}