namespace BlogAPI.BL.JwtTokenService;

public interface IJwtTokenService
{
    int? GetUserIdFromToken(string token);
}