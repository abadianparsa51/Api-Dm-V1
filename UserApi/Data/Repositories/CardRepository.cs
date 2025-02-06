using UserApi.Core.Models;
using UserApi.Data;
using UserApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserApi.Core.Models.DTOs;

namespace UserApi.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly ApiDbContext _context;

        public CardRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckCardPrefixAsync(string prefix)
        {
            // فرض می‌کنیم که کارت‌ها یک Prefix دارند
            return await _context.CardDetails.AnyAsync(c => c.CardNumber.StartsWith(prefix));
        }

        public async Task AddAsync(CardDetail cardDetail)
        {
            await _context.CardDetails.AddAsync(cardDetail);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<CardDetailDTO>> GetCardsByUserEmailAsync(string userEmail)
        {
            throw new NotImplementedException();
        }
    }
}
