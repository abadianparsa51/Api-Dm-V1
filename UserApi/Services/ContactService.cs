<<<<<<< HEAD
﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserApi.Core.Interfaces;
using UserApi.Data;

namespace UserApi.Services
{
    public class ContactService : IContactService
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public ContactService(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ContactDTO>> AddContactAsync(string userId, ContactDTO contactDto)
        {
            var response = new ServiceResponse<ContactDTO>();

            if (string.IsNullOrEmpty(contactDto.Name) || string.IsNullOrEmpty(contactDto.Phone))
            {
                response.Message = "Name and Phone are required.";
                return response;
            }

            var contact = new Contact
            {
                Name = contactDto.Name,
                Phone = contactDto.Phone,
                Mail = contactDto.Mail,
                UserId = userId
            };

            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Contact added successfully.";
            response.Data = _mapper.Map<ContactDTO>(contact);

            return response;
        }

        public async Task<ServiceResponse<ContactDTO>> GetContactByIdAsync(string userId, int id)
        {
            var response = new ServiceResponse<ContactDTO>();

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (contact == null)
            {
                response.Message = "Contact not found.";
                return response;
            }

            response.Success = true;
            response.Data = _mapper.Map<ContactDTO>(contact);
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<ContactDTO>>> GetUserContactsAsync(string userId)
        {
            var response = new ServiceResponse<IEnumerable<ContactDTO>>();

            var contacts = await _context.Contacts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            response.Success = true;
            response.Data = _mapper.Map<IEnumerable<ContactDTO>>(contacts);
            return response;
        }

        public async Task<ServiceResponse<ContactDTO>> UpdateContactAsync(string userId, int id, ContactDTO updatedContactDto)
        {
            var response = new ServiceResponse<ContactDTO>();

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (contact == null)
            {
                response.Message = "Contact not found.";
                return response;
            }

            contact.Name = updatedContactDto.Name;
            contact.Phone = updatedContactDto.Phone;
            contact.Mail = updatedContactDto.Mail;

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Contact updated successfully.";
            response.Data = _mapper.Map<ContactDTO>(contact);
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteContactAsync(string userId, int id)
        {
            var response = new ServiceResponse<bool>();

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (contact == null)
            {
                response.Message = "Contact not found.";
                response.Data = false;
                return response;
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Contact deleted successfully.";
            response.Data = true;
            return response;
        }
    }

=======
﻿// ContactService.cs
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using UserApi.Core.Interfaces;
using UserApi.Core.Repositories;
using UserApi.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper; // Add this line for AutoMapper
    private readonly IMediator _mediator;

    public ContactService(IMediator mediator, IContactRepository contactRepository, IMapper mapper)
    {
        _mediator = mediator;
        _contactRepository = contactRepository;
        _mapper = mapper; // Initialize the mapper
    }

    public async Task<ServiceResponse<ContactDto>> AddContact(AddContactCommand command, string userId)
    {
        // ارسال userId به دستور AddContactCommand
        var addContactCommand = new AddContactCommand(command.Name, command.Phone, command.Mail, command.DestinationCardNumber);
        var response = await _mediator.Send(addContactCommand);

        return response.Success
            ? new ServiceResponse<ContactDto> { Success = true, Data = response.Data }
            : new ServiceResponse<ContactDto> { Success = false, Message = response.Message };
    }

    public async Task<ServiceResponse<ContactDto>> UpdateContact(UpdateContactCommand command)
    {
        return await _mediator.Send(command);
    }

    public async Task<ServiceResponse<bool>> DeleteContact(DeleteContactCommand command)
    {
        return await _mediator.Send(command);
    }
    // Implement GetAllContacts method
    public async Task<List<ContactDto>> GetAllContacts()
    {
        var contacts = await _contactRepository.GetAllContactsAsync(); // Assuming this method exists in the repository
        return contacts.Select(contact => new ContactDto
        {
            Id = contact.Id,
            Name = contact.Name,
            Mail = contact.Mail
        }).ToList();
    }
>>>>>>> future/Api-Dm-v1/Impliment-CQRS-for-Contact
}
