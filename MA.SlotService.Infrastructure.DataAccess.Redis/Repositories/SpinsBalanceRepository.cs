using MA.SlotService.Application.Abstractions;
using StackExchange.Redis;

namespace MA.SlotService.Infrastructure.DataAccess.Redis.Repositories;

public class SpinsBalanceRepository: ISpinsBalanceRepository
{
    private readonly IConnectionMultiplexer _redis;

    public SpinsBalanceRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    
    public async Task<SpinsBalanceDeductionResult> TryDeductPoints(int userId)
    {
        var db = _redis.GetDatabase();
        var key = GetKey(userId);
        var newBalance = await db.StringDecrementAsync(key);
        if (newBalance < 0)
        {
            await db.StringIncrementAsync(key);

            return new SpinsBalanceDeductionResult(false, 0);
        }

        return new SpinsBalanceDeductionResult(true, newBalance);
    }
    
    private string GetKey(int userId) => $"spins_balance:{userId}";
}