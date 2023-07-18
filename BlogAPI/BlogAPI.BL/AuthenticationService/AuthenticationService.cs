using System;
using System.Text;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogAPI.DAL.RoleRepository;
using BlogAPI.DAL.UserRepository;
using BlogAPI.Domain.Entity.Table;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BlogAPI.Security.HashDataHelper;
using BlogAPI.BL.DTOs.AuthenticationDto;
using Microsoft.Extensions.Configuration;

namespace BlogAPI.BL.AuthenticationService;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(IConfiguration configuration, IUserRepository userRepository, IRoleRepository roleRepository, ILogger<AuthenticationService> logger)
    {
        _logger = logger;
        _configuration = configuration;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<string> AuthenticationAsync(AuthenticationDto authenticationDto)
    {
        var user = await _userRepository.FindUserByEmailAsync(authenticationDto.Email);
        var role = await _roleRepository.FindRolesByUserIdAsync(user.UserId);

        if (user == null! || role == null!)
        {
            _logger.LogError("No user found with this email or role!");
            return null!;
        }

        if (!PasswordHasher.VerifyPasswordHash(authenticationDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            _logger.LogError("Incorrect password!");
            return null!;
        }

        var token = GenerateJwtToken(user, role);
        return token;
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