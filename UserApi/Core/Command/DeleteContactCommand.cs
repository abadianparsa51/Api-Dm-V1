// DeleteContactCommand.cs
using MediatR;
using UserApi.Services;

public class DeleteContactCommand : IRequest<ServiceResponse<bool>>
{
    public int ContactId { get; set; }

    //public DeleteContactCommand(int contactId)
    //{
    //    ContactId = contactId;
    //}
}
