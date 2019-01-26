using CronHosts.Domain;
using System;
using System.IO;
using System.Text;
using Xunit;
using Should;

namespace CronHosts.Tests.Unit
{
    public class DomainTests
    {
        protected readonly IDomain Domain;

        public DomainTests()
        {
            Domain = new CronHosts.Domain.Domain();
        }

        [Fact]
        public void Execute_CommentsOut()
        {
            Execute(@"
#cronhosts 0 16 * * * ; 0 20 * * *
127.0.0.1 www.facebook.com
#endcronhosts
            ", @"
#cronhosts 0 16 * * * ; 0 20 * * *
#cronhostsout 127.0.0.1 www.facebook.com
#endcronhosts
            ",
            new DateTime(2010, 1, 1, 13, 0, 0));
        }

        [Fact]
        public void Execute_KeepsUncommented()
        {
            Execute(@"
#cronhosts 0 16 * * * ; 0 20 * * *
127.0.0.1 www.facebook.com
#endcronhosts
            ", @"
#cronhosts 0 16 * * * ; 0 20 * * *
127.0.0.1 www.facebook.com
#endcronhosts
            ",
            new DateTime(2010, 1, 1, 17, 0, 0));
        }

        [Fact]
        public void Execute_Uncomments()
        {
            Execute(@"
#cronhosts 0 16 * * * ; 0 20 * * *
#cronhostsout 127.0.0.1 www.facebook.com
#endcronhosts
            ", @"
#cronhosts 0 16 * * * ; 0 20 * * *
127.0.0.1 www.facebook.com
#endcronhosts
            ",
            new DateTime(2010, 1, 1, 16, 0, 0));
        }

        [Fact]
        public void Execute_KeepsCommented()
        {
            Execute(@"
#cronhosts 0 16 * * * ; 0 20 * * *
#cronhostsout 127.0.0.1 www.facebook.com
#endcronhosts
            ", @"
#cronhosts 0 16 * * * ; 0 20 * * *
#cronhostsout 127.0.0.1 www.facebook.com
#endcronhosts
            ",
            new DateTime(2010, 1, 1, 20, 0, 0));
        }

        protected void Execute(string input, string expectedOutput, DateTime dateTimeUtc)
        {
            var outputBuilder = new StringBuilder();
            using (var inputReader = new StringReader(input))
            using (var outputWriter = new StringWriter(outputBuilder))
            {
                Domain.Execute(inputReader, outputWriter, dateTimeUtc);
            }
            var output = outputBuilder.ToString();
            if (output != expectedOutput)
            {
                var expectedOutput2 = expectedOutput + Environment.NewLine;
                if (output == expectedOutput2)
                    expectedOutput = expectedOutput2;
            }
            output.ShouldEqual(expectedOutput);
        }
    }
}