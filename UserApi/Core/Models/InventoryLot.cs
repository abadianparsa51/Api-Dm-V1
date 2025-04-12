namespace UserApi.Core.Models
{
    public class InventoryLot
    {
        public long LotId { get; set; }

        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int? SupplierId { get; set; }

        public string? LotNumber { get; set; }
        public DateOnly? ExpireDate { get; set; }

        public decimal QtyOnHand { get; set; }
        public long? CostIRR { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

        public Store Store { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public Supplier? Supplier { get; set; }
    }
}
