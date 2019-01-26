using System;
using System.Collections.Generic;
using System.Text;

namespace CronHosts.ConsoleApp
{
    public interface IDateTimeService
    {
        DateTime GetUtcNow();
    }
}