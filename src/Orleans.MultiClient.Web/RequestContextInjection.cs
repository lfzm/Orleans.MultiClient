using Microsoft.AspNetCore.Http;

namespace Orleans.MultiClient.Web
{
    public  class RequestContextInjection
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _httpContext;
        public RequestContextInjection(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            this._httpContext = httpContext;
        }

        public void Inject()
        {

        }
    }
}
