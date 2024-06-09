namespace Donut.SharedKernel.Contracts;

public interface IEntity<KeyType>
{
    KeyType Id { get; set; }
}
