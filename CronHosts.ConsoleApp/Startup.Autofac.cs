using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using CronHosts.Domain;

namespace CronHosts.Runner
{
    public static partial class Startup
    {
        public static void ConfigureAutofac(ContainerBuilder builder)
        {
            Domain.Startup.ConfigureAutofac(builder);
        }
    }
}