using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.Domain.Entity.Table;
using BlogAPI.BL.DTOs.RegistrationDto;

namespace BlogAPI.BL.RegistrationService;

public interface IRegistrationService
{
    Task<IBaseResponse<UserEntity>> RegistrationServiceAsync(RegistrationDto registrationDto);
}