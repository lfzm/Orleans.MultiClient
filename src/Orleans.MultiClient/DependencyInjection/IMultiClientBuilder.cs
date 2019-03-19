using Microsoft.Extensions.DependencyInjection;
using System;

namespace Orleans.MultiClient.DependencyInjection
{
    public interface IMultiClientBuilder
    {
        IServiceCollection Services { get;  }

         Action<IClientBuilder> OrleansConfigure { get; set; }

        void Build();
    }
}
