using System.Threading.Tasks;
using BlogAPI.BL.DTOs.AuthenticationDto;
using BlogAPI.Domain.Response;

namespace BlogAPI.BL.AuthenticationService;

public interface IAuthenticationService
{
    Task<IBaseResponse<string>> AuthenticationAsync(AuthenticationDto authenticationDto);
}