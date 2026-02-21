using MediatR;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

namespace UserApi.Core.Queries
{
    public class GetProductByBarcodeQuery : IRequest<ServiceResponse<ProductDto>>
    {
        public string Barcode { get; }

        public GetProductByBarcodeQuery(string barcode)
        {
            Barcode = barcode;
        }
    }
}
