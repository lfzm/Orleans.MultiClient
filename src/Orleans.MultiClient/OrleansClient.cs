using System;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.MultiClient
{
    public class OrleansClient : IOrleansClient
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IClusterClientFactory _clusterClientFactory;

        public OrleansClient(IServiceProvider serviceProvider, IClusterClientFactory clusterClientFactory)
        {
            _serviceProvider = serviceProvider;
            _clusterClientFactory = clusterClientFactory;
        }

        public Task<TGrainObserverInterface> CreateObjectReference<TGrainObserverInterface>(IGrainObserver obj) where TGrainObserverInterface : IGrainObserver
        {
            return _clusterClientFactory.Create<TGrainObserverInterface>().CreateObjectReference<TGrainObserverInterface>(obj);
        }

        public Task DeleteObjectReference<TGrainObserverInterface>(IGrainObserver obj) where TGrainObserverInterface : IGrainObserver
        {
            return _clusterClientFactory.Create<TGrainObserverInterface>().DeleteObjectReference<TGrainObserverInterface>(obj);
        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidKey
        {
            return _clusterClientFactory.Create<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerKey
        {
            return _clusterClientFactory.Create<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
        }

        public TGrainInterface GetGrain<TGrainInterface>(string primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithStringKey
        {
            return _clusterClientFactory.Create<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey
        {
            return _clusterClientFactory.Create<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
        }
        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerCompoundKey
        {
            return _clusterClientFactory.Create<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
        }


    }
}
