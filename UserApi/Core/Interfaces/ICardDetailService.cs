using System.Collections.Generic;
using System.Threading.Tasks;
using UserApi.Core.Models.DTOs;
using UserApi.Core.Models;
using UserApi.Services;

namespace UserApi.Core.Interfaces
{
    public interface ICardDetailService
    {
        Task<ServiceResponse<CardDetailDTO>> AddCardAsync(string userId, CardDetailDTO cardDetailDto);
        Task<ServiceResponse<IEnumerable<CardDetailDTO>>> GetUserCardsAsync(string userId);
        Task<ServiceResponse<bool>> DeleteCardAsync(string userId, int cardId);
        Task<ServiceResponse<CardDetailDTO>> UpdateCardAsync(string userId, int cardId, CardDetailDTO updatedCardDto);
    }
}
