using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.Domain.Entity.Table;
using BlogAPI.Domain.DTOs.AccountDTOs;
using BlogAPI.Domain.DTOs.AuthenticationDto;
using BlogAPI.Domain.DTOs.EditUserDto;

namespace BlogAPI.BL.AccountService;

public interface IAccountService
{
    Task<IBaseResponse<UserDto>> GetUserInfo(string token);
    Task<IBaseResponse<UserEntity>> DeleteUserAccountAsync(string token);
    Task<IBaseResponse<UpdateUserDto>> UpdateUserInfoAsync(UpdateUserDto updateDto, string token);
    Task<IBaseResponse<UpdateAuthenticationDto>> UpdateAuthenticationInfoAsync(UpdateAuthenticationDto updateDto, string token);
}