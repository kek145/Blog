using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using System.Collections.Generic;
using BlogAPI.BL.DTOs.EditUserDto;
using BlogAPI.BL.DTOs.AuthenticationDto;

namespace BlogAPI.BL.EditAccountService;

public interface IEditAccountService
{
    Task<IBaseResponse<IEnumerable<EditUserDto>>> UpdateUserInfoAsync(EditUserDto editDto, string token);
    Task<IBaseResponse<IEnumerable<EditAuthenticationDto>>> UpdateAuthenticationInfoAsync(EditAuthenticationDto editDto, string token);
}