using Donut.SharedKernel.Builder.Examples.Address;

namespace Donut.SharedKernel.Builder.Examples.Order;

internal class OrderBuilder : IBuilder<Order>
{
    private int _orderId;
    private string _orderName;
    private DateTime _orderCreationDate;
    private readonly AddressBuilder _addressBuilder = AddressBuilder.Empty();
    private OrderBuilder()
    {
        _orderId = 0;
        _orderCreationDate = DateTime.Now;
        _orderName = string.Empty;
    }

    public OrderBuilder WithId(int id)
    {
        _orderId = id;
        return this;
    }
    public OrderBuilder WithName(string name)
    {
        _orderName = name;
        return this;
    }
    public OrderBuilder CreatedOn(DateTime creationDate)
    {
        _orderCreationDate = creationDate;
        return this;
    }

    public OrderBuilder WithShippingAddress(Action<AddressBuilder> action)
    {
        action(_addressBuilder);
        return this;
    }

    public Order Build()
    {
        return new Order()
        {
            CreationDate = _orderCreationDate,
            Id = _orderId,
            Name = _orderName
        };
    }

    public static OrderBuilder Empty()
    {
        return new OrderBuilder();
    }
}
