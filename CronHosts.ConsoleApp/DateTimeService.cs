using System;
using System.Collections.Generic;
using System.Text;

namespace CronHosts.ConsoleApp
{
    public class DateTimeService: IDateTimeService
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        public DateTimeOffset GetLocalTime()
        {
            return DateTimeOffset.Now;
        }
    }
}