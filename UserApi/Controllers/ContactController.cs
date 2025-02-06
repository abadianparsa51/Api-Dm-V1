using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserApi.Core.Interfaces;
using UserApi.Core.Models;
using UserApi.Core.Services;

namespace UserApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

      private string GetUserId()
{
    return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}

        [HttpPost]
        public async Task<ActionResult<ContactDTO>> AddContact([FromBody] ContactDTO contactDto)
        {
            {
                try
                {
                    var userId = GetUserId();

                    // بررسی اینکه آیا Contact موجود است یا باید یک Contact جدید ساخته شود
                    var contactResponse = await _contactService.AddContactAsync(userId, contactDto);

                    if (!contactResponse.Success)
                    {
                        return BadRequest(contactResponse.Message);
                    }

                    return CreatedAtAction(nameof(GetContactById), new { id = contactResponse.Data.Id }, contactResponse.Data);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            
        }


            [HttpGet("{id}")]
        public async Task<ActionResult<ContactDTO>> GetContactById(int id)
        {
            var userId = GetUserId();
            var contact = await _contactService.GetContactByIdAsync(userId, id);
            if (contact.Data == null)
            {
                return NotFound();
            }

            return Ok(contact.Data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetAllContacts()
        {
            var userId = GetUserId();
            var contacts = await _contactService.GetUserContactsAsync(userId);
            return Ok(contacts.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ContactDTO>> UpdateContact(int id, [FromBody] ContactDTO contactDto)
        {
            var userId = GetUserId();
            contactDto.Id = id;
            var updatedContact = await _contactService.UpdateContactAsync(userId, id, contactDto);
            if (updatedContact.Data == null)
            {
                return NotFound();
            }

            return Ok(updatedContact.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var userId = GetUserId();
            var success = await _contactService.DeleteContactAsync(userId, id);
            if (!success.Data)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
