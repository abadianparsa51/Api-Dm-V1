using UserApi.Core.Interfaces;
using UserApi.Core.Models;
using UserApi.Core.Models.DTOs;
using UserApi.Helpers;

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHelper _jwtHelper;

        public UserService(IUserRepository userRepository, IJwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
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

            var token = _jwtHelper.GenerateJwtToken(newUser);
            return new AuthResult { Token = token, Email = userDto.Email, PhoneNumber = userDto.PhoneNumber, Result = true }; // ارسال شماره تلفن در پاسخ
        }

        public async Task<AuthResult> LoginUserAsync(UserLoginRequestDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !await _userRepository.CheckPasswordAsync(user, loginDto.Password))
                return new AuthResult { Result = false, Errors = new List<string> { "Invalid credentials." } };

            var token = _jwtHelper.GenerateJwtToken(user);
            return new AuthResult { Token = token, Email = user.Email, PhoneNumber = user.PhoneNumber, Result = true }; // ارسال شماره تلفن در پاسخ
        }
        public async Task<ApplicationUser> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            // Use IJwtHelper to generate the JWT token
            return _jwtHelper.GenerateJwtToken(user);
        }
    }
}
