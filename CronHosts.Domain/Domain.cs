using Shuttle.Core.Cron;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CronHosts.Domain
{
    public class Domain : IDomain
    {
        public async Task Execute(TextReader input, TextWriter output, DateTime nowUtc)
        {
            var line = (string?)null;
            while ((line = await input.ReadLineAsync()) != null)
            {
            }
        }

        public async IAsyncEnumerable<(CronExpression Begin, CronException End)> ListCrons(TextReader content)
        {
            yield return (null, null);
        }
    }
}