using MediatR;
using System.Collections.Generic;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

/// <summary>
/// Client can send human-friendly codes/names; backend resolves to FK ids.
/// Useful when client does NOT want to manage ids.
/// </summary>
public class CreateProductByCodeCommand : IRequest<ServiceResponse<ProductDto>>
{
    public string SKU { get; set; } = null!;
    public string NameFa { get; set; } = null!;
    public string? NameEn { get; set; }

    // Resolve by names/codes:
    public string CategoryName { get; set; } = null!;   // e.g. "قهوه و کپسول"
    public string? BrandName { get; set; }              // e.g. "Illy"
    public string UnitName { get; set; } = null!;       // e.g. "jar"
    public string? UnitSymbol { get; set; }             // optional helper for resolve

    public decimal? NetWeightValue { get; set; }
    public string? NetWeightUnit { get; set; }
    public bool IsPerishable { get; set; }
    public int MinStockAlert { get; set; }

    public List<string> Barcodes { get; set; } = new();

    public string? PriceListName { get; set; }          // e.g. "Regular"
    public long? PriceIRR { get; set; }

    public string? StoreName { get; set; }              // e.g. "شعبه فرمانیه"
    public string? SupplierName { get; set; }           // optional
    public decimal? QtyOnHand { get; set; }
    public DateOnly? ExpireDate { get; set; }
    public long? CostIRR { get; set; }
    public string? LotNumber { get; set; }
}
