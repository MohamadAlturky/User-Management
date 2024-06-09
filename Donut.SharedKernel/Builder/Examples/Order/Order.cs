namespace Donut.SharedKernel.Builder.Examples.Order;

internal class Order
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public Donut.SharedKernel.Builder.Examples.Address.Address ShippingAddress { get; set; } = null!;
}
