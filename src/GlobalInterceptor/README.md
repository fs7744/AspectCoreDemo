# GlobalInterceptor demo

* 拦截器配置。

    全局拦截器。使用`AddDynamicProxy(Action<IAspectConfiguration>)`的重载方法，其中`IAspectConfiguration`提供`Interceptors`注册全局拦截器:
    ``` csharp
    services.AddDynamicProxy(config =>
    {
        config.Interceptors.AddTyped<CustomInterceptorAttribute>();
    });
    ```
    带构造器参数的全局拦截器，在`CustomInterceptorAttribute`中添加带参数的构造器：
    ``` csharp
    public class CustomInterceptorAttribute : AbstractInterceptorAttribute 
    {
        private readonly string _name;
        public CustomInterceptorAttribute(string name)
        {
            _name = name;
        }
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                Console.WriteLine("Before service call");
                await next(context);
            }
            catch (Exception)
            {
                Console.WriteLine("Service threw an exception!");
                throw;
            }
            finally
            {
                Console.WriteLine("After service call");
            }
        }
    }
    ```
    修改全局拦截器注册:
    ``` csharp
    services.AddDynamicProxy(config =>
    {
        config.Interceptors.AddTyped<CustomInterceptorAttribute>(args: new object[] { "custom" });
    });
    ```
    作为服务的全局拦截器。在`ConfigureServices`中添加：
    ``` csharp
    services.AddTransient<CustomInterceptorAttribute>(provider => new CustomInterceptorAttribute("service"));
    ```
    修改全局拦截器注册:
    ``` csharp
    services.AddDynamicProxy(config =>
    {
        config.Interceptors.AddServiced<CustomInterceptorAttribute>();
    });
    ```
    作用于特定`Service`或`Method`的全局拦截器，下面的代码演示了作用于带有`Service`后缀的类的全局拦截器：
    ``` csharp
    services.AddDynamicProxy(config =>
    {
        config.Interceptors.AddTyped<CustomInterceptorAttribute>(method => method.DeclaringType.Name.EndsWith("Service"));
    });
    ```
    使用通配符的特定全局拦截器：
    ``` csharp
    services.AddDynamicProxy(config =>
    {
        config.Interceptors.AddTyped<CustomInterceptorAttribute>(Predicates.ForService("*Service"));
    });
    ```