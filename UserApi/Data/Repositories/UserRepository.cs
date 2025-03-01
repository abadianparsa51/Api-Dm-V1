using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserApi.Core.Models;
using UserApi.Data;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly ApiDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(ApiDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ApplicationUser> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<ApplicationUser> GetUserByPhoneAsync(string phoneNumber)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
    {
        // استفاده از UserManager برای ایجاد کاربر با رمز عبور هش شده
        var result = await _userManager.CreateAsync(user, password);

        return result.Succeeded;
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        // بررسی رمز عبور با UserManager
        return await _userManager.CheckPasswordAsync(user, password);
    }
}
