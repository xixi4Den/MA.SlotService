using MA.SlotService.Api.Contracts;
using MA.SlotService.Application.Features.StartSpin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MA.SlotService.Api.Endpoints;

public static class SlotEndpoints
{
    public static IEndpointRouteBuilder MapSlotEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/slot/spin", async (
                [FromHeader(Name = "UserId")] int userId, IMediator mediator) =>
            {
                var command = new StartSpinCommand(userId);
                var result = await mediator.Send(command);
                return result.IsSuccessful
                    ? Results.Ok(new SpinResultResponse
                    {
                        SpinId = result.Data!.SpinId,
                        Result = result.Data.Result,
                        Balance = result.Balance
                    })
                    : Results.UnprocessableEntity(result.Error);
            }).WithOpenApi()
            .WithTags("Slot")
            .WithSummary("Handles the spinning action")
            .WithDescription("If a player has sufficient spins balance it deducts one spin point and generates a random result for the slot machine reels.")
            .Produces<SpinResultResponse>()
            .Produces(StatusCodes.Status422UnprocessableEntity);

        return endpoints;
    }
}