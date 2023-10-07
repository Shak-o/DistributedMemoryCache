using DistributedMemm.Interfaces;
using DistributedMemm.Lib.Implementation;
using DistributedMemm.Lib.Implementation.Rabbit;
using DistributedMemm.Lib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedMemm.Lib;

public static class ServiceExtension
{
    public static IServiceCollection AddDistributedMemm(this IServiceCollection services)
    {
        services.AddHostedService<MessageConsumer>();
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddSingleton<IDistributedMemm, DistributedMemmImpl>();
        services.AddSingleton<ICacheAccessor, CacheAccessor>();
        
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}