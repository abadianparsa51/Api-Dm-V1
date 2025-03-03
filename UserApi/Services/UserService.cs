using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;
using UserApi.Core.Models;
using UserApi.Helpers;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtHelper _jwtHelper;
    private readonly IWalletRepository _walletRepository;
    private readonly IOtpService _otpService;

    public UserService(IUserRepository userRepository, IJwtHelper jwtHelper, IWalletRepository walletRepository, IOtpService otpService)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
        _walletRepository = walletRepository;
        _otpService = otpService;
    }

    public async Task<AuthResult> RegisterUserAsync(UserRegistrationRequestDto userDto)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
        if (existingUser != null)
            return new AuthResult { Result = false, Errors = new List<string> { "Email already exists." } };

        var newUser = new ApplicationUser
        {
            Email = userDto.Email,
            UserName = userDto.Email,
            PhoneNumber = userDto.PhoneNumber  // اضافه کردن شماره تلفن
        };

        var isCreated = await _userRepository.CreateUserAsync(newUser, userDto.Password);

        if (!isCreated)
            return new AuthResult { Result = false, Errors = new List<string> { "Failed to register user." } };

        // ایجاد کیف پول جدید برای کاربر
        var newWallet = new Wallet { UserId = newUser.Id };
        await _walletRepository.CreateWalletAsync(newWallet);

        var token = _jwtHelper.GenerateJwtToken(newUser);
        return new AuthResult
        {
            Token = token,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            WalletId = newWallet.Id,
            WalletBalance = newWallet.Balance,
            Result = true
        };
    }

    public async Task<AuthResult> LoginUserAsync(UserLoginRequestDto loginDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
        if (user == null || !await _userRepository.CheckPasswordAsync(user, loginDto.Password))
            return new AuthResult { Result = false, Errors = new List<string> { "Invalid credentials." } };

        var token = _jwtHelper.GenerateJwtToken(user);

        var wallet = await _walletRepository.GetWalletByUserIdAsync(user.Id);
        var walletId = wallet?.Id;
        var walletBalance = wallet?.Balance ?? 0;

        return new AuthResult
        {
            Token = token,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            WalletId = walletId,
            WalletBalance = walletBalance,
            Result = true
        };
    }

    public async Task<ApplicationUser> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return await _userRepository.GetUserByPhoneAsync(phoneNumber);
    }

    public string GenerateJwtToken(ApplicationUser user)
    {
        return _jwtHelper.GenerateJwtToken(user);
    }

    public async Task<string> GenerateOtpAsync(string phoneNumber ,string userId)
    {
        return await _otpService.GenerateOtpAsync(phoneNumber,userId);
    }
    public async Task<AuthResult> VerifyOtpAsync(string phoneNumber, string otp)
    {
        var user = await _userRepository.GetUserByPhoneAsync(phoneNumber);
        if (user == null)
            return new AuthResult { Result = false, Errors = new List<string> { "User not found." } };

        bool isOtpValid = await _otpService.VerifyOtpAsync(user.Id, otp);
        if (!isOtpValid)
        {
            return new AuthResult { Result = false, Errors = new List<string> { "Invalid OTP." } };
        }

        return new AuthResult { Result = true };
    }
}
