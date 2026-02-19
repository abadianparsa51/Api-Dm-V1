namespace UserApi.Core.Models.DTOs
{
    public class ProductListItemDto
    {
        public int ProductId { get; set; }
        public string SKU { get; set; } = null!;
        public string NameFa { get; set; } = null!;
        public string? Brand { get; set; }
        public string Category { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
