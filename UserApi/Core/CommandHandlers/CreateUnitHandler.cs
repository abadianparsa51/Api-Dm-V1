using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;
using UserApi.Data;
using UserApi.Services;
using Unit = UserApi.Core.Models.Unit;

public class CreateUnitHandler : IRequestHandler<CreateUnitCommand, ServiceResponse<LookupItemDto>>
{
    private readonly ApiDbContext _db;

    public CreateUnitHandler(ApiDbContext db)
    {
        _db = db;
    }

    public async Task<ServiceResponse<LookupItemDto>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var name = (request.Name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            return new ServiceResponse<LookupItemDto> { Success = false, Message = "Name is required" };

        var exists = await _db.Units.AnyAsync(u => u.Name == name, cancellationToken);
        if (exists)
            return new ServiceResponse<LookupItemDto> { Success = false, Message = "Unit already exists" };

        var unit = new Unit
        {
            Name = name,
            Symbol = string.IsNullOrWhiteSpace(request.Symbol) ? null : request.Symbol.Trim()
        };

        _db.Units.Add(unit);
        await _db.SaveChangesAsync(cancellationToken);

        return new ServiceResponse<LookupItemDto>
        {
            Success = true,
            Data = new LookupItemDto { Id = unit.UnitId, Title = unit.Name, Code = unit.Symbol }
        };
    }
}
