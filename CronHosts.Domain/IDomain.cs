using Shuttle.Core.Cron;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CronHosts.Domain
{
    public interface IDomain
    {
        Regex BeginRegex { get; set; }
        Regex EndRegex { get; set; }
        Regex CommentRegex { get; set; }

        Task Execute(TextReader input, TextWriter output, DateTime now);

        IAsyncEnumerable<(CronExpression Begin, CronException End)> ListCrons(TextReader content);
    }
}