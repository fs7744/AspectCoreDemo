using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using AspectCore.Configuration;

namespace AspectCoreDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<ICustomService, CustomService>();
            services.AddDynamicProxy(config => 
            {
                ////App1命名空间下的Service不会被代理
                //config.NonAspectPredicates.AddNamespace("App1");

                ////最后一级为App1的命名空间下的Service不会被代理
                //config.NonAspectPredicates.AddNamespace("*.App1");

                ////ICustomService接口不会被代理
                //config.NonAspectPredicates.AddService("ICustomService");

                //后缀为Service的接口和类不会被代理
                //config.NonAspectPredicates.AddService("*Service");

                ////命名为Query的方法不会被代理
                //config.NonAspectPredicates.AddMethod("Query");

                ////后缀为Query的方法不会被代理
                //config.NonAspectPredicates.AddMethod("*Query");
                config.Interceptors.AddTyped<CustomInterceptor>(Predicates.ForService("*Service"));
            });
            return services.BuildAspectCoreServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}