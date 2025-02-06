using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;

namespace UserApi.Core.Interfaces
{
    public interface ICardRepository
    {
        Task<bool> CheckCardPrefixAsync(string prefix);
        Task AddAsync(CardDetail cardDetail); // اضافه کردن متد افزودن کارت
        Task SaveChangesAsync(); // اضافه کردن متد ذخیره تغییرات
     
    }
}
