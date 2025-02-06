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
        private readonly ICardDetailService _cardDetailService;
        private readonly IUserService _userService;

        public CardDetailController(ICardDetailService cardDetailService, IUserService userService)
        {
            _cardDetailService = cardDetailService;
            _userService = userService;
        }

        /// <summary>
        /// Add a new card for the authenticated user.
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> AddCard([FromBody] CardDetailDTO cardDetailDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found in token.");

            var response = await _cardDetailService.AddCardAsync(userId, cardDetailDto);
            if (response.Success)
                return Ok(new { message = response.Message, cardDetails = response.Data });

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Retrieve all cards for the authenticated user.
        /// </summary>
        [HttpGet("user-cards")]
        public async Task<IActionResult> GetUserCards()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found in token.");

            var response = await _cardDetailService.GetUserCardsAsync(userId);
            if (response.Success)
                return Ok(response.Data);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Delete a card by ID for the authenticated user.
        /// </summary>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found in token.");

            var response = await _cardDetailService.DeleteCardAsync(userId, id);
            if (response.Success)
                return Ok(new { message = response.Message });

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Update card details for the authenticated user.
        /// </summary>
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateCard(int id, [FromBody] CardDetailDTO updatedCardDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found in token.");

            var response = await _cardDetailService.UpdateCardAsync(userId, id, updatedCardDto);
            if (response.Success)
                return Ok(new { message = response.Message, card = response.Data });

            return BadRequest(response.Message);
        }
    }
}
