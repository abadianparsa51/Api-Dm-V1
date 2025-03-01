using MediatR;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;

public record LoginUserCommand(UserLoginRequestDto LoginDto) : IRequest<AuthResult>;
