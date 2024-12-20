namespace MA.SlotService.Infrastructure.DataAccess.Redis.Configuration;

public class RedisConfiguration
{
    public const string Key = "Redis";
    
    public string ConnectionString { get; set; }
}