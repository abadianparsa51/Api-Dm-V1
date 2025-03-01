using MediatR;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;

public record RegisterUserCommand(UserRegistrationRequestDto UserDto) : IRequest<AuthResult>;
