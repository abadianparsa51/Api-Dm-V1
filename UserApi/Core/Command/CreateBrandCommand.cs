using MediatR;
using UserApi.Core.Models.DTOs;
using UserApi.Services;

public class CreateBrandCommand : IRequest<ServiceResponse<LookupItemDto>>
{
    public string Name { get; set; } = null!;
    public string? Country { get; set; }
    public bool IsPremium { get; set; } = true;
}
