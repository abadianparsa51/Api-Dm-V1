namespace UserApi.Core.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? City { get; set; } = "Tehran";
        public string? Address { get; set; }
        public bool IsImporter { get; set; }

        public ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();
    }
}
