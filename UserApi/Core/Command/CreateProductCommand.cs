using MediatR;
using System.Collections.Generic;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

public class CreateProductCommand : IRequest<ServiceResponse<ProductDto>>
{
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

    // Barcodes (optional). If provided, first one becomes primary.
    public List<string> Barcodes { get; set; } = new();

    // Optional: create initial price in the same transaction
    public int? PriceListId { get; set; }
    public long? PriceIRR { get; set; }

    // Optional: create initial inventory lot in the same transaction
    public int? StoreId { get; set; }
    public int? SupplierId { get; set; }
    public decimal? QtyOnHand { get; set; }
    public DateOnly? ExpireDate { get; set; }
    public long? CostIRR { get; set; }
    public string? LotNumber { get; set; }
}
