using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace CronHosts.ConsoleApp
{
    public static partial class Startup
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            ConfigureAutofac(builder);
            return builder.Build();
        }
    }
}
