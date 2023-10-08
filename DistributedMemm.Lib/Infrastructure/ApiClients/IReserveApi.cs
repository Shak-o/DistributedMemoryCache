using DistributedMemm.Lib.Infrastructure.Models;
using RestEase;

namespace DistributedMemm.Lib.Infrastructure.ApiClients;

public interface IReserveApi
{
    [Get("Reserve")]
    Task<GetReserveResult> GetPagedDataAsync(int page);
}