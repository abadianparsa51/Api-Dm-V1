namespace UserApi.Core.Models
{
    public class PriceList
    {
        public int PriceListId { get; set; }
        public string Name { get; set; } = null!;     // Regular, VIP, Promo
        public string Currency { get; set; } = "IRR";
        public bool IsActive { get; set; } = true;

        public ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
    }
}
