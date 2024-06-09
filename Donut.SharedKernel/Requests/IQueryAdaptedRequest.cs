using Donut.SharedKernel.CQRS;

namespace Donut.SharedKernel.Requests;

public interface IQueryAdaptedRequest<TQuery, TQueryResult>
    where TQuery : IQuery<TQueryResult>
{
    TQuery Adapt();
}