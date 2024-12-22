using MA.SlotService.Infrastructure.DataAccess.Redis.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MA.SlotService.Infrastructure.DataAccess.Redis.Extensions;

public static class HealthChecksBuilderExtensions
{
    public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetValue<string>($"{RedisConfiguration.Key}:{nameof(RedisConfiguration.ConnectionString)}");
        builder.AddRedis(redisConnectionString);

        return builder;
    }
}