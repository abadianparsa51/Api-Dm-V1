using Microsoft.AspNetCore.Mvc;
using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        // Method to check the card prefix
        [HttpPost("CheckPrefix")]
        public async Task<IActionResult> CheckCardPrefix([FromBody] CheckCardPrefixRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.CardNumber) || request.CardNumber.Length < 6)
            {
                return BadRequest(new CheckCardPrefixResponseDto
                {
                    IsValid = false,
                    Message = "Invalid request. Card number must be at least 6 digits."
                });
            }

            try
            {
                var response = await _cardService.CheckCardPrefixAsync(request);
                if (response.IsValid)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
    }
}
