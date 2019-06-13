using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;
using System;

namespace Orleans.MultiClient.DependencyInjection
{
    public class MultiClientBuilder : IMultiClientBuilder
    {
        public IServiceCollection Services { get; private set; }

        public Action<IClientBuilder> OrleansConfigure { get;  set; }

        public MultiClientBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
        public void Build()
        {
            this.Services.AddSingleton(typeof(IKeyedServiceCollection<,>), typeof(KeyedServiceCollection<,>));
            this.Services.AddSingleton<IClusterClientFactory, MultiClusterClientFactory>();
            this.Services.AddTransient<IOrleansClient, OrleansClient>();
        }
    }
}
