using UserApi.Services;

namespace UserApi.Core.Interfaces
{
    public interface IContactService
    {
        Task<ServiceResponse<ContactDTO>> AddContactAsync(string userId, ContactDTO contactDto);
        Task<ServiceResponse<ContactDTO>> GetContactByIdAsync(string userId, int id);
        Task<ServiceResponse<IEnumerable<ContactDTO>>> GetUserContactsAsync(string userId);
        Task<ServiceResponse<ContactDTO>> UpdateContactAsync(string userId, int id, ContactDTO updatedContactDto);
        Task<ServiceResponse<bool>> DeleteContactAsync(string userId, int id);
    }
}
