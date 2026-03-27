using Microsoft.EntityFrameworkCore;
using UserApi.Core.Interfaces;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;
using UserApi.Data;

namespace UserApi.Services
{
    public class CardManagementRepository : ICardManagementRepository
    {
        private readonly ApiDbContext _context;

        public CardManagementRepository(ApiDbContext context)
        {
            _context = context;
        }

        // بررسی پیشوند کارت
        public async Task<bool> CheckCardPrefixAsync(string prefix)
        {
            return await _context.CardPrefixes.AnyAsync(cp => cp.Prefix == prefix);
        }

        // دریافت بانک بر اساس پیشوند کارت
        public async Task<Bank> GetBankByPrefixAsync(string prefix)
        {
            return await _context.CardPrefixes
                .Where(cp => cp.Prefix == prefix)
                .Select(cp => cp.Bank)
                .FirstOrDefaultAsync();
        }

        // دریافت کارت‌های کاربر
        public async Task<IEnumerable<CardDetail>> GetUserCardsAsync(string userId)
        {
            return await _context.CardDetails
                .Where(cd => cd.UserId == userId)
                .Include(cd => cd.Bank)
                .ToListAsync();
        }

        // دریافت کارت خاص با شناسه و شناسه کاربر
        public async Task<CardDetail> GetCardByIdAsync(int id, string userId)
        {
            return await _context.CardDetails
                .Where(cd => cd.Id == id && cd.UserId == userId)
                .Include(cd => cd.Bank)
                .FirstOrDefaultAsync();
        }

        // بررسی موجودیت کارت
        public async Task<bool> CardExistsAsync(string cardNumber, string userId)
        {
            return await _context.CardDetails
                .AnyAsync(cd => cd.CardNumber == cardNumber && cd.UserId == userId);
        }

        // اضافه کردن کارت جدید
        public async Task<ServiceResponse<CardDetail>> AddCardAsync(CardDetail cardDetail)
        {
            var response = new ServiceResponse<CardDetail>();

            var bank = await GetBankByPrefixAsync(cardDetail.CardNumber.Substring(0, 6));
            if (bank == null)
            {
                response.Success = false;
                response.Message = "پیش‌شماره کارت معتبر نیست.";
                return response;
            }

            cardDetail.BankId = bank.Id;

            _context.CardDetails.Add(cardDetail);
            await _context.SaveChangesAsync();

            response.Data = cardDetail;
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteCardAsync(CardDetail cardDetail)
        {
            var response = new ServiceResponse<bool>();

            var card = await _context.CardDetails
                .Where(cd => cd.Id == cardDetail.Id && cd.UserId == cardDetail.UserId)
                .FirstOrDefaultAsync();

            if (card == null)
            {
                response.Success = false;
                response.Message = "کارت یافت نشد.";
                response.Data = false;
                return response;
            }

            _context.CardDetails.Remove(card);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateCardAsync(CardDetail cardDetail)
        {
            var response = new ServiceResponse<bool>();

            var card = await _context.CardDetails
                .Where(cd => cd.Id == cardDetail.Id && cd.UserId == cardDetail.UserId)
                .FirstOrDefaultAsync();

            if (card == null)
            {
                response.Success = false;
                response.Message = "کارت یافت نشد.";
                response.Data = false;
                return response;
            }

            card.CardNumber = cardDetail.CardNumber;
            card.ExpirationDate = cardDetail.ExpirationDate;
            card.CVV2 = cardDetail.CVV2;
            card.BankId = cardDetail.BankId;

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            return response;
        }

        // بررسی پیشوند کارت با استفاده از درخواست
        public async Task CheckCardPrefixAsync(CheckCardPrefixRequest request)
        {
            var prefixExists = await _context.CardPrefixes
                .AnyAsync(cp => cp.Prefix == request.Prefix);

            if (!prefixExists)
            {
                throw new Exception("Prefix not found");
            }
        }

        // دریافت کارت‌ها بر اساس شناسه کاربر
        public async Task<IEnumerable<CardDetail>> GetCardsByUserIdAsync(string userId)
        {
            return await _context.CardDetails
                .Where(cd => cd.UserId == userId)
                .ToListAsync();
        }
    }
}
