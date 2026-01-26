using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FrenchRevolution.Infrastructure.Cache;

public interface ICacheAside
{
    Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        DistributedCacheEntryOptions? options = null,
        CancellationToken ct = default
        );
}

public sealed class 
    CacheAside(
    IDistributedCache cache,
    ILogger<CacheAside> logger
    ) : ICacheAside
{
    private static readonly DistributedCacheEntryOptions Default = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
    };
    
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);
    
    public async Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        DistributedCacheEntryOptions? options = null,
        CancellationToken ct = default
    )
    {
        var cachedValue = await cache.GetStringAsync(key, ct);

        T? value;

        if (!string.IsNullOrWhiteSpace(cachedValue))
        {
            value = JsonSerializer.Deserialize<T>(cachedValue);

            if (value is not null)
            {
                return value;
            }
        }
        
        var hasLock = await SemaphoreSlim.WaitAsync(500, ct);
        
        if (!hasLock)
        {
            return default;
        }

        try
        {
            cachedValue = await cache.GetStringAsync(key, ct);
            
            if (!string.IsNullOrWhiteSpace(cachedValue))
            {
                value = JsonSerializer.Deserialize<T>(cachedValue);

                if (value is not null)
                {
                    return value;
                }
            }
            
            value = await factory(ct);

            if (value is null)
            {
                return default;
            }
        
            await cache.SetStringAsync(
                key, 
                JsonSerializer.Serialize(value), 
                options ?? Default, 
                ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while accessing cache for key {Key}", key);
            throw;
        }
        finally
        {
            SemaphoreSlim.Release();
        }
        
        return value;
    }
}