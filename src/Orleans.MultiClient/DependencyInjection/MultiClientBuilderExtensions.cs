using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.MultiClient;
using Orleans.MultiClient.DependencyInjection;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultiClientBuilderExtensions
    {
        public static IMultiClientBuilder AddClient(this IMultiClientBuilder builder, Action<OrleansClientOptions> startup)
        {
            OrleansClientOptions options = new OrleansClientOptions();
            startup.Invoke(options);
           return builder.AddClient(options);
        }
        public static IMultiClientBuilder AddClient(this IMultiClientBuilder builder, OrleansClientOptions options)
        {
            if (options.Configure == null)
            {
                options.Configure = builder.OrleansConfigure;
            }
            if (!options.ExistAssembly(options.ServiceName))
                throw new ArgumentNullException($"{options.ServiceName} service does not exist in the assembly");

            builder.Services.AddSingletonNamedService<IClusterClientBuilder>(options.ServiceName.ToLower(), (sp, key) =>
            {
                return new ClusterClientBuilder(sp, options);
            });
            return builder;
        }
        public static IMultiClientBuilder AddClient(this IMultiClientBuilder builder, IConfiguration config)
        {
            var optionList = config.Get<IList<OrleansClientOptions>>();
            foreach (var options in optionList)
            {
                 builder.AddClient(options);
            }
            return builder;
        }
        public static IMultiClientBuilder Configure(this IMultiClientBuilder builder, Action<IClientBuilder> OrleansConfigure)
        {
            builder.OrleansConfigure = OrleansConfigure;
            return builder;
        }

       
    }
}
