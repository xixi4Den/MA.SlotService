using MA.SlotService.Application.Abstractions;
using StackExchange.Redis;

namespace MA.SlotService.Infrastructure.DataAccess.Redis.Repositories;

public class SpinsBalanceRepository(IConnectionMultiplexer redis) : ISpinsBalanceRepository
{
    private readonly IConnectionMultiplexer _redis = redis;

    private const string ReferenceIdLogKey = "reference_log";
    private static readonly TimeSpan ReferenceIdLogTtl = TimeSpan.FromDays(15);

    public async Task<long> GetAsync(int userId)
    {
        var db = _redis.GetDatabase();
        var key = GetKey(userId);

        var balanceValue = await db.StringGetAsync(key);
        
        return balanceValue.TryParse(out long balance)
            ? balance
            : 0;
    }

    public async Task<long> AddAsync(int userId, long amount, string referenceId)
    {
        var db = _redis.GetDatabase();
        var key = GetKey(userId);
        
        var newBalance = await db.StringIncrementAsync(key, amount);
        
        var expirationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + ReferenceIdLogTtl.TotalMilliseconds;
        await db.SortedSetAddAsync(ReferenceIdLogKey, referenceId, expirationTimestamp);

        return newBalance;
    }
    
    public async Task<SpinsBalanceDeductionResult> TryDeductAsync(int userId)
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
    
    public async Task<bool> ContainsReferenceIdAsync(string referenceId)
    {
        var db = redis.GetDatabase();
        
        var score = await db.SortedSetScoreAsync(ReferenceIdLogKey, referenceId);
        await RemoveOutdatedReferenceIds(db);

        return score.HasValue;
    }

    private static async Task RemoveOutdatedReferenceIds(IDatabase db)
    {
        var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await db.SortedSetRemoveRangeByScoreAsync(ReferenceIdLogKey, start: double.NegativeInfinity,
            stop: currentTimestamp);
    }
    
    private string GetKey(int userId) => $"spins_balance:{userId}";
}