using BlogAPI.BL.DTOs;
using System.Threading.Tasks;

namespace BlogAPI.BL.AuthenticationService;

public interface IAuthenticationService
{
    Task<string> AuthenticationAsync(AuthenticationDto authenticationDto);
}