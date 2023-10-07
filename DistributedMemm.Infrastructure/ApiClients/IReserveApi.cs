using DistributedMemm.Infrastructure.Models;
using RestEase;

namespace DistributedMemm.Infrastructure.ApiClients;

public interface IReserveApi
{
    [Get("Reserve")]
    Task<GetReserveResult> GetPagedDataAsync(int page);
}