using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using UserApi.Data;
using UserApi.Services;

public class CreateProductByCodeHandler : IRequestHandler<CreateProductByCodeCommand, ServiceResponse<UserApi.Core.Models.DTOs.ProductDto>>
{
    private readonly ApiDbContext _db;
    private readonly IMediator _mediator;

    public CreateProductByCodeHandler(ApiDbContext db, IMediator mediator)
    {
        _db = db;
        _mediator = mediator;
    }

    public async Task<ServiceResponse<UserApi.Core.Models.DTOs.ProductDto>> Handle(CreateProductByCodeCommand request, CancellationToken cancellationToken)
    {
        // Resolve Category
        var categoryName = (request.CategoryName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(categoryName))
            return new ServiceResponse<UserApi.Core.Models.DTOs.ProductDto> { Success = false, Message = "CategoryName is required" };

        var categoryId = await _db.Categories
            .AsNoTracking()
            .Where(c => c.Name == categoryName)
            .Select(c => (int?)c.CategoryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (categoryId is null)
            return new ServiceResponse<UserApi.Core.Models.DTOs.ProductDto> { Success = false, Message = "Category not found" };

        // Resolve Unit
        var unitName = (request.UnitName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(unitName))
            return new ServiceResponse<UserApi.Core.Models.DTOs.ProductDto> { Success = false, Message = "UnitName is required" };

        var unitQuery = _db.Units.AsNoTracking().Where(u => u.Name == unitName);
        if (!string.IsNullOrWhiteSpace(request.UnitSymbol))
        {
            var sym = request.UnitSymbol.Trim();
            unitQuery = unitQuery.Where(u => u.Symbol == sym);
        }

        var unitId = await unitQuery.Select(u => (int?)u.UnitId).FirstOrDefaultAsync(cancellationToken);
        if (unitId is null)
            return new ServiceResponse<UserApi.Core.Models.DTOs.ProductDto> { Success = false, Message = "Unit not found" };

        // Resolve Brand (optional)
        int? brandId = null;
        if (!string.IsNullOrWhiteSpace(request.BrandName))
        {
            var brandName = request.BrandName.Trim();
            brandId = await _db.Brands.AsNoTracking()
                .Where(b => b.Name == brandName)
                .Select(b => (int?)b.BrandId)
                .FirstOrDefaultAsync(cancellationToken);

            if (brandId is null)
                return new ServiceResponse<UserApi.Core.Models.DTOs.ProductDto> { Success = false, Message = "Brand not found" };
        }

        // Resolve PriceList (optional)
        int? priceListId = null;
        if (request.PriceListName is not null || request.PriceIRR is not null)
        {
            if (string.IsNullOrWhiteSpace(request.PriceListName) || request.PriceIRR is null)
                return new ServiceResponse<UserApi.Core.Models.DTOs.ProductDto> { Success = false, Message = "Both PriceListName and PriceIRR must be provided together" };

            var plName = request.PriceListName.Trim();
            priceListId = await _db.PriceLists.AsNoTracking()
                .Where(p => p.Name == plName)
                .Select(p => (int?)p.PriceListId)
                .FirstOrDefaultAsync(cancellationToken);

            if (priceListId is null)
                return new ServiceResponse<UserApi.Core.Models.DTOs.ProductDto> { Success = false, Message = "PriceList not found" };
        }

        // Resolve Store/Supplier for initial inventory (optional)
        int? storeId = null;
        int? supplierId = null;

        if (request.StoreName is not null || request.QtyOnHand is not null || request.ExpireDate is not null)
        {
            if (string.IsNullOrWhiteSpace(request.StoreName) || request.QtyOnHand is null)
                return new ServiceResponse<UserApi.Core.Models.DTOs.ProductDto> { Success = false, Message = "For initial inventory, StoreName and QtyOnHand are required" };

            var sName = request.StoreName.Trim();
            storeId = await _db.Stores.AsNoTracking()
                .Where(s => s.Name == sName)
                .Select(s => (int?)s.StoreId)
                .FirstOrDefaultAsync(cancellationToken);

            if (storeId is null)
                return new ServiceResponse<UserApi.Core.Models.DTOs.ProductDto> { Success = false, Message = "Store not found" };

            if (!string.IsNullOrWhiteSpace(request.SupplierName))
            {
                var supName = request.SupplierName.Trim();
                supplierId = await _db.Suppliers.AsNoTracking()
                    .Where(s => s.Name == supName)
                    .Select(s => (int?)s.SupplierId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (supplierId is null)
                    return new ServiceResponse<UserApi.Core.Models.DTOs.ProductDto> { Success = false, Message = "Supplier not found" };
            }
        }

        // Map into existing command (single source of truth for validation + write)
        var cmd = new CreateProductCommand
        {
            SKU = request.SKU,
            NameFa = request.NameFa,
            NameEn = request.NameEn,
            CategoryId = categoryId.Value,
            BrandId = brandId,
            UnitId = unitId.Value,
            NetWeightValue = request.NetWeightValue,
            NetWeightUnit = request.NetWeightUnit,
            IsPerishable = request.IsPerishable,
            MinStockAlert = request.MinStockAlert,
            Barcodes = request.Barcodes,
            PriceListId = priceListId,
            PriceIRR = request.PriceIRR,
            StoreId = storeId,
            SupplierId = supplierId,
            QtyOnHand = request.QtyOnHand,
            ExpireDate = request.ExpireDate,
            CostIRR = request.CostIRR,
            LotNumber = request.LotNumber
        };

        return await _mediator.Send(cmd, cancellationToken);
    }
}
