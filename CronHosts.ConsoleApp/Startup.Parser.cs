using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CronHosts.ConsoleApp
{
    public static partial class Startup
    {
        public static void ConfigureParser(ParserSettings settings)
        {
            settings.HelpWriter = Console.Error;
        }
    }
}