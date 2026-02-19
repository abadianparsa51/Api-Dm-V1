using System.Collections.Generic;

namespace UserApi.Core.Models.DTOs
{
    public class CategoryTreeDto
    {
        public int CategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public List<CategoryTreeDto> Children { get; set; } = new();
    }
}
