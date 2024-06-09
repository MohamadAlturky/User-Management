namespace Donut.SharedKernel.Contracts.DAOs;

public interface IDataAccessObject<KeyType>
{
    KeyType Id { get; set; }
}
