using DistributedMemm.Infrastructure.ApiClients;
using DistributedMemm.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;

namespace DistributedMemm.Infrastructure;

public static class InfrastructureServiceCollectionExtension
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("ReserveApi").Get<ReserveApiOptions>();
        services.AddOptions<ReserveApiOptions>("ReserveApi");
        services.AddRestEaseClient<IReserveApi>(options.Url);
        
        return services;
    }
}