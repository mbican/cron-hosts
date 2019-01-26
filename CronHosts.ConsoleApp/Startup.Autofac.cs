using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using CommandLine;
using CronHosts.Domain;

namespace CronHosts.ConsoleApp
{
    public static partial class Startup
    {
        public static void ConfigureAutofac(ContainerBuilder builder)
        {
            Domain.Startup.ConfigureAutofac(builder);
            builder.Register(a => new Parser(ConfigureParser)).AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CronHostsProgram>().As<IProgram>().PropertiesAutowired().InstancePerLifetimeScope();
        }
    }
}