using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

namespace Orleans.MultiClient
{
    public class OrleansClientOptions
    {
        public OrleansClientOptions()
        {
            ServiceList = new List<string>();
        }
        public IList<string> ServiceList { get; set; }
        public string ServiceId { get; set; }
        public string ClusterId { get; set; }
        public Action<IClientBuilder> Configure { get; set; }

        public void SetServiceAssembly(params Assembly[] assemblys)
        {
            foreach (var assembly in assemblys)
            {
                var name = assembly.FullName;
                if (!this.ServiceList.Contains(name))
                {
                    this.ServiceList.Add(name);
                }
            }
        }

    }
}
