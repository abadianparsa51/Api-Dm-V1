using MediatR;

public class GenerateOtpHandler : IRequestHandler<GenerateOtpCommand, string>
{
    private readonly IUserService _userService;

    public GenerateOtpHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(GenerateOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByPhoneNumberAsync(request.PhoneNumber);
        if (user == null)
            throw new Exception("User not found."); // یا می‌توانید هندل بهتری برای این خطا داشته باشید

        return await _userService.GenerateOtpAsync(request.PhoneNumber, user.Id);
    }
}
