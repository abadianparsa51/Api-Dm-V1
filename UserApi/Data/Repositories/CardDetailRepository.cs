using Microsoft.EntityFrameworkCore;
using UserApi.Core.Models;
using UserApi.Data;

public class CardDetailRepository : ICardDetailRepository
{
    private readonly ApiDbContext _context;

    public CardDetailRepository(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CardDetail>> GetUserCardsAsync(string userId)
    {
        return await _context.CardDetails
            .Where(c => c.UserId == userId)
            .Include(c => c.Bank)
            .ToListAsync();
    }

    public async Task<CardDetail> GetCardByIdAsync(int id, string userId)
    {
        return await _context.CardDetails
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }

    public async Task<IEnumerable<CardDetail>> GetCardsByUserIdAsync(string userId)
    {
        return await _context.CardDetails
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task AddCardAsync(CardDetail card)
    {
        await _context.CardDetails.AddAsync(card);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCardAsync(CardDetail card)
    {
        _context.CardDetails.Remove(card);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CardExistsAsync(string cardNumber, string userId)
    {
        return await _context.CardDetails
            .AnyAsync(c => c.CardNumber == cardNumber && c.UserId == userId);
    }

    public async Task<Bank> GetBankByPrefixAsync(string prefix)
    {
        return await _context.CardPrefixes
            .Where(cp => cp.Prefix == prefix)
            .Select(cp => cp.Bank)
            .FirstOrDefaultAsync();
    }
}
