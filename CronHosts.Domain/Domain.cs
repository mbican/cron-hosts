using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CronHosts.Domain
{
    public class Domain: IDomain
    {
        public void Execute(TextReader input, TextWriter output, DateTime nowUtc)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ListCrons(TextReader content)
        {
            throw new NotImplementedException();
        }
    }
}