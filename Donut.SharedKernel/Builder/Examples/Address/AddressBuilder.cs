namespace Donut.SharedKernel.Builder.Examples.Address;

internal class AddressBuilder : IBuilder<Address>
{
    private string _city;
    private string _street;
    private AddressBuilder()
    {
        _city = string.Empty;
        _street = string.Empty;
    }
    public static AddressBuilder Empty()
    {
        return new AddressBuilder();
    }

    public Address Build()
    {
        return new Address()
        {
            City = _city,
            Street = _street,
        };
    }

    public AddressBuilder City(string city)
    {
        _city = city;
        return this;
    }

    public AddressBuilder Street(string street)
    {
        _street = street;
        return this;
    }
}
