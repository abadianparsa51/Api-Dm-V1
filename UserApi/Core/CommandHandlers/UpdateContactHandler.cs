// UpdateContactHandler.cs
using MediatR;
using UserApi.Core.Interfaces;
using UserApi.Services;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

public class UpdateContactHandler : IRequestHandler<UpdateContactCommand, ServiceResponse<ContactDto>>
{
    private readonly IContactCommandRepository _contactCommandRepository;
    private readonly IMapper _mapper;

    public UpdateContactHandler(IContactCommandRepository contactCommandRepository, IMapper mapper)
    {
        _contactCommandRepository = contactCommandRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<ContactDto>> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _contactCommandRepository.GetContactByIdAsync(request.ContactId);
        if (contact == null)
        {
            return new ServiceResponse<ContactDto> { Success = false, Message = "Contact not found" };
        }

        contact.Name = request.Name;
        contact.Phone = request.Phone;

        await _contactCommandRepository.UpdateContactAsync(contact);
        var response = _mapper.Map<ContactDto>(contact);

        return new ServiceResponse<ContactDto> { Data = response, Success = true };
    }
}
