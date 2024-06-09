namespace Donut.SharedKernel.Contracts;

public class Entity<KeyType> : IEntity<KeyType>
{
    public required KeyType Id { get; set; }
}
