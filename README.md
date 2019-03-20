# Orleans MultiClient 多个Silo复合客户端

---

[![NuGet](https://img.shields.io/nuget/v/Orleans.MultiClient.svg?style=flat)](http://www.nuget.org/packages/Orleans.MultiClient)

## 介绍
`Orleans.MultiClient` 是一个 Orleans 复合客户端，只需要简单配置就可以简单高效连接和请求 Orleans 服务。
`Orleans.MultiClient` 可以轻松连接多个不同服务的 Orleans 服务,在请求 Orleans 时会根据请求的接口自动寻找 Orleans 客户端，使用者无需关心底层的实现。

`Orleans.MultiClient` 的开源地址和 `Demo` 可以在 `GitHub` 源代码下载查看。
> https://github.com/AClumsy/Orleans.MultiClient/

## 使用

从 `NuGet` 下载 `Orleans.MultiClient` 包。
```

```

### 简单例子
如果有一个服务需要同时调用 `A` 和 `B` 两个 `Orleans` 服务，看一看 `Orleans.MultiClient` 是怎么更简单的调用 Orleans 服务的。
第一步：先引用 `Orleans.MultiClient` 包和 `A`、`B` 的接口，分别为 `IHelloA`、 `IHelloB`
第二步：需要把 `Orleans.MultiClient` 注入到 `DI 容器` 中，并且配置添加两个 Orleans Client。

*提示：`Orleans.MultiClient` 暂时只支持 `.NET Core` 平台上面使用。*
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
            b.UseLocalhostClustering();
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
第二步：开始调用对应的 Orleans 服务。
`IOrleansClient` 是 `Orleans.MultiClient` 的复合客户端，通过 `IOrleansClient` 调用 Orleans 服务。
```CSharp
// 调用 A 服务。
var serviceA = _serviceProvider.GetRequiredService<IOrleansClient>().GetGrain<IHelloA>(1);
var resultA = serviceA.SayHello("Hello World Success GrainA").GetAwaiter().GetResult();

// 调用 B 服务。
var serviceB = _serviceProvider.GetRequiredService<IOrleansClient>().GetGrain<IHelloB>(1);
var resultB = serviceB.SayHello("Hello World Success GrainB").GetAwaiter().GetResult();
```
简单吧，只要配置好客户端之后，在使用的过程中，无需管怎么连接 `Orleans` ，只要通过依赖注入得到  `IOrleansClient` 就可以轻松的请求 Orleans 服务。

## 配置
### **注入到 DI 容器**

**`AddOrleansMultiClient`** ：把 `Orleans.MultiClient` 注入到  `DI 容器` 中，使用时需要通过依赖注入得到 `IOrleansClient` 。

### **添加多个 Client**
**`AddClient`**： 添加多个 Orleans 客户客户端，添加客户端时需要配置 Orleans 相关选项。 `Orleans.MultiClient` 提供了函数和 `IConfiguration` 两种方式进行配置。
使用  `IConfiguration`  进行配置时需要注意配置文件的内容必须是 `IList<OrleansClientOptions>` 类型的。

```CSharp
services.AddOrleansMultiClient(build =>
{
    build.AddClient((OrleansClientOptions opt) =>{
       ...// OrleansClientOptions 配置
    }
});
```
### **全局 Orleans 服务配置**
**`Configure`**：如果所有的 Orleans 的连接配置是一样的情况下，可以配置全局的 Orleans 服务配置。
比如：如果所有的 Orleans Silo 都是通过 `Consul` 进行服务发现的，就可以配置一个全局配置。
```CSharp
services.AddOrleansMultiClient(build =>{
    build.Configure(b =>{
        b.UseConsulClustering(o =>{
            o.Address = new Uri("https://127.0.0.1:8500");
        });
    });
}
```

#### **OrleansClientOptions 配置**
* **`ServiceList`**：用于在 `IOrleansClient` 调用接口时和 Orleans 连接配置建立关联。ServiceList 的值时 Orleans Silo 接口的 `Assembly.FullName`,  由于 Orleans Silo 可能有多个接口，所以 ServiceList 是一个数组集合。可以通过 `SetServiceAssembly` 方法来配置 ServiceList。

* **`ServiceId`**：Orleans Silo 的 ServiceId，在连接 Orleans 时需要。

* **`ClusterId`**：Orleans Silo 的 ClusterId，在连接 Orleans 时需要。

* **`Configure`**：Orleans 服务配置，如连接组件（`Consul`、`Zookeeperr`、等）。如果配置了 `全局 Orleans 服务配置` 这个选项可以不配置，但是这选项配置之后会覆盖上面的 `全局 Orleans 服务配置`。








