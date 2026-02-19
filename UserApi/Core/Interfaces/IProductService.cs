using System.Threading.Tasks;
using UserApi.Core.Models.DTOs;
using UserApi.Core.Queries;
using UserApi.Services;

namespace UserApi.Core.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<ProductDto>> CreateProduct(CreateProductCommand command);
        Task<ServiceResponse<ProductDto>> CreateProductByCode(CreateProductByCodeCommand command);

        Task<ServiceResponse<ProductCreateLookupsDto>> GetCreateLookups(bool onlyActivePriceLists = true, bool onlyPremiumBrands = false);

        Task<ServiceResponse<PagedResult<ProductListItemDto>>> GetProducts(GetProductsQuery query);

        Task<ServiceResponse<ProductDto>> GetById(int productId);

        Task<ServiceResponse<ProductDto>> GetByBarcode(string barcode);
    }
}
