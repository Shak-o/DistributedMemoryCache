using System.Collections.Concurrent;
using System.Text.Json;
using DistributedMemm.Lib.Infrastructure.ApiClients;
using DistributedMemm.Lib.Infrastructure.Models;
using DistributedMemm.Lib.Interfaces;
using Microsoft.Extensions.Hosting;

namespace DistributedMemm.Lib.HostedServices;

public class ReserveHostedService : IHostedService
{
    private readonly ICacheAccessor _cacheAccessor;
    private readonly IReserveApi _reserveApi;

    public ReserveHostedService(ICacheAccessor cacheAccessor, IReserveApi reserveApi)
    {
        _cacheAccessor = cacheAccessor;
        _reserveApi = reserveApi;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var cache = _cacheAccessor.GetCache();
        await UpdateCasheAsync(1, cache, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task UpdateCasheAsync(int page, ConcurrentDictionary<string, GenericCacheModel> cache,
        CancellationToken cancellationToken)
    {
        var reserveResult = await _reserveApi.GetPagedDataAsync(page);
        foreach (var item in reserveResult.Pairs)
        {
            try
            {
                cache.TryAdd(item.Key, JsonSerializer.Deserialize<GenericCacheModel>(item.Value.ToString()));
            }
            catch { }
        }

        if (page != reserveResult.MaxPages)
        {
            await UpdateCasheAsync(++page, cache, cancellationToken);
        }
    }
}