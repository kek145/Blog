using System;
using BlogAPI.Domain.Enum;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.BL.JwtTokenService;
using BlogAPI.DAL.UserRepository;
using System.Collections.Generic;
using BlogAPI.BL.DTOs.EditUserDto;
using Microsoft.Extensions.Logging;
using BlogAPI.Security.HashDataHelper;
using BlogAPI.BL.DTOs.AuthenticationDto;

namespace BlogAPI.BL.AccountService;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<AccountService> _logger;

    public AccountService(IUserRepository userRepository, IJwtTokenService jwtTokenService, ILogger<AccountService> logger)
    {
        _logger = logger;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }


    public async Task<IBaseResponse<IEnumerable<UpdateUserDto>>> UpdateUserInfoAsync(UpdateUserDto updateDto, string token)
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
                return new BaseResponse<UpdateUserDto>().ServerResponseEnumerable("User is not found!", StatusCode.BadRequest);
            }

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;

            await _userRepository.UpdateUserAsync(user);
            
            _logger.LogInformation("Update of this user was successful!");
            return new BaseResponse<UpdateUserDto>().ServerResponseEnumerable("Update of this user was successful!", StatusCode.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<UpdateUserDto>().ServerResponseEnumerable("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<IEnumerable<UpdateAuthenticationDto>>> UpdateAuthenticationInfoAsync(UpdateAuthenticationDto updateDto, string token)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var user = await _userRepository.FindUserByIdAsync(userId!.Value);
            
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not authorized to update this article!");
            
            if (user == null!)
            {
                _logger.LogError("User is not found!");
                return new BaseResponse<UpdateAuthenticationDto>().ServerResponseEnumerable("User is not found!", StatusCode.BadRequest);
            }

            if (updateDto.Password != updateDto.ConfirmPassword)
            {
                _logger.LogError("Password mismatch!");
                return new BaseResponse<UpdateAuthenticationDto>().ServerResponseEnumerable("Password mismatch!", StatusCode.BadRequest);
            }
            
            PasswordHasher.CreatePasswordHash(updateDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Email = updateDto.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.UpdateUserAsync(user);
            
            _logger.LogInformation("Login details updated successfully!");
            return new BaseResponse<UpdateAuthenticationDto>().ServerResponseEnumerable("Login details updated successfully!", StatusCode.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<UpdateAuthenticationDto>().ServerResponseEnumerable("Internal server error", StatusCode.InternalServerError);
        }
    }
}