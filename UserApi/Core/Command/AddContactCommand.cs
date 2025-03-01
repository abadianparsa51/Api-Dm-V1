// AddContactCommand.cs
using MediatR;
using UserApi.Services;

public class AddContactCommand : IRequest<ServiceResponse<ContactDto>>
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Mail { get; set; }
    public string DestinationCardNumber { get; set; }
    //public string UserId { get;internal set; }  // Add UserId to the command

    public AddContactCommand(string name, string phone, string mail, string destinationCardNumber)
    {
        Name = name;
        Phone = phone;
        Mail = mail;
        DestinationCardNumber = destinationCardNumber;
        //UserId = userId ;
    }
}
