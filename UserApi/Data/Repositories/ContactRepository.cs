using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserApi.Core.Models;
using UserApi.Core.Repositories;

namespace UserApi.Data.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApiDbContext _context;

        public ContactRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            return null;
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<Contact> UpdateContactAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return false;

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
