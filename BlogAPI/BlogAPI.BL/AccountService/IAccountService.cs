using System.Threading.Tasks;
using BlogAPI.DAL.DTOs.AccountDTOs;
using BlogAPI.Domain.Response;
using BlogAPI.Domain.Entity.Table;
using BlogAPI.DAL.DTOs.EditUserDto;
using BlogAPI.DAL.DTOs.AuthenticationDto;

namespace BlogAPI.BL.AccountService;

public interface IAccountService
{
    Task<IBaseResponse<UserDto>> GetUserInfo(string token);
    Task<IBaseResponse<UserEntity>> DeleteUserAccountAsync(string token);
    Task<IBaseResponse<UpdateUserDto>> UpdateUserInfoAsync(UpdateUserDto updateDto, string token);
    Task<IBaseResponse<UpdateAuthenticationDto>> UpdateAuthenticationInfoAsync(UpdateAuthenticationDto updateDto, string token);
}