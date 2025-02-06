using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UserApi.Services;
using UserApi.Core.Interfaces;
using UserApi.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Add authorization
[Route("api/[controller]")]
[ApiController]

public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    // Endpoint for adding a new contact

    [HttpPost]
    public async Task<IActionResult> AddContact([FromBody] AddContactCommand command)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return BadRequest("User ID not found in token.");

        // اضافه کردن userId به دستور
        var response = await _contactService.AddContact(command, userId);
        if (response.Success)
            return Ok(new { message = response.Message, cardDetails = response.Data });

        return BadRequest(response.Message);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateContact(UpdateContactCommand command)
    {
        var response = await _contactService.UpdateContact(command);
        return response.Success ? Ok(response) : BadRequest(response.Message);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteContact(DeleteContactCommand command)
    {
        var response = await _contactService.DeleteContact(command);
        return response.Success ? Ok(response) : BadRequest(response.Message);
    }

    // Add GetAllContacts to return all contacts
    [HttpGet]
    public async Task<IActionResult> GetAllContacts()
    {
        var contacts = await _contactService.GetAllContacts();
        return Ok(contacts);
    }
}
