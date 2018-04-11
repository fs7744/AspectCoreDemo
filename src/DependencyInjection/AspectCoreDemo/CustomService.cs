using AspectCore.DynamicProxy;
using System;

namespace AspectCoreDemo
{
    public interface ICustomService
    {
        [ServiceInterceptor(typeof(CustomInterceptor))]
        void Call();
    }

    public class CustomService : ICustomService
    {
        public void Call()
        {
            Console.WriteLine("service calling...");
        }
    }
}