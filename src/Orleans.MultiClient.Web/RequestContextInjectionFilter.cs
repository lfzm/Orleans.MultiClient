using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Orleans.MultiClient.Web
{
    public class RequestContextInjectionFilter : IClusterClientRequestFilter
    {
        private readonly IServiceProvider serviceProvider;
        public RequestContextInjectionFilter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IGrainFactory Filter(IGrainFactory grainFactory)
        {
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            if (httpContextAccessor != null)
            {
                grainFactory.InjectRequestContext(httpContextAccessor.HttpContext);
            }
            return grainFactory;
        }
    }
}
