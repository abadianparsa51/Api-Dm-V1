using System.Threading;
using System.Threading.Tasks;
using UserApi.Core.Models;

namespace UserApi.Core.Repositories
{
    public interface IProductRepository
    {
        Task<bool> SkuExistsAsync(string sku, CancellationToken ct = default);
        Task<Product> CreateProductGraphAsync(Product product, CancellationToken ct = default);

    }
}
