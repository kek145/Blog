﻿using System;
using System.Linq;
using BlogAPI.Domain.Enum;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Response;
using BlogAPI.Domain.Entity.Table;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BlogAPI.BL.DTOs.RegistrationDto;
using BlogAPI.Security.HashDataHelper;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.BL.RegistrationService;

public class RegistrationService : IRegistrationService
{
    private readonly ILogger<RegistrationService> _logger;
    private readonly IBaseRepository<RoleEntity> _roleRepository;
    private readonly IBaseRepository<UserEntity> _userRepository;
    private readonly IBaseRepository<UserRoleEntity> _userRoleRepository;

    public RegistrationService(ILogger<RegistrationService> logger,
        IBaseRepository<RoleEntity> roleRepository,
        IBaseRepository<UserEntity> userRepository,
        IBaseRepository<UserRoleEntity> userRoleRepository)
    {
        _logger = logger;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
    }


    public async Task<IBaseResponse<UserEntity>> RegistrationServiceAsync(RegistrationDto registrationDto)
    {
        try
        {
            var user = await _userRepository.GetAll()
                .Where(find => find.Email == registrationDto.Email)
                .FirstOrDefaultAsync();
            
            var role = await _roleRepository.GetAll()
                .Where(find => find.RoleName == registrationDto.Role)
                .FirstOrDefaultAsync();

            if (user != null!)
            {
                _logger.LogError("User with this email is already registered!");
                return new BaseResponse<UserEntity>().ServerResponse("User with this email is already registered!", StatusCode.BadRequest);
            }

            if (role == null!)
            {
                _logger.LogError("This type of account cannot be created!");
                return new BaseResponse<UserEntity>().ServerResponse("This type of account cannot be created!", StatusCode.BadRequest);
            }

            if (registrationDto.Password != registrationDto.ConfirmPassword)
            {
                _logger.LogError("Password mismatch!");
                return new BaseResponse<UserEntity>().ServerResponse("Password mismatch!", StatusCode.BadRequest);
            }

            PasswordHasher.CreatePasswordHash(registrationDto.Password, out var passwordHash, out var passwordSalt);
            var userEntity = new UserEntity
            {
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                Email = registrationDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            var userRoleEntity = new UserRoleEntity
            {
                User = userEntity,
                Role = role
            };

            await _userRepository.AddAsync(userEntity);
            await _userRoleRepository.AddAsync(userRoleEntity);
            
            _logger.LogInformation("Registration completed successfully!");
            return new BaseResponse<UserEntity>().ServerResponse("Registration completed successfully!", StatusCode.Ok);
        }
        catch (Exception)
        {
            _logger.LogError("Some problems with the repositories or the service itself!");
            return new BaseResponse<UserEntity>().ServerResponse("Some problems with the repositories or the service itself!", StatusCode.InternalServerError);
        }
    }
}