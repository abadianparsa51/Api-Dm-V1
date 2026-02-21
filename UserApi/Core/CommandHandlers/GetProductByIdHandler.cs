using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using UserApi.Core.Models.DTOs;
using UserApi.Core.Queries;
using UserApi.Data;
using UserApi.Services;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ServiceResponse<ProductDto>>
{
    private readonly ApiDbContext _db;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(ApiDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.ProductId <= 0)
            return new ServiceResponse<ProductDto> { Success = false, Message = "Invalid ProductId" };

        var product = await _db.Products
            .AsNoTracking()
            .Include(p => p.Barcodes)
            .Include(p => p.Prices)
            .Include(p => p.InventoryLots)
            .FirstOrDefaultAsync(p => p.ProductId == request.ProductId, cancellationToken);

        if (product == null)
            return new ServiceResponse<ProductDto> { Success = false, Message = "Product not found" };

        var dto = _mapper.Map<ProductDto>(product);
        return new ServiceResponse<ProductDto> { Success = true, Data = dto };
    }
}
