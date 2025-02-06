using Microsoft.AspNetCore.Identity;

namespace UserApi.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<CardDetail> Cards { get; set; } // کارت‌های کاربر
                                                           // اضافه کردن ارتباط یک کاربر با چند Contact
        public ICollection<Contact> Contacts { get; set; }
        public Wallet Wallet { get; set; } // ارتباط یک به یک با کیف پول
    }
}
