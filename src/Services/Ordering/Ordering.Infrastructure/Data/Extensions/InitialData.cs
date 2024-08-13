
namespace Ordering.Infrastructure.Data.Extensions
{
    public class InitialData
    {
        public static IEnumerable<Customer> Customers => new List<Customer> {

        Customer.Create(CustomerId.Of(new Guid("40b9ce16-1e58-49da-8c2b-aaa56f325f26")),"Saad","Saad@gmail.com"),

        Customer.Create(CustomerId.Of(new Guid("980bf3a9-7c60-42a1-8c5b-2efa8ee557cd")),"Mohamed","Mohamed@gmail.com"),

        };






        public static IEnumerable<Product> Products => new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("b6d40124-b573-4416-90cd-3a4c9f00e158")),"Iphone X",5000),
            Product.Create(ProductId.Of(new Guid("ad537cc8-03af-4197-9d6e-9420ec215fc9")),"Huawei Laptop",4600),
            Product.Create(ProductId.Of(new Guid("40a3d884-6eae-47c1-b2ae-3e06b8477d0f")),"Club de nuit intense man",91),
            Product.Create(ProductId.Of(new Guid("cc52588e-1b7f-4076-9e51-b3944992dd63")),"Lattafa Hatem",54),

        };



        public static IEnumerable<Order> OrdersWithItems
        {
            get
            {


                var address1 = Address.Of("Saad", "Elshazly", "Saad@Gmail.com", "Fx", "NX", "Yx", "3456");
                var address2 = Address.Of("Saad", "Elshazly", "Saad@Gmail.com", "Fx", "NX", "Yx", "3456");

                var payment1 = Payment.Of("Saad", "123456789087654267891234", "08/25", "768", 1);
                var payment2 = Payment.Of("Mohamed", "123456789087654267091234", "08/27", "794", 2);
                var order1 = Order.Create(OrderId.Of(Guid.NewGuid())
                      , CustomerId.Of(new Guid("40b9ce16-1e58-49da-8c2b-aaa56f325f26"))
                      , OrderName.Of("ClosesOrder")
                      , shippingAddress: address1
                      , billingAddress: address2
                      , payment: payment1
                      , OrderStatus.Completed);

                order1.Add(ProductId.Of(new Guid("b6d40124-b573-4416-90cd-3a4c9f00e158")), 2, 10000);
                order1.Add(ProductId.Of(new Guid("ad537cc8-03af-4197-9d6e-9420ec215fc9")), 1, 4000);

                var order2 = Order.Create(OrderId.Of(Guid.NewGuid())
                     , CustomerId.Of(new Guid("980bf3a9-7c60-42a1-8c5b-2efa8ee557cd"))
                     , OrderName.Of("PerfumesOrder")
                     , shippingAddress: address1
                     , billingAddress: address2
                     , payment: payment2, OrderStatus.Pending);


                order2.Add(ProductId.Of(new Guid("b6d40124-b573-4416-90cd-3a4c9f00e158")), 1, 5000);
                order2.Add(ProductId.Of(new Guid("ad537cc8-03af-4197-9d6e-9420ec215fc9")), 2, 8000);


                return new List<Order> { order1, order2 };
            }
        }
    }
}
