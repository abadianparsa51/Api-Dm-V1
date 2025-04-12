using AutoMapper;
using System.Linq;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;

namespace UserApi.Helper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Barcodes, opt => opt.MapFrom(s => s.Barcodes.Select(b => b.Barcode).ToList()));
        }
    }
}
