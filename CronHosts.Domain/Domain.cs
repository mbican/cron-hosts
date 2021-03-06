﻿using Shuttle.Core.Cron;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CronHosts.Domain
{
    public class Domain: IDomain
    {
        public const string BeginRegexDefault = @"^\s*#cronhosts((?:\s+(?:[-0-9a-z*,?#/])+){5})\s+;((?:\s+(?:[-0-9a-z*,?#/])+){5})\s*$";
        public const string EndRegexDefault = @"^\s*#endcronhosts\s*$";
        public const string CommentDefault = "#cronhostsout ";
        public const string CommentRegexDefault = @"^#cronhostsout\s?";

        public Regex BeginRegex { get; set; }
        public Regex EndRegex { get; set; }
        public Regex CommentRegex { get; set; }
        public string Comment { get; set; }

        public Domain()
        {
            BeginRegex = new Regex(BeginRegexDefault, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(10));
            EndRegex = new Regex(EndRegexDefault, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(10));
            CommentRegex = new Regex(CommentRegexDefault, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(10));
            Comment = CommentDefault;
        }

        public async Task Execute(TextReader input, TextWriter output, DateTime now)
        {
            string? lineOrNull;
            // we are inside #cronhosts block
            var inside = false;
            // wheter to uncomment or comment out lines inside #cronhosts block
            var uncomment = false;
            var lineNumber = 0;
            while ((lineOrNull = await input.ReadLineAsync()) != null)
            {
                var line = lineOrNull!;
                lineNumber++;
                if (!inside)
                {
                    var match = BeginRegex.Match(line);
                    if (match.Success)
                    {
                        var beginCron = new CronExpression(match.Groups[1].Value, now);
                        var endCron = new CronExpression(match.Groups[2].Value, now);
                        inside = true;
                        var prevEnd = endCron.PreviousOccurrence();
                        var prevBegin = beginCron.PreviousOccurrence();
                        if (endCron.GetNextOccurrence(prevEnd) <= now)
                            endCron.NextOccurrence();
                        if (beginCron.GetNextOccurrence(prevBegin) <= now)
                            beginCron.NextOccurrence();
                        uncomment = endCron.CurrentDate < beginCron.CurrentDate;
                    }
                    await output.WriteLineAsync(line);
                }
                else
                {
                    if (EndRegex.Match(line).Success)
                    {
                        inside = false;
                        await output.WriteLineAsync(line);
                    }
                    else if (uncomment)
                    {
                        await output.WriteLineAsync(UncommentLine(line));
                    }
                    else
                    {
                        await output.WriteLineAsync(CommentOutLine(line));
                    }
                }
            }
        }

        public async IAsyncEnumerable<(CronExpression Begin, CronException End)> ListCrons(TextReader content)
        {
            yield return (null, null);
        }

        protected string UncommentLine(string line)
        {
            var match = Match.Empty;
            var begining = 0;
            while ((match = CommentRegex.Match(line, begining)).Success)
            {
                begining += match.Length;
            }
            return line.Substring(begining);
        }

        protected string CommentOutLine(string line)
        {
            // if line is already a comment
            if (CommentRegex.Match(line).Success)
                // keep line because it is already a comment
                return line;
            else
                // comment out line
                return Comment + line;
        }
    }
}