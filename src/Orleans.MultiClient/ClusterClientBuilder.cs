using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Orleans.MultiClient
{
    public class ClusterClientBuilder : IClusterClientBuilder
    {
        private readonly OrleansClientOptions _options;
        private readonly ILogger _logger;
        private readonly string _serviceName;

        public ClusterClientBuilder(IServiceProvider serviceProvider, OrleansClientOptions options,string serviceName)
        {
            this._logger = serviceProvider.GetRequiredService<ILogger<ClusterClientBuilder>>();
            this._options = options;
            this._serviceName = serviceName;
        }
        public IClusterClient Build()
        {
            IClientBuilder build = new ClientBuilder();
            if (_options.Configure == null)
            {
                _logger.LogError($"{_serviceName} There is no way to connect to Orleans, please configure it in OrleansClientOptions.Configure");
            }
            _options.Configure(build);
            build.Configure<ClusterOptions>(opt =>
            {
                if (!string.IsNullOrEmpty(_options.ClusterId))
                    opt.ClusterId = _options.ClusterId;
                if (!string.IsNullOrEmpty(_options.ServiceId))
                    opt.ServiceId = _options.ServiceId;
            });

            var client = build.Build();
            return this.ConnectClient(_serviceName, client);
        }

        private IClusterClient ConnectClient(string serviceName, IClusterClient client)
        {
            try
            {
                var res = client.Connect(RetryFilter).Wait(TimeSpan.FromSeconds(10));
                if (!res)
                {
                    throw new Exception($"Connection {serviceName} timeout...");
                }
                _logger.LogDebug($"Connection {serviceName} Sucess...");
                return client;
            }
            catch (Exception ex)
            {
                throw new Exception($"Connection {serviceName} Faile...", ex);
            }
        }

        private int attempt = 0;
        private async Task<bool> RetryFilter(Exception exception)
        {
            if (exception.GetType() != typeof(SiloUnavailableException))
            {
                _logger.LogError(exception,$"Cluster client failed to connect to cluster with unexpected error. ");
                return false;
            }
            attempt++;
            _logger.LogError(exception,$"Cluster client attempt {attempt} of {10} failed to connect to cluster.");
            if (attempt > 10)
            {
                return false;
            }
            await Task.Delay(TimeSpan.FromSeconds(4));
            return true;
        }
    }
}
