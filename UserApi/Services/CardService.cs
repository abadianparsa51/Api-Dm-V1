using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;
using UserApi.Helpers;

namespace UserApi.Core.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
    

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
       
        }

        public async Task<CheckCardPrefixResponseDto> CheckCardPrefixAsync(CheckCardPrefixRequest request)
        {
            var response = new CheckCardPrefixResponseDto();

            // Check if the card prefix exists using the repository
            var exists = await _cardRepository.CheckCardPrefixAsync(request.CardNumber.Substring(0, 6));

            response.IsValid = exists;
            response.Message = exists ? "Valid card prefix." : "Invalid card prefix.";

            return response;
        }
    }
}
