
namespace Ordering.API.Endpoints;
public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);
public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            //map from request to command
            var command = request.Adapt<CreateOrderCommand>();
            //handle creation by mediator
            var result = sender.Send(command);
            //map result to CreateOrderResponse
            var response = result.Adapt<CreateOrderResponse>();

            return Results.Created($"/orders/{response.Id}", response);
        }).WithName("CreateOrder")
          .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Create Order")
          .WithDescription("Create Order");
    }
}

