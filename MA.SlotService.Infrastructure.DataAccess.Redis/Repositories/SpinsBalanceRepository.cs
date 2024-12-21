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

    public async Task<long> Get(int userId)
    {
        var db = _redis.GetDatabase();
        var key = GetKey(userId);

        var balanceValue = await db.StringGetAsync(key);
        
        return balanceValue.TryParse(out long balance)
            ? balance
            : 0;
    }

    public async Task<long> Add(int userId, long amount)
    {
        var db = _redis.GetDatabase();
        var key = GetKey(userId);
        
        var newBalance = await db.StringIncrementAsync(key, amount);

        return newBalance;
    }
    
    public async Task<SpinsBalanceDeductionResult> TryDeduct(int userId)
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