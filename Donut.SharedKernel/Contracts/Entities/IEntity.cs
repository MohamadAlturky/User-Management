namespace Donut.SharedKernel.Contracts.Entities;

public interface IEntity<KeyType>
{
    KeyType Id { get; set; }
}
