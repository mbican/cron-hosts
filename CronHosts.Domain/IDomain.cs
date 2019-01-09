using System;
using System.Collections.Generic;
using System.IO;

namespace CronHosts.Domain
{
    public interface IDomain
    {
        void Execute(TextReader input, TextWriter output, DateTime nowUtc);

        IEnumerable<string> ListCrons(TextReader content);
    }
}