// IContactCommandRepository.cs
using System.Threading.Tasks;

public interface IContactCommandRepository
{
    Task AddContactAsync(Contact contact);
    Task UpdateContactAsync(Contact contact);
    Task DeleteContactAsync(int contactId);
    Task<Contact> GetContactByIdAsync(int contactId);
}
