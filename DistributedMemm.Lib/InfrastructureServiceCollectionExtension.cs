using DistributedMemm.Lib.Infrastructure.ApiClients;
using DistributedMemm.Lib.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;

namespace DistributedMemm.Lib;

public static class InfrastructureServiceCollectionExtension
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("ReserveApiSettings").Get<ReserveApiSettings>();
        services.AddOptions<ReserveApiSettings>("ReserveApiSettings");
        services.AddRestEaseClient<IReserveApi>(options.Url);
        
        return services;
    }
}