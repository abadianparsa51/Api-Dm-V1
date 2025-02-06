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

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == contactDto.Id && c.UserId == userId);

            if (contact == null)
            {
                // ایجاد Contact جدید
                contact = new Contact
                {
                    Name = contactDto.Name,
                    Phone = contactDto.Phone,
                    Mail = contactDto.Mail,
                    DestinationCardNumber = contactDto.DestinationCardNumber,
                    UserId = userId
                };
                _context.Contacts.Add(contact);
            }
            else
            {
                // به‌روزرسانی اطلاعات Contact موجود
                contact.Name = contactDto.Name;
                contact.Phone = contactDto.Phone;
                contact.Mail = contactDto.Mail;

                if (!string.IsNullOrEmpty(contactDto.DestinationCardNumber))
                {
                    contact.DestinationCardNumber = contactDto.DestinationCardNumber;
                }
            }

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = new ContactDTO
            {
                Id = contact.Id,
                Name = contact.Name,
                Phone = contact.Phone,
                Mail = contact.Mail,
                DestinationCardNumber = contact.DestinationCardNumber
            };

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

            // اگر مقدار جدیدی برای کارت مقصد وجود داشت، آن را به‌روزرسانی کن
            if (!string.IsNullOrEmpty(updatedContactDto.DestinationCardNumber))
            {
                contact.DestinationCardNumber = updatedContactDto.DestinationCardNumber;
            }

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
