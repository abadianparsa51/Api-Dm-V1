using MediatR;

public record GenerateOtpCommand(string PhoneNumber) : IRequest<string>;
