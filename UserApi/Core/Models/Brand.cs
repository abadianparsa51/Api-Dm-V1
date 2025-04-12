namespace UserApi.Core.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
        public bool IsPremium { get; set; } = true;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
