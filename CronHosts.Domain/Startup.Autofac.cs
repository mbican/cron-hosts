using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using CronHosts.Domain;

namespace CronHosts.Domain
{
    public static partial class Startup
    {
        public static void ConfigureAutofac(ContainerBuilder builder)
        {
            builder.RegisterType<Domain>().As<IDomain>().PropertiesAutowired();
        }
    }
}