# InterceptorAttribute demo

* 在一般情况下可以使用抽象的`AbstractInterceptorAttribute`自定义特性类，它实现`IInterceptor`接口。AspectCore默认实现了基于`Attribute`的拦截器配置。我们的自定义拦截器看起来像下面这样:
    ``` csharp
    public class CustomInterceptorAttribute : AbstractInterceptorAttribute 
    {
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
* 定义`ICustomService`接口和它的实现类`CustomService`:
    ``` csharp
    public interface ICustomService
    {
        [CustomInterceptor]
        void Call();
    }

    public class CustomService : ICustomService
    {
        public void Call()
        {
            Console.WriteLine("service calling...");
        }
    }
    ```
* 在`HomeController`中注入`ICustomService`:
    ``` csharp
    public class HomeController : Controller
    {
        private readonly ICustomService _service;
        public HomeController(ICustomService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            _service.Call();
            return View();
        }
    }
    ```
* 注册`ICustomService`，接着，在`ConfigureServices`中配置创建代理类型的容器:
    ``` csharp
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ICustomService, CustomService>();
        services.AddMvc();
        services.AddDynamicProxy();
        return services.BuildAspectCoreServiceProvider();
    }
    ```