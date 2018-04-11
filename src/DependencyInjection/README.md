# DependencyInjection demo

* 拦截器中的依赖注入。在拦截器中支持属性注入，构造器注入和服务定位器模式。

    属性注入，在拦截器中拥有`public get and set`权限的属性标记`[AspectCore.Injector.FromContainerAttribute]`特性，即可自动注入该属性，如：
    ``` csharp
    public class CustomInterceptorAttribute : AbstractInterceptorAttribute 
    {
        //ps : 只有使用 config.Interceptors.AddTyped<CustomInterceptor>(); 时，属性注入才生效， 
        //     不能使用以下这种方式 services.AddSingleton<CustomInterceptor>(); + [ServiceInterceptor(typeof(CustomInterceptor))]
        [FromContainer]
        public ILogger<CustomInterceptorAttribute> Logger { get; set; }

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            Logger.LogInformation("call interceptor");
            return next(context);
        }
    }
    ```
    构造器注入需要使拦截器作为`Service`，除全局拦截器外，仍可使用`ServiceInterceptor`使拦截器从DI中激活：

    ``` csharp
    public class CustomInterceptorAttribute : AbstractInterceptorAttribute 
    {
        private readonly ILogger<CustomInterceptor> ctorlogger;

        // ps : 当全局配置 config.Interceptors.AddTyped<CustomInterceptor>(); 时，构造器注入无法自动注入，需要手动处理
        //      只有使用 services.AddSingleton<CustomInterceptor>(); + [ServiceInterceptor(typeof(CustomInterceptor))] 才会自动注入
        public CustomInterceptor(ILogger<CustomInterceptor> ctorlogger)
        {
            this.ctorlogger = ctorlogger;
        }
    }
    public interface ICustomService
    {
        [ServiceInterceptor(typeof(CustomInterceptorAttribute))]
        void Call();
    }
    ```

    服务定位器模式。拦截器上下文`AspectContext`可以获取当前Scoped的`ServiceProvider`：
    ``` csharp
    public class CustomInterceptorAttribute : AbstractInterceptorAttribute 
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var logger = context.ServiceProvider.GetService<ILogger<CustomInterceptorAttribute>>();
            logger.LogInformation("call interceptor");
            return next(context);
        }
    }
    ```