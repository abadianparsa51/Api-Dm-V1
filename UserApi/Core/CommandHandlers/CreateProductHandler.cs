using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;
using UserApi.Core.Repositories;
using UserApi.Data;
using UserApi.Services;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ServiceResponse<ProductDto>>
{
    private readonly ApiDbContext _db;
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public CreateProductHandler(ApiDbContext db, IProductRepository repo, IMapper mapper)
    {
        _db = db;
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        request.SKU = (request.SKU ?? string.Empty).Trim();
        request.NameFa = (request.NameFa ?? string.Empty).Trim();
        request.NameEn = request.NameEn?.Trim();

        if (string.IsNullOrWhiteSpace(request.SKU)) return Fail("SKU is required");
        if (string.IsNullOrWhiteSpace(request.NameFa)) return Fail("NameFa is required");
        if (request.CategoryId <= 0) return Fail("CategoryId is required");
        if (request.UnitId <= 0) return Fail("UnitId is required");

        if (await _repo.SkuExistsAsync(request.SKU, cancellationToken))
            return Fail("SKU already exists");

        // FK checks
        if (!await _db.Categories.AnyAsync(x => x.CategoryId == request.CategoryId, cancellationToken))
            return Fail("Category not found");

        if (!await _db.Units.AnyAsync(x => x.UnitId == request.UnitId, cancellationToken))
            return Fail("Unit not found");

        if (request.BrandId is not null &&
            !await _db.Brands.AnyAsync(x => x.BrandId == request.BrandId, cancellationToken))
            return Fail("Brand not found");

        if (request.PriceListId is not null || request.PriceIRR is not null)
        {
            if (request.PriceListId is null || request.PriceIRR is null)
                return Fail("Both PriceListId and PriceIRR must be provided together");

            if (!await _db.PriceLists.AnyAsync(x => x.PriceListId == request.PriceListId, cancellationToken))
                return Fail("PriceList not found");

            if (request.PriceIRR <= 0) return Fail("PriceIRR must be > 0");
        }

        if (request.StoreId is not null || request.QtyOnHand is not null || request.ExpireDate is not null)
        {
            if (request.StoreId is null || request.QtyOnHand is null)
                return Fail("For initial inventory, StoreId and QtyOnHand are required");

            if (!await _db.Stores.AnyAsync(x => x.StoreId == request.StoreId, cancellationToken))
                return Fail("Store not found");

            if (request.SupplierId is not null &&
                !await _db.Suppliers.AnyAsync(x => x.SupplierId == request.SupplierId, cancellationToken))
                return Fail("Supplier not found");
        }

        // Aggregate
        var product = new Product
        {
            SKU = request.SKU,
            NameFa = request.NameFa,
            NameEn = request.NameEn,
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,
            UnitId = request.UnitId,
            NetWeightValue = request.NetWeightValue,
            NetWeightUnit = request.NetWeightUnit,
            IsPerishable = request.IsPerishable,
            MinStockAlert = request.MinStockAlert,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Barcodes
        var normalizedBarcodes = request.Barcodes
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        for (int i = 0; i < normalizedBarcodes.Count; i++)
        {
            product.Barcodes.Add(new ProductBarcode
            {
                Barcode = normalizedBarcodes[i],
                BarcodeType = "EAN",
                IsPrimary = i == 0
            });
        }

        // Initial price (optional)
        if (request.PriceListId is not null && request.PriceIRR is not null)
        {
            product.Prices.Add(new ProductPrice
            {
                PriceListId = request.PriceListId.Value,
                PriceIRR = request.PriceIRR.Value,
                ValidFrom = DateTime.UtcNow,
                ValidTo = null
            });

            _db.PriceChangeLogs.Add(new PriceChangeLog
            {
                Product = product,
                PriceListId = request.PriceListId.Value,
                OldPriceIRR = null,
                NewPriceIRR = request.PriceIRR.Value,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = "system",
                Reason = "Initial price"
            });
        }

        // Initial inventory lot (optional)
        if (request.StoreId is not null && request.QtyOnHand is not null)
        {
            product.InventoryLots.Add(new InventoryLot
            {
                StoreId = request.StoreId.Value,
                SupplierId = request.SupplierId,
                LotNumber = string.IsNullOrWhiteSpace(request.LotNumber) ? null : request.LotNumber.Trim(),
                ExpireDate = request.ExpireDate,
                QtyOnHand = request.QtyOnHand.Value,
                CostIRR = request.CostIRR,
                ReceivedAt = DateTime.UtcNow
            });
        }

        var created = await _repo.CreateProductGraphAsync(product, cancellationToken);

        var dto = _mapper.Map<ProductDto>(created);

        if (request.PriceListId is not null && request.PriceIRR is not null)
        {
            dto.InitialPriceListId = request.PriceListId;
            dto.InitialPriceIRR = request.PriceIRR;
        }

        if (request.StoreId is not null && request.QtyOnHand is not null)
        {
            dto.InitialStoreId = request.StoreId;
            dto.InitialQtyOnHand = request.QtyOnHand;
            dto.InitialExpireDate = request.ExpireDate;
        }

        return new ServiceResponse<ProductDto> { Success = true, Data = dto };
    }

    private static ServiceResponse<ProductDto> Fail(string message)
        => new ServiceResponse<ProductDto> { Success = false, Message = message };
}
