using System.Threading.Tasks;
using BlogAPI.BL.DTOs.AuthenticationDto;

namespace BlogAPI.BL.AuthenticationService;

public interface IAuthenticationService
{
    Task<string> AuthenticationAsync(AuthenticationDto authenticationDto);
}