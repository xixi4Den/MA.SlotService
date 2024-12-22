using MA.SlotService.Api.Contracts;
using MA.SlotService.Application.Features.SpinsBalance;
using MA.SlotService.Application.Features.TopUpSpinsBalance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MA.SlotService.Api.Endpoints;

public static class BalanceTestEndpoints
{
    public static IEndpointRouteBuilder MapBalanceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/balance", async (
            [FromHeader(Name = "UserId")] int userId, IMediator mediator) =>
        {
            var query = new SpinsBalanceQuery(userId);
            var result = await mediator.Send(query);

            return Results.Ok(new SpinsBalanceResponse{Balance = result});
        }).WithOpenApi()
        .WithTags("Balance")
        .WithSummary("Provides player's spins balance")
        .Produces<SpinsBalanceResponse>();

        endpoints.MapPost("/api/balance/{amount}", async (
                [FromHeader(Name = "UserId")] int userId, long amount, IMediator mediator) =>
            {
                var command = new TopUpSpinsBalanceCommand(userId, amount);
                var result = await mediator.Send(command);

                return result.IsSuccessful
                    ? Results.Ok(new SpinsBalanceResponse {Balance = result.Balance!.Value})
                    : Results.BadRequest(result.Error);
            }).WithOpenApi()
            .WithTags("Balance")
            .WithSummary("[test purpose endpoint] Tops up player's spins balance")
            .Produces<SpinsBalanceResponse>()
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}