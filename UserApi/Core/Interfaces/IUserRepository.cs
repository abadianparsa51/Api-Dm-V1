using UserApi.Core.Models;

namespace UserApi.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<ApplicationUser> GetUserByPhoneNumberAsync(string phoneNumber);  // متد جدید برای پیدا کردن کاربر با شماره تلفن
        Task<bool> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    }
}
