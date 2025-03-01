using MediatR;
using UserApi.Core.Models;

public class VerifyOtpCommand : IRequest<AuthResult>
{
    public string PhoneNumber { get; }
    public string Otp { get; }

    public VerifyOtpCommand(string phoneNumber, string otp)
    {
        PhoneNumber = phoneNumber;
        Otp = otp;
    }
}
