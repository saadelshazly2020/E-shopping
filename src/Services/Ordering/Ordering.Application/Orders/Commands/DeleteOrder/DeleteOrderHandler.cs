
namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler(IApplicationDBContext dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            //get order from db 
            var orderId = OrderId.Of(command.OrderId);
            var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);
            if (order == null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }
            //delete from db
            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteOrderResult(true);
        }
    }
}
