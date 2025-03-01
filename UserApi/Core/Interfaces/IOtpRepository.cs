using System.Threading.Tasks;
using UserApi.Core.Models;

namespace UserApi.Core.Interfaces
{
    public interface IOtpRepository
    {
        Task<Otp> GetOtpAsync(string userId, string otp);
        Task SaveOtpAsync(Otp otp);
        Task MarkOtpAsUsedAsync(Otp otp);
    }
}
