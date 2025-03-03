using MediatR;
using UserApi.Core.Interfaces;
using UserApi.Core.Models;
using UserApi.Data.Repositories;
using UserApi.Helpers;

public class VerifyOtpHandler : IRequestHandler<VerifyOtpCommand, AuthResult>
{
    private readonly IUserService _userService;
    private readonly IJwtHelper _jwtHelper;
    private readonly IWalletRepository _walletRepository;

    public VerifyOtpHandler(IUserService userService, IJwtHelper jwtHelper, IWalletRepository walletRepository)
    {
        _userService = userService;
        _jwtHelper = jwtHelper;
        _walletRepository = walletRepository;
    }

    public async Task<AuthResult> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        // Verify OTP validity
        var authResult = await _userService.VerifyOtpAsync(request.PhoneNumber, request.Otp);

        if (!authResult.Result)
        {
            return new AuthResult { Success = false, Message = "Invalid OTP" };
        }

        // Retrieve user information
        var user = await _userService.GetUserByPhoneNumberAsync(request.PhoneNumber);
        if (user == null)
        {
            return new AuthResult { Success = false, Message = "User not found" };
        }

        // Generate JWT token
        var token = _jwtHelper.GenerateJwtToken(user);
        var wallet = await _walletRepository.GetWalletByUserIdAsync(user.Id);
        var walletId = wallet?.Id;
        var walletBalance = wallet?.Balance ?? 0;

        // Return the user information along with the token
        return new AuthResult
        {
            Result = true,
            Token = token,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            WalletId = user.WalletId,
            WalletBalance = user.Wallet?.Balance ?? 0
        };
    }
}
