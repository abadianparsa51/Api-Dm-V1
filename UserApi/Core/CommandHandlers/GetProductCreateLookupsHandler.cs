using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserApi.Core.Models.DTOs;
using UserApi.Core.Queries;
using UserApi.Data;
using UserApi.Services;

public class GetProductCreateLookupsHandler : IRequestHandler<GetProductCreateLookupsQuery, ServiceResponse<ProductCreateLookupsDto>>
{
    private readonly ApiDbContext _db;

    public GetProductCreateLookupsHandler(ApiDbContext db)
    {
        _db = db;
    }

    public async Task<ServiceResponse<ProductCreateLookupsDto>> Handle(GetProductCreateLookupsQuery request, CancellationToken cancellationToken)
    {
        var categoriesFlat = await _db.Categories
            .AsNoTracking()
            .Select(c => new CategoryTreeDto
            {
                CategoryId = c.CategoryId,
                ParentCategoryId = c.ParentCategoryId,
                Name = c.Name
            })
            .ToListAsync(cancellationToken);

        var categoriesTree = BuildTree(categoriesFlat);

        var brandsQuery = _db.Brands.AsNoTracking();
        if (request.OnlyPremiumBrands)
            brandsQuery = brandsQuery.Where(b => b.IsPremium);

        var brands = await brandsQuery
            .OrderBy(b => b.Name)
            .Select(b => new LookupItemDto { Id = b.BrandId, Title = b.Name, Code = b.Country })
            .ToListAsync(cancellationToken);

        var units = await _db.Units.AsNoTracking()
            .OrderBy(u => u.Name)
            .Select(u => new LookupItemDto { Id = u.UnitId, Title = u.Name, Code = u.Symbol })
            .ToListAsync(cancellationToken);

        var stores = await _db.Stores.AsNoTracking()
            .OrderBy(s => s.Name)
            .Select(s => new LookupItemDto { Id = s.StoreId, Title = s.Name, Code = s.Area })
            .ToListAsync(cancellationToken);

        var suppliers = await _db.Suppliers.AsNoTracking()
            .OrderBy(s => s.Name)
            .Select(s => new LookupItemDto { Id = s.SupplierId, Title = s.Name, Code = s.City })
            .ToListAsync(cancellationToken);

        var priceListsQuery = _db.PriceLists.AsNoTracking();
        if (request.OnlyActivePriceLists)
            priceListsQuery = priceListsQuery.Where(p => p.IsActive);

        var priceLists = await priceListsQuery
            .OrderBy(p => p.Name)
            .Select(p => new LookupItemDto { Id = p.PriceListId, Title = p.Name, Code = p.Currency })
            .ToListAsync(cancellationToken);

        return new ServiceResponse<ProductCreateLookupsDto>
        {
            Success = true,
            Data = new ProductCreateLookupsDto
            {
                Categories = categoriesTree,
                Brands = brands,
                Units = units,
                Stores = stores,
                Suppliers = suppliers,
                PriceLists = priceLists
            }
        };
    }

    private static List<CategoryTreeDto> BuildTree(List<CategoryTreeDto> flat)
    {
        var dict = flat.ToDictionary(x => x.CategoryId);
        foreach (var c in flat)
        {
            if (c.ParentCategoryId is int pid && dict.TryGetValue(pid, out var parent))
                parent.Children.Add(c);
        }

        // Roots only
        var roots = flat.Where(x => x.ParentCategoryId == null).ToList();

        // Optional: order children by name recursively
        void Order(CategoryTreeDto node)
        {
            node.Children = node.Children.OrderBy(x => x.Name).ToList();
            foreach (var ch in node.Children) Order(ch);
        }
        foreach (var r in roots) Order(r);

        return roots.OrderBy(x => x.Name).ToList();
    }
}
