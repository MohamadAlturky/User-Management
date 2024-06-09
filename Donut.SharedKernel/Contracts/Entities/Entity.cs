namespace Donut.SharedKernel.Contracts.Entities;

public class Entity<KeyType> : IEntity<KeyType>, IEntityBase
{
    public required KeyType Id { get; set; }
}
