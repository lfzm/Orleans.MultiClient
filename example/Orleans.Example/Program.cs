using Microsoft.Extensions.DependencyInjection;
using Orleans.Grains;
using Orleans.Grains2;
using Orleans.Hosting;
using System;

namespace Orleans.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter btn start");
            Console.ReadKey();
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddOrleansMultiClient(build =>
            {
                build.Configure(b =>
                {
                    b.UseLocalhostClustering();
                });
                build.AddClient(opt =>
                {
                    opt.ServiceId = "dev";
                    opt.ClusterId = "HelloWorldApp";
                    opt.SetServiceAssembly(typeof(IHello).Assembly);
                });
                build.AddClient(opt =>
                {
                    opt.ServiceId = "dev2";
                    opt.ClusterId = "HelloWorldApp2";
                    opt.ServiceName = "Orleans.Grain2"; ;
                    opt.Configure = (b =>
                    {
                        b.UseLocalhostClustering(gatewayPort: 30001);
                    });
                });
            });

            var service = services.BuildServiceProvider().GetRequiredService<IOrleansClient>().GetGrain<IHello>(1);
            var result1 = service.SayHello("Hello World Success Grain1").GetAwaiter().GetResult();

            var service2 = services.BuildServiceProvider().GetRequiredService<IOrleansClient>().GetGrain<IHello2>(1);
            var result2 = service2.SayHello("Hello World Success Grain2").GetAwaiter().GetResult();
            Console.WriteLine("dev1:" + result1);
            Console.WriteLine("dev2:" + result2);

            Console.ReadKey();
        }
    }
}
