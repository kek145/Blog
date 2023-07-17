using System;
using System.Net;
using BlogAPI.BL.DTOs;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.DAL.RoleRepository;
using BlogAPI.DAL.UserRepository;
using BlogAPI.Domain.Entity.Table;
using BlogAPI.DAL.UserRoleRepository;
using BlogAPI.Domain.Entity.Connection;
using BlogAPI.Domain.Enum;
using BlogAPI.Security.HashDataHelper;
using Microsoft.Extensions.Logging;

namespace BlogAPI.BL.RegistrationService;

public class RegistrationService : IRegistrationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<RegistrationService> _logger;
    private readonly IUserRoleRepository _userRoleRepository;

    public RegistrationService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, ILogger<RegistrationService> logger)
    {
        _logger = logger;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<IBaseResponse<UserEntity>> RegistrationServiceAsync(RegistrationDto registrationDto)
    {
        try
        {
            var user = await _userRepository.FindUserByEmailAsync(registrationDto.Email);
            var role = await _roleRepository.GetRoleByNameAsync(registrationDto.RoleName);

            if (user != null! || role == null!)
            {
                _logger.LogError("User with this email is already registered!");
                return new BaseResponse<UserEntity>
                {
                    Description = "User with this email is already registered!",
                    StatusCode = StatusCode.BadRequest
                };
            }
            
            PasswordHasher.CreatePasswordHash(registrationDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
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

            await _userRepository.AddNewUserAsync(userEntity);
            await _userRoleRepository.AddNewUserRoleAsync(userRoleEntity);
            
            _logger.LogInformation("Registration completed successfully");
            return new BaseResponse<UserEntity>
            {
                Description = "Registration completed successfully",
                StatusCode = StatusCode.Ok,
            };
        }
        catch (Exception)
        {
            _logger.LogError("Some problems with the repositories or the service itself");
            return new BaseResponse<UserEntity>
            {
                Description = "Internal server error",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
}