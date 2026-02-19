using MediatR;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

namespace UserApi.Core.Queries
{
    public class GetProductsQuery : IRequest<ServiceResponse<PagedResult<ProductListItemDto>>>
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool IncludeInactive { get; set; } = false;
    }

    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
