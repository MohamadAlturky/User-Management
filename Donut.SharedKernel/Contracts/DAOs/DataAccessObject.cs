namespace Donut.SharedKernel.Contracts.DAOs;

public class DataAccessObject<KeyType> : IDataAccessObject<KeyType>
{
    public required KeyType Id { get; set; }
}
