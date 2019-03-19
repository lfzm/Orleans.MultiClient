using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

namespace Orleans.MultiClient
{
    public class OrleansClientOptions
    {
        public string ServiceId { get; set; }
        public string ClusterId { get; set; }
        public string ServiceName { get;  set; }
        public Action<IClientBuilder> Configure { get; set; }
        public void SetServiceAssembly(Assembly assembly)
        {
            this.ServiceName = assembly.GetName().Name;
        }
        internal bool ExistAssembly(string serviceName)
        {
            //获取所有的程序集
            var assembly = Assembly.GetEntryAssembly();
            List<AssemblyName> assemblys = assembly
                  .GetReferencedAssemblies().ToList();//获取所有引用程序集
            assemblys.Add(assembly.GetName());

            var assemblyName = assemblys.Where(f => f.Name.Equals(serviceName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (assemblyName == null)
                return false;
            else
                return true;

        }
    }
}
