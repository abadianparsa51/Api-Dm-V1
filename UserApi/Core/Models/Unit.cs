namespace UserApi.Core.Models
{
    public class Unit
    {
        public int UnitId { get; set; }
        public string Name { get; set; } = null!;
        public string? Symbol { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
