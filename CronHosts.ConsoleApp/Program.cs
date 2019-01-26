using Autofac;
using System.Threading.Tasks;

namespace CronHosts.ConsoleApp
{
    public static class Program
    {
        private static IContainer Container { get; set; }

        public static async Task<int> Main(string[] args)
        {
            using (Container = Startup.Build())
            using (var scope = Container.BeginLifetimeScope("program"))
                return await scope.Resolve<IProgram>().Run(args);
        }
    }
}