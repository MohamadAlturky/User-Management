using Donut.SharedKernel.CQRS;

namespace Donut.SharedKernel.Requests;

public interface ICommandAdaptedRequest<TCommand>
    where TCommand : ICommand
{
    TCommand Adapt();
}

public interface ICommandAdaptedRequest<TCommand, TCommandResult>
    where TCommand : ICommand<TCommandResult>
{
    TCommand Adapt();
}
