using System;

namespace AspectCoreDemo
{
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