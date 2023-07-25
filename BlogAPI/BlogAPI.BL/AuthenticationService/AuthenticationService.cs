using System;
using System.Text;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Azure;
using BlogAPI.Security.HashDataHelper;
using BlogAPI.BL.DTOs.AuthenticationDto;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Connection;
using BlogAPI.Domain.Enum;
using BlogAPI.Domain.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlogAPI.BL.AuthenticationService;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IGenericRepository<UserEntity> _userRepository;
    private readonly IGenericRepository<UserRoleEntity> _userRoleRepository;

    public AuthenticationService(IConfiguration configuration,
        ILogger<AuthenticationService> logger,
        IGenericRepository<UserEntity> userRepository,
        IGenericRepository<UserRoleEntity> userRoleRepository)
    {
        _logger = logger;
        _configuration = configuration;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
    }


    public async Task<IBaseResponse<string>> AuthenticationAsync(AuthenticationDto authenticationDto)
    {
        try
        {
            var user = await _userRepository.GetAll()
                .Where(find => find.Email == authenticationDto.Email)
                .FirstOrDefaultAsync();
            var role = await _userRoleRepository.GetAll()
                .Where(find => find.UserId == user!.UserId)
                .Select(ur => ur.Role.RoleName)
                .FirstOrDefaultAsync();
        

            if (role == null)
            {
                _logger.LogError("");
                return new BaseResponse<string>().ServerResponse("User with this role not found", StatusCode.Unauthorized);
            }

            if (user == null! || !PasswordHasher.VerifyPasswordHash(authenticationDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                _logger.LogError("Invalid email or password");
                return new BaseResponse<string>().ServerResponse("Invalid email or password!", StatusCode.Unauthorized);
            }
        
            var token = GenerateJwtToken(user, role);

            return new BaseResponse<string>
            {
                Data = token,
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<string>().ServerResponse("Internal server error!", StatusCode.InternalServerError);
        }
    }

    private string GenerateJwtToken(UserEntity user, string role)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTConfiguration:SecretKey").Value!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new []
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture)),
            }),
            Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JWTConfiguration:ExpirationTime").Value!)),
            Issuer = _configuration.GetSection("JWTConfiguration:Issuer").Value,
            Audience = _configuration.GetSection("JWTConfiguration:Audience").Value,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        return jwtTokenHandler.WriteToken(token);
    }
}