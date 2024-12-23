using MA.SlotService.Application.Abstractions;
using MA.SlotService.Infrastructure.DataAccess.Redis.Configuration;
using MA.SlotService.Infrastructure.DataAccess.Redis.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MA.SlotService.Infrastructure.DataAccess.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedis(configuration);

        services.AddScoped<ISpinsBalanceRepository, SpinsBalanceRepository>();
        
        return services;
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        AddRedisConfiguration(services, configuration);
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisConfig = sp.GetRequiredService<IOptions<RedisConfiguration>>();
            return ConnectionMultiplexer.Connect(redisConfig.Value.ConnectionString);
        });

        return services;
    }

    private static IServiceCollection AddRedisConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetRequiredSection(RedisConfiguration.Key);
        services.AddOptions<RedisConfiguration>().Bind(configurationSection);

        return services;
    }
}