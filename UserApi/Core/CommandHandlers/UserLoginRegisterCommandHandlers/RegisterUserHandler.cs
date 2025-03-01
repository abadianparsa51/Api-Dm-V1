using MediatR;
using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;
using UserApi.Core.Models;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResult>
{
    private readonly IUserService _userService;

    public RegisterUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RegisterUserAsync(request.UserDto);
    }
}
