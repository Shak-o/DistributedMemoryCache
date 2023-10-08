using DistributedMemm.Lib.HostedServices;
using DistributedMemm.Lib.Implementation;
using DistributedMemm.Lib.Implementation.Rabbit;
using DistributedMemm.Lib.Interfaces;
using DistributedMemm.Lib.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DistributedMemm.Lib;

public static class ServiceExtension
{
    public static IServiceCollection AddDistributedMemm(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQSettings>(ops => configuration.GetSection("RabbitMqSettings").Bind(ops));
        services.Configure<ReserveApiSettings>(ops => configuration.GetSection("ReserveApiSettings").Bind(ops));

        services.AddHostedService<MessageConsumer>(sp =>
        {
            var eventProcessor = sp.GetRequiredService<IEventProcessor>();
            var rabbitMqSettings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
            return new MessageConsumer(eventProcessor, rabbitMqSettings);
        });

        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>(sp =>
        {
            var rabbitMqSettings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
            return new MessagePublisher(rabbitMqSettings);
        });

        services.AddSingleton<IDistributedMemm, DistributedMemmImpl>();
        services.AddSingleton<ICacheAccessor, CacheAccessor>();

        var url = configuration.GetValue<string?>("ReserveApiSettings:Url");
        if (url != null)
        {
            services.AddInfra(configuration);
            services.AddHostedService<ReserveHostedService>();
        }

        return services;
    }
}