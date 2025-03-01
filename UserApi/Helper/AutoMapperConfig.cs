using AutoMapper;
using UserApi.Core.Models;

namespace UserApi.Helper
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactDto>().ReverseMap();
        }
    }
}
