using Shuttle.Core.Cron;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CronHosts.Domain
{
    public interface IDomain
    {
        Task Execute(TextReader input, TextWriter output, DateTime nowUtc);

        IAsyncEnumerable<(CronExpression Begin, CronException End)> ListCrons(TextReader content);
    }
}