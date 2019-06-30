using CommandLine;
using CronHosts.Domain;
using Efaut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CronHosts.ConsoleApp
{
    public class CronHostsProgram: IProgram
    {
        #region Dependencies

#nullable disable

        public Lazy<Parser> ParserLazy { get; set; }
        protected Parser Parser => ParserLazy?.Value;

        public Lazy<Random> RandomLazy { get; set; }
        protected Random Random => RandomLazy?.Value;

        public Lazy<IDomain> DomainLazy { get; set; }
        protected IDomain Domain => DomainLazy?.Value;

        public Lazy<IDateTimeService> DateTimeServiceLazy { get; set; }
        protected IDateTimeService DateTimeService => DateTimeServiceLazy?.Value;

#nullable restore

        #endregion Dependencies

        public sealed class Arguments: ValueObject
        {
            [Value(0)]
            public string? File { get; }

            public Arguments(string? file)
            {
                File = file;
            }
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
                await DoRun(arguments);
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

        protected async Task DoRun(Arguments arguments)
        {
            // if file name has been provided
            if (arguments.File == null)
            { // use standard input
                using (var input = Console.OpenStandardInput())
                using (var reader = new StreamReader(input))
                using (var output = Console.OpenStandardOutput())
                using (var writer = new StreamWriter(output))
                    await Domain.Execute(reader, writer, DateTimeService.GetLocalTime().DateTime);
            }
            else
            { // use file
                var existingFile = arguments.File;
                // create name for a temp file
                var random = Random.Next();
                var tempFile = $"{existingFile}_{random:x8}.tmp";
                // create name for a backup file
                var bakFile = $"{existingFile}_{random:x8}.bak";
                try
                {
                    // copy existingFile to tempFile so it has original metadata
                    File.Copy(existingFile, tempFile);
                    // open existing file for reading
                    using (var reader = new StreamReader(existingFile, true))
                    // open temp file for writing
                    using (var writer = new StreamWriter(tempFile))
                        // execute cronhosts processes existing file into temp file
                        await Domain.Execute(reader, writer, DateTimeService.GetLocalTime().DateTime);
                    // swap completed temp file for existing file while renaming existing file to bakFile in case of something goes wrong
                    File.Replace(tempFile, existingFile, bakFile);
                    // after successful swap delete the original file renamed to bakFile
                    File.Delete(bakFile);
                }
                catch
                {   // in case of an error
                    // try restore original file from back up
                    try { File.Move(bakFile, existingFile); } catch { }
                    try
                    {
                        // if original file exists
                        if (File.Exists(existingFile))
                            // delete temp file
                            File.Delete(tempFile);
                        else
                            // otherwise rename tempFile to original file
                            File.Move(tempFile, existingFile);
                    }
                    catch { }
                    throw;
                }
            }
        }

        public static IReadOnlyDictionary<ExitCode, string> ErrorMessages = new Dictionary<ExitCode, string>
        {
            [ExitCode.Success] = "Success",
            [ExitCode.ArgumentError] = "ArgumentError",
            [ExitCode.UnknownError] = "UnknownError",
        };

        public class CronHostsFatalException: ApplicationException
        {
            public ExitCode ExitCode { get; protected set; }

            public CronHostsFatalException(ExitCode exitCode) : base(ErrorMessages[exitCode])
            {
                ExitCode = exitCode;
            }

            public CronHostsFatalException(ExitCode exitCode, string message) : base(message)
            {
                ExitCode = exitCode;
            }

            public CronHostsFatalException(ExitCode exitCode, Exception innerExcpetion) : base(ErrorMessages[exitCode], innerExcpetion)
            {
                ExitCode = exitCode;
            }

            public CronHostsFatalException(ExitCode exitCode, string message, Exception innerExcpetion) : base(message, innerExcpetion)
            {
                ExitCode = exitCode;
            }
        }
    }
}