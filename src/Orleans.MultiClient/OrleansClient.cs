using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.MultiClient
{
    public class OrleansClient : IOrleansClient
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IClusterClientFactory _clusterClientFactory;
        private readonly IEnumerable<IClusterClientRequestFilter> _clusterClientRequestFilters;

        public OrleansClient(IServiceProvider serviceProvider, IClusterClientFactory clusterClientFactory, IEnumerable<IClusterClientRequestFilter> clusterClientRequestFilter)
        {
            _serviceProvider = serviceProvider;
            _clusterClientFactory = clusterClientFactory;
            _clusterClientRequestFilters = clusterClientRequestFilter;
        }

        public Task<TGrainObserverInterface> CreateObjectReference<TGrainObserverInterface>(IGrainObserver obj) where TGrainObserverInterface : IGrainObserver
        {
            return this.GetGrainFactory<TGrainObserverInterface>().CreateObjectReference<TGrainObserverInterface>(obj);
        }

        public Task DeleteObjectReference<TGrainObserverInterface>(IGrainObserver obj) where TGrainObserverInterface : IGrainObserver
        {
            return this.GetGrainFactory<TGrainObserverInterface>().DeleteObjectReference<TGrainObserverInterface>(obj);
        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidKey
        {
            return this.GetGrainFactory<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerKey
        {
            return this.GetGrainFactory<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
        }

        public TGrainInterface GetGrain<TGrainInterface>(string primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithStringKey
        {
            return this.GetGrainFactory<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey
        {
            return this.GetGrainFactory<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
        }
        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerCompoundKey
        {
            return this.GetGrainFactory<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
        }

        private  IGrainFactory GetGrainFactory<T>()
        {
            var frainFactory = _clusterClientFactory.Create<T>();
            if(_clusterClientRequestFilters.Count()>0)
            {
                foreach (var filter in _clusterClientRequestFilters)
                {
                    filter.Filter(frainFactory);
                }
            }
            return frainFactory;

        }
    }
}
