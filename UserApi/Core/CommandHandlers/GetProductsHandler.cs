using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserApi.Core.Models.DTOs;
using UserApi.Core.Queries;
using UserApi.Data;
using UserApi.Services;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, ServiceResponse<PagedResult<ProductListItemDto>>>
{
    private readonly ApiDbContext _db;

    public GetProductsHandler(ApiDbContext db)
    {
        _db = db;
    }

    public async Task<ServiceResponse<PagedResult<ProductListItemDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        if (request.Page <= 0) request.Page = 1;
        if (request.PageSize <= 0) request.PageSize = 20;
        if (request.PageSize > 200) request.PageSize = 200;

        var q = _db.Products.AsNoTracking()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .AsQueryable();

        if (!request.IncludeInactive)
            q = q.Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            q = q.Where(p =>
                p.SKU.Contains(s) ||
                p.NameFa.Contains(s) ||
                (p.NameEn != null && p.NameEn.Contains(s)) ||
                (p.Brand != null && p.Brand.Name.Contains(s)));
        }

        var total = await q.LongCountAsync(cancellationToken);

        var items = await q
            .OrderByDescending(p => p.ProductId)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductListItemDto
            {
                ProductId = p.ProductId,
                SKU = p.SKU,
                NameFa = p.NameFa,
                Brand = p.Brand != null ? p.Brand.Name : null,
                Category = p.Category.Name,
                Unit = p.Unit.Name,
                IsActive = p.IsActive
            })
            .ToListAsync(cancellationToken);

        return new ServiceResponse<PagedResult<ProductListItemDto>>
        {
            Success = true,
            Data = new PagedResult<ProductListItemDto>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                Items = items
            }
        };
    }
}
