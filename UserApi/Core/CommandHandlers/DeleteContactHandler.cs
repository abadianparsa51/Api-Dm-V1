
using MediatR;
using UserApi.Core.Interfaces;
using UserApi.Services;
using System.Threading;
using System.Threading.Tasks;

public class DeleteContactHandler : IRequestHandler<DeleteContactCommand, ServiceResponse<bool>>
{
    private readonly IContactCommandRepository _contactCommandRepository;

    public DeleteContactHandler(IContactCommandRepository contactCommandRepository)
    {
        _contactCommandRepository = contactCommandRepository;
    }

    public async Task<ServiceResponse<bool>> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _contactCommandRepository.GetContactByIdAsync(request.ContactId);
        if (contact == null)
        {
            return new ServiceResponse<bool> { Success = false, Message = "Contact not found" };
        }

        await _contactCommandRepository.DeleteContactAsync(request.ContactId);
        return new ServiceResponse<bool> { Data = true, Success = true };
    }
}
