using System;
using System.Collections.Generic;

namespace CronHosts.Domain
{
    public interface IDomain
    {
        string Execute(string content, DateTime nowUtc);

        IEnumerable<string> ListCrons(string content);
    }
}