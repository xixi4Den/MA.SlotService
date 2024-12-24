using HealthChecks.UI.Client;
using MA.SlotService.Api.Endpoints;
using MA.SlotService.Application;
using MA.SlotService.Host.Extensions;
using MA.SlotService.Infrastructure.DataAccess.Redis;
using MA.SlotService.Infrastructure.DataAccess.Redis.Extensions;
using MA.SlotService.Infrastructure.Messaging;
using MA.SlotService.Infrastructure.Randomization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices()
    .AddDataAccessServices(builder.Configuration)
    .AddRandomizationServices()
    .AddMessagingServices(builder.Configuration);
    
builder.Services.AddHealthChecks()
    .AddRedisHealthCheck(builder.Configuration);

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: "slot"))
    .AddTraces(builder.Configuration)
    .AddMetrics();

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

app.MapPrometheusScrapingEndpoint();

// app.UseHttpsRedirection();

app.Run();

public partial class Program
{
}