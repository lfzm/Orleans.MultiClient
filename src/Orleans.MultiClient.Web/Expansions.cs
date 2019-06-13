using Orleans.MultiClient.Web;

namespace Orleans
{
    public static class Expansions
    {
        public static IGrainFactory InjectRequestContext(this IGrainFactory request, Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            new RequestContextInjection(httpContext).Inject();
            return request;
        }

        public static HttpContext GetHttpContext(this Grain grain)
        {
            return new RequestContextAnalyze().Analyze();
        }
    }
}
