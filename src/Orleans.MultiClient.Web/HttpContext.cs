using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Orleans.MultiClient.Web
{
    public class HttpContext
    {
        public  HttpRequest Request { get; internal set; }
        public  ConnectionInfo Connection { get; internal set; }
        public  ClaimsPrincipal User { get; internal set; }
        public  IServiceProvider RequestServices { get; internal set; }
        public  IDictionary<object, object> Items { get; internal set; }
    }
}
