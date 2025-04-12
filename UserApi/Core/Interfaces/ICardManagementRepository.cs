using UserApi.Core.Models;
using UserApi.Services;

namespace UserApi.Core.Interfaces
{
    public interface ICardManagementRepository
    {
        Task<bool> CheckCardPrefixAsync(string prefix);
        Task<Bank> GetBankByPrefixAsync(string prefix);
        Task<IEnumerable<CardDetail>> GetUserCardsAsync(string userId);
        Task<CardDetail> GetCardByIdAsync(int id, string userId);
        Task<bool> CardExistsAsync(string cardNumber, string userId);
        Task<ServiceResponse<CardDetail>> AddCardAsync(CardDetail cardDetail);  // Changed return type
        Task<ServiceResponse<bool>> DeleteCardAsync(CardDetail cardDetail); // Changed return type to ServiceResponse<bool>
        Task<ServiceResponse<bool>> UpdateCardAsync(CardDetail cardDetail); // Changed return type to ServiceResponse<bool>
        Task<IEnumerable<CardDetail>> GetCardsByUserIdAsync(string userId);
    }
}
