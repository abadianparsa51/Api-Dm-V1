using MediatR;
using System.Threading.Tasks;
using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;
using UserApi.Core.Queries;

namespace UserApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IMediator _mediator;

        public ProductService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<ServiceResponse<ProductDto>> CreateProduct(CreateProductCommand command)
            => _mediator.Send(command);
        public Task<ServiceResponse<ProductDto>> CreateProductByCode(CreateProductByCodeCommand command)
            => _mediator.Send(command);

        public Task<ServiceResponse<ProductCreateLookupsDto>> GetCreateLookups(bool onlyActivePriceLists = true, bool onlyPremiumBrands = false)
            => _mediator.Send(new GetProductCreateLookupsQuery
            {
                OnlyActivePriceLists = onlyActivePriceLists,
                OnlyPremiumBrands = onlyPremiumBrands
            });

        public Task<ServiceResponse<PagedResult<ProductListItemDto>>> GetProducts(GetProductsQuery query)
            => _mediator.Send(query);

        public Task<ServiceResponse<ProductDto>> GetById(int productId)
            => _mediator.Send(new GetProductByIdQuery(productId));

        public Task<ServiceResponse<ProductDto>> GetByBarcode(string barcode)
            => _mediator.Send(new GetProductByBarcodeQuery(barcode));
    }
}
