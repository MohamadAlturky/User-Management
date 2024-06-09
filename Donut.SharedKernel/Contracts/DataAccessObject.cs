namespace Donut.SharedKernel.Contracts;

public class DataAccessObject<KeyType> : IDataAccessObject<KeyType>
{
    public required KeyType Id { get; set; }
}
