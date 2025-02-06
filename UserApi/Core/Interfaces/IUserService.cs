using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;

namespace UserApi.Core.Interfaces
{
    public interface IUserService
    {
        Task<AuthResult> RegisterUserAsync(UserRegistrationRequestDto userDto);
        Task<AuthResult> LoginUserAsync(UserLoginRequestDto loginDto);
    }
}
