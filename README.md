# Orleans MultiClient 　　　　　　　　　　　　　　　　[中文](https://github.com/lfzm/Orleans.MultiClient/blob/master/README.zh-cn.md)


[![NuGet](https://img.shields.io/nuget/v/Orleans.MultiClient.svg?style=flat)](http://www.nuget.org/packages/Orleans.MultiClient)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/dotnetcore/CAP/master/LICENSE.txt)


`Orleans.MultiClient` is an Orleans multi-client, with simple configuration you can easily connect to Orleans Slio.
`Orleans.MultiClient` can connect to many different Orleans Slio, Orleans.MultiClient Orleans Grain automatically finds Orleans clients upon request.

*Note：Orleans.MultiClient only supports .NET Core platform.*

## Getting Started

### NuGet

```
PM> dotnet add package Orleans.MultiClient
```

### Simple example

A simple example demonstrates how Orleans.MultiClient connects to two different services (A, B).

*Note：The source code for the [simple example](https://github.com/lfzm/Orleans.MultiClient/tree/master/example) is in the example folder in the Github repository*


1、Add the Grain interface package of A and B to the project of Orleans Client

```
<ItemGroup>
    <PackageReference Include="Orleans.MultiClient" Version="3.1.0" />
    <!--A Slio Grain interface package-->
    <ProjectReference Include="..\Orleans.Grain2\Orleans.Grain2.csproj" /> 
    <!--B Slio Grain interface package-->
    <ProjectReference Include="..\Orleans.Grain\Orleans.Grain.csproj" /> 
</ItemGroup>
```


2、Configure Orleans.MultiClient and inject Orleans.MultiClient into `IServiceCollection`。


```CSharp
services.AddOrleansMultiClient(build =>
{
    build.AddClient(opt =>
    {
        opt.ServiceId = "A";
        opt.ClusterId = "AApp";
        opt.SetServiceAssembly(typeof(IHelloA).Assembly);
        opt.Configure = (b =>
        {
            b.UseLocalhostClustering(gatewayPort: 30000);
        });
    });
    build.AddClient(opt =>
    {
        opt.ServiceId = "B";
        opt.ClusterId = "BApp";
        opt.SetServiceAssembly(typeof(IHelloB).Assembly);
        opt.Configure = (b =>
        {
            b.UseLocalhostClustering(gatewayPort: 30001);
        });
    });
});
```

3、Request A Slio and B Slio separately

*Note: Before requesting, A Slio and B Slio need to be started, otherwise the request will fail.*

```CSharp
// Request A Slio.
var serviceA = _serviceProvider.GetRequiredService<IOrleansClient>().GetGrain<IHelloA>(1);
var resultA = serviceA.SayHello("Hello World Success GrainA").GetAwaiter().GetResult();

// Request B Slio.
var serviceB = _serviceProvider.GetRequiredService<IOrleansClient>().GetGrain<IHelloB>(1);
var resultB = serviceB.SayHello("Hello World Success GrainB").GetAwaiter().GetResult();

```


## Configuration

`AddOrleansMultiClient` method supports IServiceCollection and GenericHost configuration.


###  Configure Orleans Slio connection


Configure connectivity for a single Orleans Slio.

``` CSharp
services.AddOrleansMultiClient(build =>
{
    build.AddClient(opt =>
    {
        opt.ServiceId = "A";
        opt.ClusterId = "AApp";
        opt.SetServiceAssembly(typeof(IHelloA).Assembly);
        opt.Configure = (b =>
        {
            b.UseLocalhostClustering(gatewayPort: 30000);
         // b.UseStaticClustering(...);  
         // b.UseConsulClustering(...); // Configure Consul
         // b.UseKubeGatewayListProvider(); // Configure Kubernetes
         // b.UseAdoNetClustering(...); // Configure MySql and SQLServer
        });
    });
}
```

Global configuration, common to all clients

```CSharp
services.AddOrleansMultiClient(build =>{
    build.Configure(b =>{
        b.UseConsulClustering(...);
     // b.UseKubeGatewayListProvider(); // Configure Kubernetes
     // b.UseAdoNetClustering(...); // Configure MySql and SQLServer
    });
    build.AddClient(opt =>
    {
        opt.ServiceId = "A";
        opt.ClusterId = "AApp";
        opt.SetServiceAssembly(typeof(IHelloA).Assembly);
    }
}
```









