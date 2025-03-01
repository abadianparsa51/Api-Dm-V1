using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using UserApi.Core.Interfaces;
using UserApi.Core.Repositories;
using UserApi.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper; // Add this line for AutoMapper
    private readonly IMediator _mediator;

    public ContactService(IMediator mediator, IContactRepository contactRepository, IMapper mapper)
    {
        _mediator = mediator;
        _contactRepository = contactRepository;
        _mapper = mapper; // Initialize the mapper
    }

    public async Task<ServiceResponse<ContactDto>> AddContact(AddContactCommand command, string userId)
    {
        // ارسال userId به دستور AddContactCommand
        var addContactCommand = new AddContactCommand(command.Name, command.Phone, command.Mail, command.DestinationCardNumber);
        var response = await _mediator.Send(addContactCommand);

        return response.Success
            ? new ServiceResponse<ContactDto> { Success = true, Data = response.Data }
            : new ServiceResponse<ContactDto> { Success = false, Message = response.Message };
    }

    public async Task<ServiceResponse<ContactDto>> UpdateContact(UpdateContactCommand command)
    {
        return await _mediator.Send(command);
    }

    public async Task<ServiceResponse<bool>> DeleteContact(DeleteContactCommand command)
    {
        return await _mediator.Send(command);
    }
    // Implement GetAllContacts method
    public async Task<List<ContactDto>> GetAllContacts()
    {
        var contacts = await _contactRepository.GetAllContactsAsync(); // Assuming this method exists in the repository
        return contacts.Select(contact => new ContactDto
        {
            Id = contact.Id,
            Name = contact.Name,
            Mail = contact.Mail
        }).ToList();
    }
}
