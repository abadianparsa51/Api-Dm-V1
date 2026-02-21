using MediatR;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

namespace UserApi.Core.Queries
{
    /// <summary>
    /// For normal users: get all lookup data needed to create product (dropdowns).
    /// </summary>
    public class GetProductCreateLookupsQuery : IRequest<ServiceResponse<ProductCreateLookupsDto>>
    {
        public bool OnlyActivePriceLists { get; set; } = true;
        public bool OnlyPremiumBrands { get; set; } = false;
    }
}
