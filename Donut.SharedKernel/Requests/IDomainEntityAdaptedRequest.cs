using Donut.SharedKernel.Contracts.Entities;
namespace Donut.SharedKernel.Requests;

public interface IDomainEntityAdaptedRequest<TEntity>
    where TEntity : IEntityBase
{
    TEntity Adapt();
}
