using MediatR;
using UserApi.Core.Models;

public record GetUserByEmailQuery(string Email) : IRequest<ApplicationUser>;
