using Azure;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApi.Core.Interfaces;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

namespace UserApi.Services
{
    public class CardManagementService : ICardManagementService
    {
        private readonly ICardManagementRepository _repository;

        public CardManagementService(ICardManagementRepository repository)
        {
            _repository = repository;
        }

        // پیاده‌سازی متد CheckCardPrefixAsync برای درخواست جدید
        public async Task CheckCardPrefixAsync(CheckCardPrefixRequest request)
        {
            var result = await _repository.CheckCardPrefixAsync(request.Prefix);
            if (!result)
            {
                // اعمال منطق مناسب در صورت عدم وجود پیشوند
                throw new KeyNotFoundException("Card prefix not found.");
            }
        }

        public Task<bool> CheckCardPrefixAsync(string prefix)
        {
            return _repository.CheckCardPrefixAsync(prefix);
        }

        public Task<Bank> GetBankByPrefixAsync(string prefix)
        {
            return _repository.GetBankByPrefixAsync(prefix);
        }

        public Task<IEnumerable<CardDetail>> GetUserCardsAsync(string userId)
        {
            return _repository.GetUserCardsAsync(userId);
        }

        public Task<CardDetail> GetCardByIdAsync(int id, string userId)
        {
            return _repository.GetCardByIdAsync(id, userId);
        }

        public Task<bool> CardExistsAsync(string cardNumber, string userId)
        {
            return _repository.CardExistsAsync(cardNumber, userId);
        }

        public async Task<ServiceResponse<CardDetailDTO>> AddCardAsync(string userId,CardDetailDTO cardDetailDto)
        {
            var response = new ServiceResponse<CardDetailDTO>();

            try
            {
                // تبدیل DTO به مدل دیتابیس
                var cardDetail = new CardDetail
                {
                    UserId = userId,
                    CardNumber = cardDetailDto.CardNumber,
                    ExpirationDate = cardDetailDto.ExpirationDate,
                    CVV2 = cardDetailDto.CVV2,
                };

                // اضافه کردن کارت به دیتابیس
                var result = await _repository.AddCardAsync(cardDetail);

                if (result.Success)  // فرض می‌کنیم که متد AddCardAsync مقدار bool برمی‌گرداند
                {
                    response.Success = true;
                    response.Data = cardDetailDto;
                    response.Message = "Card successfully added";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to add card. Please try again.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while adding the card.";
                response.ErrorMessage = ex.Message;  // لاگ کردن جزئیات خطا
            }

            return response;
        }


        public async Task<bool> DeleteCardAsync(int cardId, string userId)
        {
            // پیدا کردن کارت و حذف آن
            var card = await _repository.GetCardByIdAsync(cardId, userId);
            if (card != null)
            {
                await _repository.DeleteCardAsync(card);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateCardAsync(int cardId, string userId, CardDetailDTO cardDetailDto)
        {
            // تبدیل CardDetailDTO به CardDetail
            var cardDetail = new CardDetail
            {
                Id = cardId,
                UserId = userId,
                CardNumber = cardDetailDto.CardNumber,
                ExpirationDate = cardDetailDto.ExpirationDate,
                CVV2 = cardDetailDto.CVV2,
                //BankId = cardDetailDto.BankId
            };

            // استفاده از repository برای به‌روزرسانی کارت
            var response = await _repository.UpdateCardAsync(cardDetail);

            // بازگشت مقدار Data از ServiceResponse<bool>
            return response.Data;
        }


        public Task<IEnumerable<CardDetail>> GetCardsByUserIdAsync(string userId)
        {
            return _repository.GetCardsByUserIdAsync(userId);
        }
    }
}
