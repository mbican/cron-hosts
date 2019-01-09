using System;
using System.Collections.Generic;
using System.Text;

namespace CronHosts.Domain
{
    public class Domain: IDomain
    {
        public string Execute(string content, DateTime nowUtc)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ListCrons(string content)
        {
            throw new NotImplementedException();
        }
    }
}