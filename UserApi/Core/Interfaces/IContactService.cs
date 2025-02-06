// IContactService.cs
using System.Threading.Tasks;
using UserApi.Services;

public interface IContactService
{
    Task<ServiceResponse<ContactDto>> AddContact(AddContactCommand command, string userId);
    Task<ServiceResponse<ContactDto>> UpdateContact(UpdateContactCommand command);
    Task<ServiceResponse<bool>> DeleteContact(DeleteContactCommand command);
    Task<List<ContactDto>> GetAllContacts();  // New method for getting all contacts
}
