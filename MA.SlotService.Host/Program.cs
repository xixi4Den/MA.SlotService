using MA.SlotService.Api.Endpoints;
using MA.SlotService.Application;
using MA.SlotService.Infrastructure.DataAccess.Redis;
using MA.SlotService.Infrastructure.Randomization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices()
    .AddDataAccessServices(builder.Configuration)
    .AddRandomizationServices();

var app = builder.Build();

app.MapSlotEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();