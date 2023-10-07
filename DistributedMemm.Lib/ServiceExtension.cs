using DistributedMemm.Lib.Implementation;
using DistributedMemm.Lib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedMemm.Lib;

public static class ServiceExtension
{
    public static IServiceCollection AddDistributedMemm(this IServiceCollection services)
    {
        services.AddSingleton<IDistributedMemm, DistributedMemmImpl>();

        return services;
    }
}