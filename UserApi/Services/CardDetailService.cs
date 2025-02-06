using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApi.Core.Interfaces;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;
using UserApi.Data;

namespace UserApi.Services
{
    public class CardDetailService : ICardDetailService
    {
        private readonly ApiDbContext _context;

        public CardDetailService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<CardDetailDTO>> AddCardAsync(string userId, CardDetailDTO cardDetailDto)
        {
            var response = new ServiceResponse<CardDetailDTO>();

            if (string.IsNullOrEmpty(cardDetailDto.CardNumber) || cardDetailDto.CardNumber.Length < 6)
            {
                response.Message = "Card number must be at least 6 digits.";
                return response;
            }

            var prefix = cardDetailDto.CardNumber.Substring(0, 6);
            var bankInfo = await _context.CardPrefixes
                .Where(cp => cp.Prefix == prefix)
                .Select(cp => new { cp.BankId, cp.BankName })
                .FirstOrDefaultAsync();

            if (bankInfo == null)
            {
                response.Message = "Invalid card prefix.";
                return response;
            }

            var isDuplicate = await _context.CardDetails
                .AnyAsync(c => c.CardNumber == cardDetailDto.CardNumber && c.UserId == userId);

            if (isDuplicate)
            {
                response.Message = "This card is already added.";
                return response;
            }

            var card = new CardDetail
            {
                CardNumber = cardDetailDto.CardNumber,
                ExpirationDate = cardDetailDto.ExpirationDate,
                CVV2 = cardDetailDto.CVV2,
                BankId = bankInfo.BankId,
                BankName = bankInfo.BankName,
                UserId = userId
            };

            await _context.CardDetails.AddAsync(card);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Card added successfully.";
            response.Data = cardDetailDto;
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<CardDetailDTO>>> GetUserCardsAsync(string userId)
        {
            var userCards = await _context.CardDetails
                .Where(c => c.UserId == userId)
                .Select(c => new CardDetailDTO
                {
                    CardNumber = c.CardNumber,
                    ExpirationDate = c.ExpirationDate,
                CVV2 = c.CVV2,
                })
                .ToListAsync();

            return new ServiceResponse<IEnumerable<CardDetailDTO>>
            {
                Success = true,
                Data = userCards
            };
        }

        public async Task<ServiceResponse<bool>> DeleteCardAsync(string userId, int cardId)
        {
            var card = await _context.CardDetails
                .FirstOrDefaultAsync(c => c.Id == cardId && c.UserId == userId);

            if (card == null)
                return new ServiceResponse<bool> { Message = "Card not found." };

            _context.CardDetails.Remove(card);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Success = true,
                Message = "Card deleted successfully.",
                Data = true
            };
        }

        public async Task<ServiceResponse<CardDetailDTO>> UpdateCardAsync(string userId, int cardId, CardDetailDTO updatedCardDto)
        {
            var card = await _context.CardDetails.FindAsync(cardId);

            if (card == null || card.UserId != userId)
                return new ServiceResponse<CardDetailDTO> { Message = "Card not found." };

            card.CardNumber = updatedCardDto.CardNumber;
            card.ExpirationDate = updatedCardDto.ExpirationDate;

            _context.CardDetails.Update(card);
            await _context.SaveChangesAsync();

            return new ServiceResponse<CardDetailDTO>
            {
                Success = true,
                Message = "Card updated successfully.",
                Data = updatedCardDto
            };
        }
    }
}
