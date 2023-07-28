using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace BlogAPI.BL.JwtTokenService;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public int? GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _configuration.GetSection("JWTConfiguration:Issuer").Value,
            ValidateAudience = true,
            ValidAudience = _configuration.GetSection("JWTConfiguration:Audience").Value,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTConfiguration:SecretKey").Value!)),
            ValidateIssuerSigningKey = true
        };

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);
        
            var userIdClaim = claimsPrincipal.FindFirst("userid");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            throw new InvalidOperationException("Invalid user ID claim");
        }
        catch (Exception)
        {
            _logger.LogError("Token validation failed");
        }

        return null;
    }
}