using MediatR;
using UserApi.Core.Models;

public record GetUserByIdQuery(string UserId) : IRequest<ApplicationUser>;
