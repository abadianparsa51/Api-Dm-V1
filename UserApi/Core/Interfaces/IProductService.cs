using System.Threading.Tasks;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

namespace UserApi.Core.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<ProductDto>> CreateProduct(CreateProductCommand command);
    }
}
