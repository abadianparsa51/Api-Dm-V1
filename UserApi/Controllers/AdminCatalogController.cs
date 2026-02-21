using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Core.Models;

namespace UserApi.Controllers
{
    /// <summary>
    /// Fulfillment / Backoffice catalog management.
    /// Operator can CRUD catalog entities that Products depend on.
    /// </summary>
    [ApiController]
    [Route("api/admin/catalog")]
    [Authorize(Roles = "Admin,Operator")]
    public class AdminCatalogController : ControllerBase
    {
        private readonly ApiDbContext _db;
        public AdminCatalogController(ApiDbContext db) => _db = db;

        // -------------------- Lookups for operator UI --------------------
        [HttpGet("lookups")]
        public async Task<IActionResult> GetLookups(CancellationToken ct)
        {
            var categories = await _db.Categories
                .AsNoTracking()
                .OrderBy(x => x.ParentCategoryId).ThenBy(x => x.Name)
                .Select(x => new { x.CategoryId, x.ParentCategoryId, x.Name })
                .ToListAsync(ct);

            var brands = await _db.Brands.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new { x.BrandId, x.Name, x.Country, x.IsPremium })
                .ToListAsync(ct);

            var units = await _db.Units.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new { x.UnitId, x.Name, x.Symbol })
                .ToListAsync(ct);

