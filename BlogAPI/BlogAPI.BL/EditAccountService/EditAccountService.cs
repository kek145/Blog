using System;
using BlogAPI.Domain.Enum;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.BL.JwtTokenService;
using BlogAPI.DAL.UserRepository;
using System.Collections.Generic;
using BlogAPI.BL.DTOs.EditUserDto;
using Microsoft.Extensions.Logging;
using BlogAPI.BL.DTOs.AuthenticationDto;

namespace BlogAPI.BL.EditAccountService;

public class EditAccountService : IEditAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<EditAccountService> _logger;

    public EditAccountService(IUserRepository userRepository, IJwtTokenService jwtTokenService, ILogger<EditAccountService> logger)
    {
        _logger = logger;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }


    public async Task<IBaseResponse<IEnumerable<EditUserDto>>> UpdateUserInfoAsync(EditUserDto editDto, string token)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var user = await _userRepository.FindUserByIdAsync(userId!.Value);
            
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not authorized to update this article");

            if (user == null!)
            {
                _logger.LogError("User is not found!");
                return new BaseResponse<EditUserDto>().ServerResponseEnumerable("User is not found!", StatusCode.BadRequest);
            }

            user.FirstName = editDto.FirstName;
            user.LastName = editDto.LastName;

            await _userRepository.UpdateUserAsync(user);
            
            return new BaseResponse<EditUserDto>().ServerResponseEnumerable("Update of this user was successful", StatusCode.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<EditUserDto>().ServerResponseEnumerable("Internal server error", StatusCode.InternalServerError);
        }
    }

    public Task<IBaseResponse<IEnumerable<EditAuthenticationDto>>> UpdateAuthenticationInfoAsync(EditAuthenticationDto editDto, string token)
    {
        throw new System.NotImplementedException();
    }
}