using Autofac;
using System;
using System.Threading.Tasks;

namespace CronHosts.Runner
{
    public class Program
    {
        private static IContainer Container { get; set; }

        public static async Task Main(string[] args)
        {
            Container = Startup.Configure();
            Console.WriteLine("Hello World!");
        }
    }
}