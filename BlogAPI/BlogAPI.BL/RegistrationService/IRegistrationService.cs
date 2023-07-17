using BlogAPI.BL.DTOs;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.BL.RegistrationService;

public interface IRegistrationService
{
    Task<IBaseResponse<UserEntity>> RegistrationServiceAsync(RegistrationDto registrationDto);
}