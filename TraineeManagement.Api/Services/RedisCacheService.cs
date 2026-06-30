using System.Runtime.InteropServices;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace TraineeManagement.Api.Services;

public class RedisCacheService: IRedisCacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            string? cachedData = await _cache.GetStringAsync(key, cancellationToken);
            if(cachedData == null)
            {
                _logger.LogWarning("Value not found for key: {key} in redis", key);
                return default;
            }

            return JsonSerializer.Deserialize<T>(cachedData);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Error occured while fetching value for key: {key} with Excpetion: {ex}", key, ex.Message);
            return default;
        }
    }
    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        try
        { 
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(ttl);
            string jsonValue = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, jsonValue, options, cancellationToken);
        }
        catch(Exception ex)
        {
            _logger.LogWarning("Error occured while setting value for key: {key} with Excpetion: {ex}", key, ex.Message);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {   
            await _cache.RemoveAsync(key, cancellationToken);
        }
        catch(Exception ex)
        {
            _logger.LogWarning("Error occured while removing value for key: {key} with Excpetion: {ex}", key, ex);
        }
    }
}