using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;

namespace UserApi.Controllers
{
    /// <summary>
    /// Public catalog APIs for the website/app (end-users).
    /// End-user does NOT need to know relational entities; they just read the catalog.
    /// </summary>
    [ApiController]
    [Route("api/catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly ApiDbContext _db;
        public CatalogController(ApiDbContext db) => _db = db;

        [HttpGet("categories")]
        public async Task<IActionResult> Categories(CancellationToken ct)
        {
            var items = await _db.Categories.AsNoTracking()
                .OrderBy(x => x.ParentCategoryId).ThenBy(x => x.Name)
                .Select(x => new { x.CategoryId, x.ParentCategoryId, x.Name })
                .ToListAsync(ct);
            return Ok(items);
        }

        /// <summary>
        /// Products list for end-users.
        /// - only active products
        /// - returns resolved labels (category/brand/unit)
        /// - returns one active price for a selected pricelist (default: Regular if exists, else first active)
        /// - returns total qty on hand (sum of lots) optionally filtered by storeId
        /// </summary>
        [HttpGet("products")]
        public async Task<IActionResult> Products(
            [FromQuery] string? q,
            [FromQuery] int? categoryId,
            [FromQuery] int? brandId,
            [FromQuery] int? storeId,
            [FromQuery] int? priceListId,
            CancellationToken ct)
        {
            // Choose a default pricelist
            int? plId = priceListId;
            if (plId is null)
            {
                plId = await _db.PriceLists.AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.Name == "Regular")
                    .ThenBy(x => x.PriceListId)
                    .Select(x => (int?)x.PriceListId)
                    .FirstOrDefaultAsync(ct);
            }

            var query = _db.Products.AsNoTracking()
                .Where(p => p.IsActive)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var s = q.Trim();
                query = query.Where(x => x.SKU.Contains(s) || x.NameFa.Contains(s) || (x.NameEn != null && x.NameEn.Contains(s)));
            }
            if (categoryId is not null) query = query.Where(x => x.CategoryId == categoryId);
            if (brandId is not null) query = query.Where(x => x.BrandId == brandId);

            var items = await query
                .Select(p => new
                {
                    p.ProductId,
                    p.SKU,
                    p.NameFa,
                    p.NameEn,
                    Category = new { p.CategoryId, Name = p.Category.Name },
                    Brand = p.BrandId != null ? new { p.BrandId, Name = p.Brand.Name } : null,
                    Unit = new { p.UnitId, Name = p.Unit.Name, p.Unit.Symbol },
                    p.NetWeightValue,
                    p.NetWeightUnit,
                    p.IsPerishable,
                    p.MinStockAlert,
                    // active price for selected pricelist
                    Price = plId == null
                        ? null
                        : _db.ProductPrices
                            .Where(pp => pp.ProductId == p.ProductId && pp.PriceListId == plId && pp.ValidTo == null)
                            .OrderByDescending(pp => pp.ValidFrom)
                            .Select(pp => (long?)pp.PriceIRR)
                            .FirstOrDefault(),
                    QtyOnHand = _db.InventoryLots
                        .Where(l => l.ProductId == p.ProductId && (storeId == null || l.StoreId == storeId))
                        .Select(l => (decimal?)l.QtyOnHand)
                        .Sum() ?? 0m
                })
                .ToListAsync(ct);

            return Ok(new { priceListId = plId, items });
        }

        [HttpGet("products/{id:int}")]
        public async Task<IActionResult> ProductDetails(int id, [FromQuery] int? priceListId, [FromQuery] int? storeId, CancellationToken ct)
        {
            var p = await _db.Products.AsNoTracking()
                .Where(x => x.IsActive && x.ProductId == id)
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Unit)
                .Include(x => x.Barcodes)
                .FirstOrDefaultAsync(ct);

            if (p == null) return NotFound();

            int? plId = priceListId;
            if (plId is null)
            {
                plId = await _db.PriceLists.AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.Name == "Regular")
                    .ThenBy(x => x.PriceListId)
                    .Select(x => (int?)x.PriceListId)
                    .FirstOrDefaultAsync(ct);
            }

            var price = plId == null ? null : await _db.ProductPrices.AsNoTracking()
                .Where(pp => pp.ProductId == id && pp.PriceListId == plId && pp.ValidTo == null)
                .OrderByDescending(pp => pp.ValidFrom)
                .Select(pp => (long?)pp.PriceIRR)
                .FirstOrDefaultAsync(ct);

            var qty = await _db.InventoryLots.AsNoTracking()
                .Where(l => l.ProductId == id && (storeId == null || l.StoreId == storeId))
                .Select(l => (decimal?)l.QtyOnHand)
                .SumAsync(ct) ?? 0m;

            return Ok(new
            {
                p.ProductId,
                p.SKU,
                p.NameFa,
                p.NameEn,
                Category = new { p.CategoryId, Name = p.Category.Name },
                Brand = p.BrandId != null ? new { p.BrandId, Name = p.Brand.Name } : null,
                Unit = new { p.UnitId, Name = p.Unit.Name, p.Unit.Symbol },
                p.NetWeightValue,
                p.NetWeightUnit,
                p.IsPerishable,
                p.MinStockAlert,
                PriceListId = plId,
                PriceIRR = price,
                QtyOnHand = qty,
                Barcodes = p.Barcodes.OrderByDescending(b => b.IsPrimary).Select(b => new { b.Barcode, b.IsPrimary })
            });
        }
    }
}
