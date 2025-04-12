using MediatR;
using System.Threading.Tasks;
using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;

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
    }
}
