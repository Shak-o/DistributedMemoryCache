using DistributedMemm.Lib.HostedServices;
using DistributedMemm.Lib.Implementation;
using DistributedMemm.Lib.Implementation.Rabbit;
using DistributedMemm.Lib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedMemm.Lib;

public static class ServiceExtension
{
    public static IServiceCollection AddDistributedMemm(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<MessageConsumer>();
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddSingleton<IDistributedMemm, DistributedMemmImpl>();
        services.AddSingleton<ICacheAccessor, CacheAccessor>();
        
        var url = configuration.GetValue<string?>("ReserveApi");
        if(url != null)
        {
            services.AddInfra(configuration);
            services.AddHostedService<ReserveHostedService>();
        }
        
        return services;
    }
}