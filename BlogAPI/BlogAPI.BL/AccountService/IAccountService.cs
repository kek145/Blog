using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using System.Collections.Generic;
using BlogAPI.BL.DTOs.EditUserDto;
using BlogAPI.BL.DTOs.AuthenticationDto;

namespace BlogAPI.BL.AccountService;

public interface IAccountService
{
    Task<IBaseResponse<IEnumerable<UpdateUserDto>>> UpdateUserInfoAsync(UpdateUserDto updateDto, string token);
    Task<IBaseResponse<IEnumerable<UpdateAuthenticationDto>>> UpdateAuthenticationInfoAsync(UpdateAuthenticationDto updateDto, string token);
}