using MediatR;
using Donut.SharedKernel.Results;

namespace Donut.SharedKernel.CQRS;


public interface ICommand : IRequest<Result> { }

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }