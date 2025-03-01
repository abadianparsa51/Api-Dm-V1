using UserApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserApi.Core.Interfaces
{
    public interface IContactQueryRepository
    {
        Task<Contact> GetContactByIdAsync(int contactId);
        Task<List<Contact>> GetAllContactsByUserIdAsync(string userId);
    }
}
