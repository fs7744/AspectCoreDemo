using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AspectCoreDemo
{
    [NonAspect]
    public class CustomInterceptor : AbstractInterceptor
    {
        private readonly ILogger<CustomInterceptor> ctorlogger;

        // ps : 当全局配置 config.Interceptors.AddTyped<CustomInterceptor>(); 时，构造器注入无法自动注入，需要手动处理
        //public CustomInterceptor(ILogger<CustomInterceptor> ctorlogger)
        //{
        //    this.ctorlogger = ctorlogger;
        //}

        //ps : 只有使用 [ServiceInterceptor(typeof(CustomInterceptor))] 时，属性注入才生效
        [FromContainer]
        public ILogger<CustomInterceptor> Logger { get; set; }

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var logger = context.ServiceProvider.GetService<ILogger<CustomInterceptor>>();
            logger.LogWarning("logger from ServiceProvider");
            ctorlogger?.LogError("logger from ctor");
            Logger?.LogWarning("logger from Property");
            return next(context);
        }
    }
}