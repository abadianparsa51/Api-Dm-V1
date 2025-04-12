namespace UserApi.Core.Models
{
    public class ProductPrice
    {
        public int ProductPriceId { get; set; }
        public int PriceListId { get; set; }
        public int ProductId { get; set; }

        public long PriceIRR { get; set; }
        public DateTime ValidFrom { get; set; } = DateTime.UtcNow;
        public DateTime? ValidTo { get; set; }

        public PriceList PriceList { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
