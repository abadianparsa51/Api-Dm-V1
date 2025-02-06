// UpdateContactCommand.cs
using MediatR;
using UserApi.Services;

public class UpdateContactCommand : IRequest<ServiceResponse<ContactDto>>
{
    public int ContactId { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }

    public UpdateContactCommand(int contactId, string name, string phone)
    {
        ContactId = contactId;
        Name = name;
        Phone = phone;
    }
}
