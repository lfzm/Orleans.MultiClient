using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Orleans.MultiClient.DependencyInjection
{
    public interface IMultiClientBuilder
    {
        IServiceCollection Services { get;  }

         Action<IClientBuilder> OrleansConfigure { get; set; }
         IList<OrleansClientOptions> ClientOptions { get; set; }
        void Build();
    }
}
