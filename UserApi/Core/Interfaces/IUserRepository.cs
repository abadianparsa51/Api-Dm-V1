using UserApi.Core.Models;

public interface IUserRepository
{
    Task<ApplicationUser> GetUserByEmailAsync(string email);
    Task<ApplicationUser> GetUserByPhoneAsync(string phoneNumber);
    Task<bool> CreateUserAsync(ApplicationUser user, string password);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}
