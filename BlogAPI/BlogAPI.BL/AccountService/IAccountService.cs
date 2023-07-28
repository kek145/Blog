using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.DAL.DTOs.AuthenticationDto;
using BlogAPI.DAL.DTOs.EditUserDto;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.BL.AccountService;

public interface IAccountService
{
    Task<IBaseResponse<UserEntity>> DeleteUserAccountAsync(string token);
    Task<IBaseResponse<UpdateUserDto>> UpdateUserInfoAsync(UpdateUserDto updateDto, string token);
    Task<IBaseResponse<UpdateAuthenticationDto>> UpdateAuthenticationInfoAsync(UpdateAuthenticationDto updateDto, string token);
}