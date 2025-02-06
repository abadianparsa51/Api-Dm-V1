// ContactCommandRepository.cs
using UserApi.Core.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;

public class ContactCommandRepository : IContactCommandRepository
{
    private readonly ApiDbContext _context;

    public ContactCommandRepository(ApiDbContext context)
    {
        _context = context;
    }

    public async Task AddContactAsync(Contact contact)
    {
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateContactAsync(Contact contact)
    {
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(int contactId)
    {
        var contact = await _context.Contacts.FindAsync(contactId);
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Contact> GetContactByIdAsync(int contactId)
    {
        return await _context.Contacts.FindAsync(contactId);
    }
}
 