using MediatR;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

public class CreateUnitCommand : IRequest<ServiceResponse<LookupItemDto>>
{
    public string Name { get; set; } = null!;
    public string? Symbol { get; set; }
}
