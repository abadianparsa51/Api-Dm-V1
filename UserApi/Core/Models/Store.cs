namespace UserApi.Core.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        public string Name { get; set; } = null!;
        public string? Area { get; set; }
        public string? Address { get; set; }

        public ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();
    }
}
