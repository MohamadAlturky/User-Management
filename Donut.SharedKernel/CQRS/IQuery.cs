using Donut.SharedKernel.Results;
using MediatR;

namespace Donut.SharedKernel.CQRS;


public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
