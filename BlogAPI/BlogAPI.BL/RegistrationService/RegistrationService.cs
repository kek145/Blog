using System;
using BlogAPI.BL.DTOs;
using BlogAPI.Domain.Enum;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.DAL.RoleRepository;
using BlogAPI.DAL.UserRepository;
using BlogAPI.Domain.Entity.Table;
using Microsoft.Extensions.Logging;
using BlogAPI.DAL.UserRoleRepository;
using BlogAPI.Security.HashDataHelper;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.BL.RegistrationService;

public class RegistrationService : IRegistrationService
{
    private BaseResponse<UserEntity> _response;
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

            if (user != null!)
            {
                _logger.LogError("User with this email is already registered!");
                return new BaseResponse<UserEntity>().BadRequestResponse("User with this email is already registered!");
            }

            if (role == null!)
            {
                _logger.LogError("This type of account cannot be created!");
                return new BaseResponse<UserEntity>().BadRequestResponse("This type of account cannot be created!");
            }

            if (registrationDto.Password != registrationDto.ConfirmPassword)
            {
                _logger.LogError("Password mismatch!");
                return new BaseResponse<UserEntity>().BadRequestResponse("Password mismatch!");
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
            
            _logger.LogInformation("Registration completed successfully!");
            return new BaseResponse<UserEntity>().SuccessRequest("Registration completed successfully!");
        }
        catch (Exception)
        {
            _logger.LogError("Some problems with the repositories or the service itself!");
            return new BaseResponse<UserEntity>().InternalServerErrorResponse("Some problems with the repositories or the service itself!");
        }
    }
}