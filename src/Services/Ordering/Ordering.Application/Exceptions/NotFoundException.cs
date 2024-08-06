using BuildingBlocks.Exceptions;

namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(string entity, object key) : base(entity, key)
        {
        }
        public OrderNotFoundException(object key) : base($"Order {key} is not found")
        {
        }
    }
}
