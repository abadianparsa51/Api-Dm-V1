using System.Collections.Generic;

namespace UserApi.Core.Models.DTOs
{
    public class ProductCreateLookupsDto
    {
        public List<CategoryTreeDto> Categories { get; set; } = new();
        public List<LookupItemDto> Brands { get; set; } = new();
        public List<LookupItemDto> Units { get; set; } = new();
        public List<LookupItemDto> Stores { get; set; } = new();
        public List<LookupItemDto> Suppliers { get; set; } = new();
        public List<LookupItemDto> PriceLists { get; set; } = new();
    }
}
