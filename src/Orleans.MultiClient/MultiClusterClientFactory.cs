using Orleans.Runtime;
using System;
using System.Collections.Concurrent;

namespace Orleans.MultiClient
{
    public class MultiClusterClientFactory : IClusterClientFactory
    {
        private readonly ConcurrentDictionary<string, IGrainFactory> clusterClientCache = new ConcurrentDictionary<string, IGrainFactory>();
        private readonly IServiceProvider _serviceProvider;
        public MultiClusterClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IGrainFactory Create<TGrainInterface>()
        {
            var name = typeof(TGrainInterface).FullName;
            return clusterClientCache.GetOrAdd(name, (key) =>
            {
                string serviceName = typeof(TGrainInterface).Assembly.GetName().Name.ToLower();
                return this._serviceProvider.GetRequiredServiceByName<IClusterClientBuilder>(serviceName).Build();

            });
        }
    }
}
