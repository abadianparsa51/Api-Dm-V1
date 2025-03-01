using MediatR;
using UserApi.Core.Models;

public record GetUserByPhoneQuery(string PhoneNumber) : IRequest<ApplicationUser>;
