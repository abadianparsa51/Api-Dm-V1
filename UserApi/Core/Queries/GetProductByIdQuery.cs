using MediatR;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

namespace UserApi.Core.Queries
{
    public class GetProductByIdQuery : IRequest<ServiceResponse<ProductDto>>
    {
        public int ProductId { get; }

        public GetProductByIdQuery(int productId)
        {
            ProductId = productId;
        }
    }
}
