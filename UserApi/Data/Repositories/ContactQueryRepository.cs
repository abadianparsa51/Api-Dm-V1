using UserApi.Core.Models;
using UserApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApi.Data;

namespace UserApi.Repositories
{
    public class ContactQueryRepository : IContactQueryRepository
    {
        private readonly ApiDbContext _dbContext;

        public ContactQueryRepository(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Contact> GetContactByIdAsync(int contactId)
        {
            return await _dbContext.Contacts
                .Include(c => c.Cards)
                .FirstOrDefaultAsync(c => c.Id == contactId);
        }

        public async Task<List<Contact>> GetAllContactsByUserIdAsync(string userId)
        {
            return await _dbContext.Contacts
                .Where(c => c.UserId == userId)
                .Include(c => c.Cards)
                .ToListAsync();
        }
    }
}
