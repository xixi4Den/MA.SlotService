using MA.SlotService.Application.Abstractions;
using MA.SlotService.Infrastructure.Messaging.Configuration;
using MA.SlotService.Infrastructure.Messaging.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MA.SlotService.Infrastructure.Messaging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitConfig = services.AddRabbitConfiguration(configuration);
        
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<SpinPointsRewardGrantedEventConsumer>();
            
            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitConfig.Host, "/", h =>
                {
                    h.Username(rabbitConfig.Username);
                    h.Password(rabbitConfig.Password);
                });
                
                cfg.ConfigureEndpoints(context);
            });
            
            busConfigurator.AddConfigureEndpointsCallback((name, cfg) =>
            {
                if (cfg is IRabbitMqReceiveEndpointConfigurator rmq)
                    rmq.SingleActiveConsumer = true;
            });
        });
        
        services.AddScoped<IEventPublisher, EventPublisher>();
        
        return services;
    }
    
    private static RabbitConfiguration AddRabbitConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetRequiredSection(RabbitConfiguration.Key);
        services.AddOptions<RabbitConfiguration>().Bind(configurationSection);
        var result = configurationSection.Get<RabbitConfiguration>();
        
        return result!;
    }
}