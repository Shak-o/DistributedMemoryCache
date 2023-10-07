using DistributedMemm.Infrastructure;
using DistributedMemm.Interfaces;
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
        services.AddHostedService<MessageBusSubscriber>();
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddSingleton<IDistributedMemm, DistributedMemmImpl>();
        services.AddSingleton<ICacheAccessor, CacheAccessor>();

        services.AddInfra(configuration);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddHostedService<ReserveHostedService>();
        return services;
    }
}