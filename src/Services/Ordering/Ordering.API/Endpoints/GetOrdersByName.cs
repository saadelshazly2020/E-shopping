
using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Endpoints;

public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);
public class GetOrdersByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
        {
            //send request to mediator handler
            var result = await sender.Send(new GetOrdersByNameQuery(orderName));
            var response = result.Adapt<GetOrdersByNameResponse>();
            Results.Ok(response);

        }).Produces<GetOrdersByNameResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Get Orders By Name")
          .WithDescription("Get Orders By Name");
    }
}

