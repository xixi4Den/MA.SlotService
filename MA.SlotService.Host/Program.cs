using HealthChecks.UI.Client;
using MA.SlotService.Api.Endpoints;
using MA.SlotService.Application;
using MA.SlotService.Infrastructure.DataAccess.Redis;
using MA.SlotService.Infrastructure.DataAccess.Redis.Extensions;
using MA.SlotService.Infrastructure.Messaging;
using MA.SlotService.Infrastructure.Randomization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices()
    .AddDataAccessServices(builder.Configuration)
    .AddRandomizationServices()
    .AddMessagingServices(builder.Configuration);
    
builder.Services.AddHealthChecks()
    .AddRedisHealthCheck(builder.Configuration);

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app
    .MapSlotEndpoints()
    .MapBalanceEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.Run();