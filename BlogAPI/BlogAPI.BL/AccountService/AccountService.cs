﻿using System;
using System.Linq;
using BlogAPI.Domain.Enum;
using BlogAPI.DAL.Interfaces;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.BL.JwtTokenService;
using BlogAPI.BL.DTOs.EditUserDto;
using BlogAPI.Domain.Entity.Table;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Security.HashDataHelper;
using BlogAPI.BL.DTOs.AuthenticationDto;

namespace BlogAPI.BL.AccountService;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IBaseRepository<UserEntity> _userRepository;
    private readonly IBaseRepository<CommentEntity> _commentRepository;
    private readonly IBaseRepository<ArticleEntity> _articleRepository;

    public AccountService(ILogger<AccountService> logger,
        IJwtTokenService jwtTokenService,
        IBaseRepository<UserEntity> userRepository, 
        IBaseRepository<CommentEntity> commentRepository,
        IBaseRepository<ArticleEntity> articleRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _commentRepository = commentRepository;
        _articleRepository = articleRepository;
    }


    public async Task<IBaseResponse<UserEntity>> DeleteUserAccountAsync(string token)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            if (!userId.HasValue)
            {
                _logger.LogError("User is not authorized");
                return new BaseResponse<UserEntity>().ServerResponse("User is not authorized", StatusCode.Unauthorized);
            }
            
            var user = await _userRepository.GetAll()
                .Where(find => find.UserId == userId.Value)
                .FirstOrDefaultAsync();
            
            var comments = await _commentRepository.GetAll()
                .Where(find => find.UserComment.Any(commentsArticle => commentsArticle.UserId == userId.Value))
                .FirstOrDefaultAsync();

            var articles = await _articleRepository.GetAll()
                .Where(find => find.UserArticle.Any(userArticle => userArticle.UserId == userId.Value))
                .FirstOrDefaultAsync();

            if (user == null!)
            {
                _logger.LogError("User is not found!");
                return new BaseResponse<UserEntity>().ServerResponse("User is not found!", StatusCode.NotFound);
            }

            if (comments != null)
            {
                await _commentRepository.DeleteAsync(comments);
            }

            if (articles != null)
            {
                await _articleRepository.DeleteAsync(articles);
            }
            
            await _userRepository.DeleteAsync(user);
            return new BaseResponse<UserEntity>().ServerResponse("Account successfully deleted!", StatusCode.NoContent);
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
            
            if (!userId.HasValue)
            {
                _logger.LogError("User is not authorized");
                return new BaseResponse<UpdateUserDto>().ServerResponse("User is not authorized", StatusCode.Unauthorized);
            }
            
            var user = await _userRepository.GetAll()
                .Where(find => find.UserId == userId.Value)
                .FirstOrDefaultAsync();

            if (user == null!)
            {
                _logger.LogError("User is not found!");
                return new BaseResponse<UpdateUserDto>().ServerResponse("User is not found!", StatusCode.NotFound);
            }

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;

            await _userRepository.UpdateAsync(user);
            
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

            if (!userId.HasValue)
            {
                _logger.LogError("User is not authorized");
                return new BaseResponse<UpdateAuthenticationDto>().ServerResponse("User is not authorized", StatusCode.Unauthorized);
            }
            
            var user = await _userRepository.GetAll()
                .Where(find => find.UserId == userId.Value)
                .FirstOrDefaultAsync();
            
            if (user == null!)
            {
                _logger.LogError("User is not found!");
                return new BaseResponse<UpdateAuthenticationDto>().ServerResponse("User is not found!", StatusCode.NotFound);
            }

            if (updateDto.Password != updateDto.ConfirmPassword)
            {
                _logger.LogError("Password mismatch!");
                return new BaseResponse<UpdateAuthenticationDto>().ServerResponse("Password mismatch!", StatusCode.Conflict);
            }
            
            PasswordHasher.CreatePasswordHash(updateDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Email = updateDto.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.UpdateAsync(user);
            
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