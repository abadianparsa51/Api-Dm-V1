using MediatR;
using System.Threading.Tasks;

namespace UserApi.Core.Interfaces
{
    public interface IContactCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
    }
}
