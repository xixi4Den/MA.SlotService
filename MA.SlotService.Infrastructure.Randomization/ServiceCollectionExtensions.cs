using MA.SlotService.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MA.SlotService.Infrastructure.Randomization;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRandomizationServices(this IServiceCollection services)
    {
        services.AddSingleton<ISpinResultGenerator, SpinResultGenerator>();
        
        return services;
    }
}