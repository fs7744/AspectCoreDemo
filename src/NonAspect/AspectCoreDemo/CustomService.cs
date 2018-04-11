using AspectCore.DynamicProxy;
using System;

namespace AspectCoreDemo
{
    [NonAspect]
    public interface ICustomService
    {
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