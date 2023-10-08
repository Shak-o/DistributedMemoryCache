using DistributedMemm.Lib.Infrastructure.Models;
using RestEase;

namespace DistributedMemm.Lib.Infrastructure.ApiClients;

public interface IReserveApi
{
    [Get("/api/KeyValue")]
    Task<GetReserveResult> GetPagedDataAsync([Query]int page);
}