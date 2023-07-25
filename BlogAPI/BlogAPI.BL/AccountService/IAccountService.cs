using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.BL.DTOs.EditUserDto;
using BlogAPI.Domain.Entity.Table;
using BlogAPI.BL.DTOs.AuthenticationDto;

namespace BlogAPI.BL.AccountService;

public interface IAccountService
{
    Task<IBaseResponse<UserEntity>> DeleteUserAccountAsync(string token);
    Task<IBaseResponse<UpdateUserDto>> UpdateUserInfoAsync(UpdateUserDto updateDto, string token);
    Task<IBaseResponse<UpdateAuthenticationDto>> UpdateAuthenticationInfoAsync(UpdateAuthenticationDto updateDto, string token);
}