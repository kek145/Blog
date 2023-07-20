using System;
using System.Linq;
using BlogAPI.Domain.Enum;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.BL.JwtTokenService;
using BlogAPI.DAL.UserRepository;
using System.Collections.Generic;
using BlogAPI.BL.DTOs.EditUserDto;
using BlogAPI.Domain.Entity.Table;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BlogAPI.DAL.ArticleRepository;
using BlogAPI.Security.HashDataHelper;
using BlogAPI.BL.DTOs.AuthenticationDto;

namespace BlogAPI.BL.AccountService;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AccountService> _logger;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IArticleRepository _articleRepository;

    public AccountService(IUserRepository userRepository, ILogger<AccountService> logger, IJwtTokenService jwtTokenService, IArticleRepository articleRepository)
    {
        _userRepository = userRepository;
        _logger = logger;
        _jwtTokenService = jwtTokenService;
        _articleRepository = articleRepository;
    }


    public async Task<IBaseResponse<UserEntity>> DeleteUserAccountAsync(string token)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var user = await _userRepository.FindUserByIdAsync(userId!.Value);

            var article = await _articleRepository.GetAllArticles()
                .Where(x => x.UserArticle
                    .Any(userArticles => user.UserId == userId)).FirstOrDefaultAsync();

            await _articleRepository.DeleteArticleAsync(article!);
            
            if (user == null!)
            {
                _logger.LogError("User is not found!");
                return new BaseResponse<UserEntity>().ServerResponse("User is not found!", StatusCode.BadRequest);
            }

            await _userRepository.DeleteUserAsync(user);
            return new BaseResponse<UserEntity>().ServerResponse("Account successfully deleted!", StatusCode.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<UserEntity>().ServerResponse("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<UpdateUserDto>> UpdateUserInfoAsync(UpdateUserDto updateDto, string token)
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
                return new BaseResponse<UpdateUserDto>().ServerResponse("User is not found!", StatusCode.BadRequest);
            }

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;

            await _userRepository.UpdateUserAsync(user);
            
            _logger.LogInformation("Update of this user was successful!");
            return new BaseResponse<UpdateUserDto>().ServerResponse("Update of this user was successful!", StatusCode.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<UpdateUserDto>().ServerResponse("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<UpdateAuthenticationDto>> UpdateAuthenticationInfoAsync(UpdateAuthenticationDto updateDto, string token)
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
                return new BaseResponse<UpdateAuthenticationDto>().ServerResponse("User is not found!", StatusCode.BadRequest);
            }

            if (updateDto.Password != updateDto.ConfirmPassword)
            {
                _logger.LogError("Password mismatch!");
                return new BaseResponse<UpdateAuthenticationDto>().ServerResponse("Password mismatch!", StatusCode.BadRequest);
            }
            
            PasswordHasher.CreatePasswordHash(updateDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Email = updateDto.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.UpdateUserAsync(user);
            
            _logger.LogInformation("Login details updated successfully!");
            return new BaseResponse<UpdateAuthenticationDto>().ServerResponse("Login details updated successfully!", StatusCode.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<UpdateAuthenticationDto>().ServerResponse("Internal server error", StatusCode.InternalServerError);
        }
    }
}