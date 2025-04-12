namespace UserApi.Core.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string SKU { get; set; } = null!;
        public string NameFa { get; set; } = null!;
        public string? NameEn { get; set; }

        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int UnitId { get; set; }

        public decimal? NetWeightValue { get; set; }
        public string? NetWeightUnit { get; set; } // "g", "kg", "ml"...

        public bool IsPerishable { get; set; }
        public int MinStockAlert { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Category Category { get; set; } = null!;
        public Brand? Brand { get; set; }
        public Unit Unit { get; set; } = null!;

        public ICollection<ProductBarcode> Barcodes { get; set; } = new List<ProductBarcode>();
        public ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();
        public ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();
    }
}
