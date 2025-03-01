using MediatR;
using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;
using UserApi.Core.Models;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResult>
{
    private readonly IUserService _userService;

    public LoginUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<AuthResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.LoginUserAsync(request.LoginDto);
    }
}
