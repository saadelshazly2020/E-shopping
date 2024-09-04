
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
            Product.Create(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")), "IPhone X", 500),
            Product.Create(ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), "Samsung 10", 400),
            Product.Create(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), "Huawei Plus", 650),
            Product.Create(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), "Xiaomi Mi", 450)

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

                order1.Add(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")), 2, 10000);
                order1.Add(ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), 1, 4000);

                var order2 = Order.Create(OrderId.Of(Guid.NewGuid())
                     , CustomerId.Of(new Guid("980bf3a9-7c60-42a1-8c5b-2efa8ee557cd"))
                     , OrderName.Of("PerfumesOrder")
                     , shippingAddress: address1
                     , billingAddress: address2
                     , payment: payment2, OrderStatus.Pending);


                order2.Add(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), 1, 5000);
                order2.Add(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), 2, 8000);


                return new List<Order> { order1, order2 };
            }
        }
    }
}
