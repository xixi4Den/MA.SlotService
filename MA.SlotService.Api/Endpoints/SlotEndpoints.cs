using MA.SlotService.Api.Contracts;
using MA.SlotService.Application.Features.StartSpin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MA.SlotService.Api.Endpoints;

public static class SlotEndpoints
{
    public static void MapSlotEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/slot/spin", async (
            [FromHeader(Name = "UserId")]int userId, IMediator mediator) =>
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
                : Results.BadRequest(result.Error);
        });
    }
}