using Microsoft.Extensions.DependencyInjection;
using Orleans.MultiClient.DependencyInjection;
using System;

namespace Microsoft.Extensions.Hosting
{
    public static class GenericHostExtensions
    {
        public static IHostBuilder UseOrleansMultiClient(this IHostBuilder hostBuilder, Action<IMultiClientBuilder> startup)
        {
            hostBuilder.ConfigureServices((context, service) => service.AddOrleansMultiClient(startup));
            return hostBuilder;
        }
    }
}
