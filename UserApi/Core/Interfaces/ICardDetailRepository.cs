using UserApi.Core.Models;

public interface ICardDetailRepository
{
    Task<IEnumerable<CardDetail>> GetUserCardsAsync(string userId);
    Task<CardDetail> GetCardByIdAsync(int id, string userId);
    Task AddCardAsync(CardDetail card);
    Task DeleteCardAsync(CardDetail card);
    Task<bool> CardExistsAsync(string cardNumber, string userId);
    Task<Bank> GetBankByPrefixAsync(string prefix);
    Task<IEnumerable<CardDetail>> GetCardsByUserIdAsync(string userId);
}