            var suppliers = await _db.Suppliers.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new { x.SupplierId, x.Name, x.Phone, x.City, x.Address, x.IsImporter })
                .ToListAsync(ct);

            var stores = await _db.Stores.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new { x.StoreId, x.Name, x.Area, x.Address })
                .ToListAsync(ct);

            var priceLists = await _db.PriceLists.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new { x.PriceListId, x.Name, x.Currency, x.IsActive })
                .ToListAsync(ct);

            return Ok(new
            {
                categories,
                brands,
                units,
                suppliers,
                stores,
                priceLists
            });
        }

        // -------------------- Categories --------------------
        [HttpGet("categories")]
        public async Task<IActionResult> Categories(CancellationToken ct)
            => Ok(await _db.Categories.AsNoTracking().OrderBy(x => x.ParentCategoryId).ThenBy(x => x.Name).ToListAsync(ct));

        public record UpsertCategoryDto(string Name, int? ParentCategoryId);

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] UpsertCategoryDto dto, CancellationToken ct)
        {
            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");

            if (dto.ParentCategoryId is not null && !await _db.Categories.AnyAsync(x => x.CategoryId == dto.ParentCategoryId, ct))
                return BadRequest("ParentCategoryId not found");

            var entity = new Category { Name = name, ParentCategoryId = dto.ParentCategoryId };
            _db.Categories.Add(entity);
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpsertCategoryDto dto, CancellationToken ct)
        {
            var entity = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryId == id, ct);
            if (entity == null) return NotFound();

            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");
            if (dto.ParentCategoryId == id) return BadRequest("Category cannot be its own parent");
            if (dto.ParentCategoryId is not null && !await _db.Categories.AnyAsync(x => x.CategoryId == dto.ParentCategoryId, ct))
                return BadRequest("ParentCategoryId not found");

            entity.Name = name;
            entity.ParentCategoryId = dto.ParentCategoryId;
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpDelete("categories/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct)
        {
            if (await _db.Products.AnyAsync(p => p.CategoryId == id, ct))
                return BadRequest("Category is in use by products");

            if (await _db.Categories.AnyAsync(c => c.ParentCategoryId == id, ct))
                return BadRequest("Category has children");

            var entity = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryId == id, ct);
            if (entity == null) return NotFound();

            _db.Categories.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return Ok();
        }

        // -------------------- Brands --------------------
        [HttpGet("brands")]
        public async Task<IActionResult> Brands(CancellationToken ct)
            => Ok(await _db.Brands.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct));

        public record UpsertBrandDto(string Name, string? Country, bool IsPremium);

        [HttpPost("brands")]
        public async Task<IActionResult> CreateBrand([FromBody] UpsertBrandDto dto, CancellationToken ct)
        {
            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");
            if (await _db.Brands.AnyAsync(x => x.Name == name, ct)) return BadRequest("Brand name already exists");

            var entity = new Brand { Name = name, Country = dto.Country?.Trim(), IsPremium = dto.IsPremium };
            _db.Brands.Add(entity);
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpPut("brands/{id:int}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] UpsertBrandDto dto, CancellationToken ct)
        {
            var entity = await _db.Brands.FirstOrDefaultAsync(x => x.BrandId == id, ct);
            if (entity == null) return NotFound();

            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");
            if (await _db.Brands.AnyAsync(x => x.BrandId != id && x.Name == name, ct)) return BadRequest("Brand name already exists");

            entity.Name = name;
            entity.Country = dto.Country?.Trim();
            entity.IsPremium = dto.IsPremium;
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpDelete("brands/{id:int}")]
        public async Task<IActionResult> DeleteBrand(int id, CancellationToken ct)
        {
            if (await _db.Products.AnyAsync(p => p.BrandId == id, ct))
                return BadRequest("Brand is in use by products");

            var entity = await _db.Brands.FirstOrDefaultAsync(x => x.BrandId == id, ct);
            if (entity == null) return NotFound();

            _db.Brands.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return Ok();
        }

        // -------------------- Units --------------------
        [HttpGet("units")]
        public async Task<IActionResult> Units(CancellationToken ct)
            => Ok(await _db.Units.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct));

        public record UpsertUnitDto(string Name, string? Symbol);

        [HttpPost("units")]
        public async Task<IActionResult> CreateUnit([FromBody] UpsertUnitDto dto, CancellationToken ct)
        {
            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");
            if (await _db.Units.AnyAsync(x => x.Name == name, ct)) return BadRequest("Unit name already exists");

            var entity = new Unit { Name = name, Symbol = dto.Symbol?.Trim() };
            _db.Units.Add(entity);
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpPut("units/{id:int}")]
        public async Task<IActionResult> UpdateUnit(int id, [FromBody] UpsertUnitDto dto, CancellationToken ct)
        {
            var entity = await _db.Units.FirstOrDefaultAsync(x => x.UnitId == id, ct);
            if (entity == null) return NotFound();

            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");
            if (await _db.Units.AnyAsync(x => x.UnitId != id && x.Name == name, ct)) return BadRequest("Unit name already exists");

            entity.Name = name;
            entity.Symbol = dto.Symbol?.Trim();
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpDelete("units/{id:int}")]
        public async Task<IActionResult> DeleteUnit(int id, CancellationToken ct)
        {
            if (await _db.Products.AnyAsync(p => p.UnitId == id, ct))
                return BadRequest("Unit is in use by products");

            var entity = await _db.Units.FirstOrDefaultAsync(x => x.UnitId == id, ct);
            if (entity == null) return NotFound();

            _db.Units.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return Ok();
        }

        // -------------------- Suppliers --------------------
        [HttpGet("suppliers")]
        public async Task<IActionResult> Suppliers(CancellationToken ct)
            => Ok(await _db.Suppliers.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct));

        public record UpsertSupplierDto(string Name, string? Phone, string? City, string? Address, bool IsImporter);

        [HttpPost("suppliers")]
        public async Task<IActionResult> CreateSupplier([FromBody] UpsertSupplierDto dto, CancellationToken ct)
        {
            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");
            var entity = new Supplier
            {
                Name = name,
                Phone = dto.Phone?.Trim(),
                City = string.IsNullOrWhiteSpace(dto.City) ? "Tehran" : dto.City.Trim(),
                Address = dto.Address?.Trim(),
                IsImporter = dto.IsImporter
            };
            _db.Suppliers.Add(entity);
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpPut("suppliers/{id:int}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpsertSupplierDto dto, CancellationToken ct)
        {
            var entity = await _db.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id, ct);
            if (entity == null) return NotFound();

            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");

            entity.Name = name;
            entity.Phone = dto.Phone?.Trim();
            entity.City = string.IsNullOrWhiteSpace(dto.City) ? entity.City : dto.City.Trim();
            entity.Address = dto.Address?.Trim();
            entity.IsImporter = dto.IsImporter;
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpDelete("suppliers/{id:int}")]
        public async Task<IActionResult> DeleteSupplier(int id, CancellationToken ct)
        {
            if (await _db.InventoryLots.AnyAsync(l => l.SupplierId == id, ct))
                return BadRequest("Supplier is in use by inventory lots");

            var entity = await _db.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id, ct);
            if (entity == null) return NotFound();

            _db.Suppliers.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return Ok();
        }

        // -------------------- Stores --------------------
        [HttpGet("stores")]
        public async Task<IActionResult> Stores(CancellationToken ct)
            => Ok(await _db.Stores.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct));

        public record UpsertStoreDto(string Name, string? Area, string? Address);

        [HttpPost("stores")]
        public async Task<IActionResult> CreateStore([FromBody] UpsertStoreDto dto, CancellationToken ct)
        {
            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");

            var entity = new Store { Name = name, Area = dto.Area?.Trim(), Address = dto.Address?.Trim() };
            _db.Stores.Add(entity);
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpPut("stores/{id:int}")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] UpsertStoreDto dto, CancellationToken ct)
        {
            var entity = await _db.Stores.FirstOrDefaultAsync(x => x.StoreId == id, ct);
            if (entity == null) return NotFound();

            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");

            entity.Name = name;
            entity.Area = dto.Area?.Trim();
            entity.Address = dto.Address?.Trim();
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpDelete("stores/{id:int}")]
        public async Task<IActionResult> DeleteStore(int id, CancellationToken ct)
        {
            if (await _db.InventoryLots.AnyAsync(l => l.StoreId == id, ct))
                return BadRequest("Store is in use by inventory lots");

            var entity = await _db.Stores.FirstOrDefaultAsync(x => x.StoreId == id, ct);
            if (entity == null) return NotFound();

            _db.Stores.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return Ok();
        }

        // -------------------- PriceLists --------------------
        [HttpGet("pricelists")]
        public async Task<IActionResult> PriceLists(CancellationToken ct)
            => Ok(await _db.PriceLists.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct));

        public record UpsertPriceListDto(string Name, string? Currency, bool IsActive);

        [HttpPost("pricelists")]
        public async Task<IActionResult> CreatePriceList([FromBody] UpsertPriceListDto dto, CancellationToken ct)
        {
            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");
            if (await _db.PriceLists.AnyAsync(x => x.Name == name, ct)) return BadRequest("PriceList name already exists");

            var entity = new PriceList { Name = name, Currency = string.IsNullOrWhiteSpace(dto.Currency) ? "IRR" : dto.Currency.Trim(), IsActive = dto.IsActive };
            _db.PriceLists.Add(entity);
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpPut("pricelists/{id:int}")]
        public async Task<IActionResult> UpdatePriceList(int id, [FromBody] UpsertPriceListDto dto, CancellationToken ct)
        {
            var entity = await _db.PriceLists.FirstOrDefaultAsync(x => x.PriceListId == id, ct);
            if (entity == null) return NotFound();

            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");
            if (await _db.PriceLists.AnyAsync(x => x.PriceListId != id && x.Name == name, ct)) return BadRequest("PriceList name already exists");

            entity.Name = name;
            entity.Currency = string.IsNullOrWhiteSpace(dto.Currency) ? entity.Currency : dto.Currency.Trim();
            entity.IsActive = dto.IsActive;
            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        [HttpDelete("pricelists/{id:int}")]
        public async Task<IActionResult> DeletePriceList(int id, CancellationToken ct)
        {
            if (await _db.ProductPrices.AnyAsync(p => p.PriceListId == id, ct))
                return BadRequest("PriceList is in use by product prices");

            var entity = await _db.PriceLists.FirstOrDefaultAsync(x => x.PriceListId == id, ct);
            if (entity == null) return NotFound();

            _db.PriceLists.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return Ok();
        }
    }
}
