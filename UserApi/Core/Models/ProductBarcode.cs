namespace UserApi.Core.Models
{
    public class ProductBarcode
    {
        public int BarcodeId { get; set; }
        public int ProductId { get; set; }

        public string Barcode { get; set; } = null!;
        public string BarcodeType { get; set; } = "EAN";
        public bool IsPrimary { get; set; }

        public Product Product { get; set; } = null!;
    }
}
