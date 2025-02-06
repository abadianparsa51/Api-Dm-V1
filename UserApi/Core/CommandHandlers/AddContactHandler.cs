using AutoMapper;
using MediatR;
using UserApi.Core.Repositories;
using UserApi.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class AddContactHandler : IRequestHandler<AddContactCommand, ServiceResponse<ContactDto>>
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // افزودن IHttpContextAccessor به constructor
    public AddContactHandler(IContactRepository contactRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<ContactDto>> Handle(AddContactCommand request, CancellationToken cancellationToken)
    {
        // استخراج userId از توکن JWT
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return new ServiceResponse<ContactDto>
            {
                Success = false,
                Message = "User ID not found in token."
            };
        }

        // ایجاد یک contact جدید با استفاده از داده‌های دستور
        var contact = new Contact
        {
            Name = request.Name,
            Phone = request.Phone,
            Mail = request.Mail,
            DestinationCardNumber = request.DestinationCardNumber,
            UserId = userId // استفاده از userId که از توکن JWT استخراج شده
        };

        // افزودن contact به repository
        await _contactRepository.AddContactAsync(contact);

        // تبدیل contact به ContactDto
        var contactDto = _mapper.Map<ContactDto>(contact);

        // بازگشت موفقیت همراه با ContactDto
        return new ServiceResponse<ContactDto>
        {
            Success = true,
            Data = contactDto
        };
    }
}
