using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserApi.Core.Repositories
{
    public interface IContactRepository
    {
        Task<Contact> AddContactAsync(Contact contact);
        Task<Contact> GetContactByIdAsync(int id);
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> UpdateContactAsync(Contact contact);
        Task<bool> DeleteContactAsync(int id);
    }
}
