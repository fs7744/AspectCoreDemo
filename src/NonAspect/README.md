# GlobalInterceptor demo

* 在AspectCore中提供`NonAspectAttribute`来使得`Service`或`Method`不被代理：
    ``` csharp
    [NonAspect]
    public interface ICustomService
    {
        void Call();
    }
    ```
    同时支持全局忽略配置，亦支持通配符：
    ``` csharp
    services.AddDynamicProxy(config =>
    {
        //App1命名空间下的Service不会被代理
        config.NonAspectPredicates.AddNamespace("App1");

        //最后一级为App1的命名空间下的Service不会被代理
        config.NonAspectPredicates.AddNamespace("*.App1");

        //ICustomService接口不会被代理
        config.NonAspectPredicates.AddService("ICustomService");

        //后缀为Service的接口和类不会被代理
        config.NonAspectPredicates.AddService("*Service");

        //命名为Query的方法不会被代理
        config.NonAspectPredicates.AddMethod("Query");

        //后缀为Query的方法不会被代理
        config.NonAspectPredicates.AddMethod("*Query");
    });
    ```