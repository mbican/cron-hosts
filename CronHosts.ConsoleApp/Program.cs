using Autofac;
using System;
using System.Threading.Tasks;

namespace CronHosts.ConsoleApp
{
    public class Program
    {
        private static IContainer Container { get; set; }

        public static async Task Main(string[] args)
        {
            using (Container = Startup.Configure())
            {
                Console.WriteLine("Hello World!");
            }
        }
    }
}