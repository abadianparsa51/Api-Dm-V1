namespace UserApi.Core.Models
{
    public class PriceChangeLog
    {
        public long LogId { get; set; }

        public int ProductId { get; set; }
        public int PriceListId { get; set; }

        public long? OldPriceIRR { get; set; }
        public long NewPriceIRR { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public string? ChangedBy { get; set; }
        public string? Reason { get; set; }

        public Product Product { get; set; } = null!;
        public PriceList PriceList { get; set; } = null!;
    }
}
