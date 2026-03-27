using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;

namespace UserApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class CardDetailController : ControllerBase
    {
        private readonly ICardManagementService _cardService;

        public CardDetailController(ICardManagementService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCard([FromBody] CardDetailDTO cardDetailDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found in token.");

            var response = await _cardService.AddCardAsync(userId, cardDetailDto);
            if (response.Success)
                return Ok(new { message = response.Message, cardDetails = response.Data });

            return BadRequest(response.Message);
        }

        [HttpDelete("delete/{cardId}")]
        public async Task<IActionResult> DeleteCard(int cardId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found in token.");

            var success = await _cardService.DeleteCardAsync(cardId, userId);
            if (success)
                return Ok(new { message = "Card deleted successfully." });

            return NotFound("Card not found.");
        }

        [HttpPut("update/{cardId}")]
        public async Task<IActionResult> UpdateCard(int cardId, [FromBody] CardDetailDTO cardDetailDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found in token.");

            var success = await _cardService.UpdateCardAsync(cardId, userId, cardDetailDto);
            if (success)
                return Ok(new { message = "Card updated successfully." });

            return NotFound("Card not found.");
        }

        [HttpGet("user-cards")]
        public async Task<IActionResult> GetUserCards()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found in token.");

            var cards = await _cardService.GetCardsByUserIdAsync(userId);
            return Ok(cards);
        }
    }
}
