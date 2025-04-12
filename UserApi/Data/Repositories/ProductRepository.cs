using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using UserApi.Core.Models;
using UserApi.Core.Repositories;

namespace UserApi.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApiDbContext _db;

        public ProductRepository(ApiDbContext db)
        {
            _db = db;
        }

        public Task<bool> SkuExistsAsync(string sku, CancellationToken ct = default)
            => _db.Products.AnyAsync(p => p.SKU == sku, ct);

        public async Task<Product> CreateProductGraphAsync(Product product, CancellationToken ct = default)
        {
            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            _db.Products.Add(product);
            await _db.SaveChangesAsync(ct);

            await tx.CommitAsync(ct);
            return product;
        }
    }
}
