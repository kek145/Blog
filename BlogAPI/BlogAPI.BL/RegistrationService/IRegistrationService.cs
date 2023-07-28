using System.Threading.Tasks;
using BlogAPI.DAL.DTOs.RegistrationDto;
using BlogAPI.Domain.Response;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.BL.RegistrationService;

public interface IRegistrationService
{
    Task<IBaseResponse<UserEntity>> RegistrationServiceAsync(RegistrationDto registrationDto);
}