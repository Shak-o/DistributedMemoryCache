using DistributedMemm.Infrastructure.ApiClients;
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
        var page = 0;
        var reserveResult = await _reserveApi.GetPagedDataAsync(page);

        foreach (var item in reserveResult.Data.Keys)
        {
            cache[item] = reserveResult.Data[item];
        }
        
        while (page != reserveResult.MaxPages)
        {
            var nextPageResult = await _reserveApi.GetPagedDataAsync(page++);
            
            foreach (var item in nextPageResult.Data.Keys)
            {
                cache[item] = reserveResult.Data[item];
            }
        }

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}