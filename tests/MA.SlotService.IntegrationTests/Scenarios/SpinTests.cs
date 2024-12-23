using System.Net;
using FluentAssertions;
using MA.SlotService.IntegrationTests.Extensions;
using MA.SlotService.IntegrationTests.Framework;

namespace MA.SlotService.IntegrationTests.Scenarios;

[TestClass]
public class SpinTests
{
    private static IntegrationTestingWebAppFactory _webAppFactory;
    private HttpClient _httpClient;

    [ClassInitialize]
    public static void GlobalInit(TestContext context)
    {
        _webAppFactory = new IntegrationTestingWebAppFactory();
    }
    
    [TestInitialize]
    public async Task Init()
    {
        _httpClient = _webAppFactory.CreateDefaultClient();
    }
    
    [TestCleanup]
    public void Cleanup() => _httpClient.Dispose();

    [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
    public static void GlobalCleanup() => _webAppFactory.Dispose();
    
    [TestMethod]
    public async Task PlayerShouldSpinAsLongAsBalanceGreaterThanZero()
    {
        var userId = 13;
        (await _httpClient.TopUpBalance(userId, 3)).Balance.Should().Be(3);

        (await _httpClient.Spin(userId, HttpStatusCode.OK))!.Balance.Should().Be(2);
        (await _httpClient.GetBalance(userId)).Balance.Should().Be(2);
        
        (await _httpClient.Spin(userId, HttpStatusCode.OK))!.Balance.Should().Be(1);
        (await _httpClient.GetBalance(userId)).Balance.Should().Be(1);
        
        (await _httpClient.Spin(userId, HttpStatusCode.OK))!.Balance.Should().Be(0);
        (await _httpClient.GetBalance(userId)).Balance.Should().Be(0);
        
        await _httpClient.Spin(userId, HttpStatusCode.UnprocessableEntity);
        (await _httpClient.GetBalance(userId)).Balance.Should().Be(0);
        
        (await _httpClient.TopUpBalance(userId, 10)).Balance.Should().Be(10);
        
        await _httpClient.Spin(userId, HttpStatusCode.OK);
        (await _httpClient.GetBalance(userId)).Balance.Should().Be(9);
    }
    
    
    [TestMethod]
    public async Task ConcurrentEnvironment_ShouldDeductPointsInAtomicWay()
    {
        var userId = 20;
        
        // 10 spin points - 10 concurrent spins
        (await _httpClient.TopUpBalance(userId, 10)).Balance.Should().Be(10);

        var spinTasks = Enumerable.Range(1, 10).Select(_ => _httpClient.Spin(userId, HttpStatusCode.OK));
        await Task.WhenAll(spinTasks);
        
        (await _httpClient.GetBalance(userId)).Balance.Should().Be(0);
        
        // 10 spin points - 20 concurrent spins
        (await _httpClient.TopUpBalance(userId, 10)).Balance.Should().Be(10);

        var spinTasks2 = Enumerable.Range(1, 20).Select(_ => _httpClient.Spin(userId));
        await Task.WhenAll(spinTasks2);
        
        (await _httpClient.GetBalance(userId)).Balance.Should().Be(0);
    }
}