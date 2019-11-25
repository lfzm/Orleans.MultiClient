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
            var name = typeof(TGrainInterface).Assembly.FullName;

            return clusterClientCache.GetOrAdd(name, (key) =>
            {
                IClusterClient client = this._serviceProvider.GetRequiredServiceByName<IClusterClientBuilder>(key).Build();
                if (client.IsInitialized)
                {
                    return client;
                }
                else
                {
                    throw new Exception("not tnitialized clusterClient");
                }
            });
        }
    }
}
