using Donut.SharedKernel.Builder.Examples.Order;

namespace Donut.SharedKernel.Builder.Examples.Demo;

internal class OrderBuilderDemo
{
    void Demo()
    {
        Donut.SharedKernel.Builder.Examples.Order.Order order = OrderBuilder.Empty()
                                  .WithId(2)
                                  .WithName("order-5")
                                  .CreatedOn(DateTime.Now)
                                  .WithShippingAddress(addressBuilder =>
                                                       addressBuilder.Street("Barza")
                                                                     .City("damas")
                                  )
                                  .Build();
    }
}
