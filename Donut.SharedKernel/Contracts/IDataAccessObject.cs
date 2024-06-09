namespace Donut.SharedKernel.Contracts;

public interface IDataAccessObject<KeyType>
{
    KeyType Id { get; set; }
}
