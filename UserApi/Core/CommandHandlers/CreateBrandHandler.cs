using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;
using UserApi.Data;
using UserApi.Services;

public class CreateBrandHandler : IRequestHandler<CreateBrandCommand, ServiceResponse<LookupItemDto>>
{
    private readonly ApiDbContext _db;

    public CreateBrandHandler(ApiDbContext db)
    {
        _db = db;
    }

    public async Task<ServiceResponse<LookupItemDto>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var name = (request.Name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            return new ServiceResponse<LookupItemDto> { Success = false, Message = "Name is required" };

        var exists = await _db.Brands.AnyAsync(b => b.Name == name, cancellationToken);
        if (exists)
            return new ServiceResponse<LookupItemDto> { Success = false, Message = "Brand already exists" };

        var brand = new Brand
        {
            Name = name,
            Country = string.IsNullOrWhiteSpace(request.Country) ? null : request.Country.Trim(),
            IsPremium = request.IsPremium
        };

        _db.Brands.Add(brand);
        await _db.SaveChangesAsync(cancellationToken);

        return new ServiceResponse<LookupItemDto>
        {
            Success = true,
            Data = new LookupItemDto { Id = brand.BrandId, Title = brand.Name, Code = brand.Country }
        };
    }
}
