using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CronHosts.ConsoleApp
{
    public class CronHostsProgram: IProgram
    {
        #region Dependencies

        public Lazy<Parser> ParserLazy { get; set; }
        protected Parser Parser => ParserLazy?.Value;

        #endregion Dependencies

        public class Arguments
        {
            [Value(0)]
            public string File { get; set; }
        }

        public enum ExitCode: int
        {
            Success = 0,
            ArgumentError = 1,
            UnknownError = 1000,
        }

        public async Task<int> Run(string[] args)
        {
            var parserResult = Parser.ParseArguments<Arguments>(args);
            if (parserResult is Parsed<Arguments> parsed)
            {
                var arguments = parsed.Value;
                return (int)ExitCode.Success;
            }
            else if (parserResult is NotParsed<Arguments> notParsed)
            {
                var errors = notParsed.Errors;
                return (int)ExitCode.ArgumentError;
            }
            else
            {
                return (int)ExitCode.UnknownError;
            }
        }
    }
}