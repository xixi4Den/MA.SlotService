using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MA.SlotService.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}