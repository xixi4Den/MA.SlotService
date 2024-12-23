using MA.SlotService.Application.Abstractions;
using MA.SlotService.Infrastructure.Messaging.Consumers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MA.SlotService.IntegrationTests.Framework;

public class IntegrationTestingWebAppFactory : WebApplicationFactory<Program>
{
    private static Mock<IEventPublisher> _eventPublisherMock;
    private IConfiguration _configuration;

    public Mock<IEventPublisher> EventDispatcherMock => _eventPublisherMock;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");

        var redisConnectionString = TestContainerHelper.RedisConnectionString;
        builder.ConfigureAppConfiguration((context, conf) =>
        {
            conf.AddInMemoryCollection([
                new KeyValuePair<string, string?>("Redis:ConnectionString", redisConnectionString)
            ]);
            _configuration = conf.Build();
        });

        builder.ConfigureTestServices((services) =>
        {
            services.AddTransient<SpinPointsRewardGrantedEventConsumer>();
            MockEventPublisher(services);
        });
    }
    
    private static void MockEventPublisher(IServiceCollection services)
    {
        _eventPublisherMock = new Mock<IEventPublisher>();
        services.AddTransient(typeof(IEventPublisher), sp => _eventPublisherMock.Object);
    }
}