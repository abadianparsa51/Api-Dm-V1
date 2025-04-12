using System.Collections.Generic;
using System.Threading.Tasks;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

namespace UserApi.Core.Interfaces
{
    public interface ICardManagementService
    {
        Task<bool> CheckCardPrefixAsync(string prefix);
        Task<Bank> GetBankByPrefixAsync(string prefix);
        Task<IEnumerable<CardDetail>> GetUserCardsAsync(string userId);
        Task<CardDetail> GetCardByIdAsync(int id, string userId);
        Task<bool> CardExistsAsync(string cardNumber, string userId);
        Task<ServiceResponse<CardDetailDTO>> AddCardAsync(string userId, CardDetailDTO cardDetailDto);

        Task<bool> DeleteCardAsync(int cardId, string userId);
        Task<bool> UpdateCardAsync(int cardId, string userId, CardDetailDTO cardDetailDto); // اضافه شده
        Task CheckCardPrefixAsync(CheckCardPrefixRequest request);
        Task<IEnumerable<CardDetail>> GetCardsByUserIdAsync(string userId);
    }
}
