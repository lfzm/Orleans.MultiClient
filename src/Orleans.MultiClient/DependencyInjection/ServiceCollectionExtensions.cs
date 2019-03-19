using Orleans.MultiClient.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrleansMultiClient(this IServiceCollection services, Action<IMultiClientBuilder> startup)
        {
            var build = new MultiClientBuilder(services);
            startup.Invoke(build);
            build.Build();
            return services;
        }
    }
}
