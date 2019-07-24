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
          
            builder.ClientOptions.Add(options);

         
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
