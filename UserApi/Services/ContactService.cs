using AutoMapper;
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

}
