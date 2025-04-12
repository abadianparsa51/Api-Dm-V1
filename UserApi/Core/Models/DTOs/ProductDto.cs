using System;
using System.Collections.Generic;

namespace UserApi.Core.Models.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string SKU { get; set; } = null!;
        public string NameFa { get; set; } = null!;
        public string? NameEn { get; set; }

        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int UnitId { get; set; }

        public decimal? NetWeightValue { get; set; }
        public string? NetWeightUnit { get; set; }

        public bool IsPerishable { get; set; }
        public int MinStockAlert { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<string> Barcodes { get; set; } = new();

        // Optional (if you create initial price/stock in same request)
        public long? InitialPriceIRR { get; set; }
        public int? InitialPriceListId { get; set; }
        public decimal? InitialQtyOnHand { get; set; }
        public int? InitialStoreId { get; set; }
        public DateOnly? InitialExpireDate { get; set; }
    }
}
