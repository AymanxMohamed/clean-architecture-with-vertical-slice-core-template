namespace ProjectName.Infrastructure.Common.Services.Caching;

public class CachingSettings
{
    public const string SectionName = "CachingSettings";

    private const int DefaultCacheExpiringTime = 1;

    public bool RedisCacheEnabled { get; init; }
    
    public string RedisServerUrl { get; init; }

    public int DefaultCacheExpiringTimeInMinutes { get; init; } = DefaultCacheExpiringTime;
}