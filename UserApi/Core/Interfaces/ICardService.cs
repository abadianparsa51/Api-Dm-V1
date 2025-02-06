using UserApi.Core.Models.DTOs;

namespace UserApi.Core.Interfaces
{
    public interface ICardService
    {
        Task<CheckCardPrefixResponseDto> CheckCardPrefixAsync(CheckCardPrefixRequest request);
    }
}
