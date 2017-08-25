using System;
using Serilog.Sinks.RollingFileAlternate.Sinks.SizeRollingFileSink;
using Xunit;

namespace Serilog.Sinks.RollingFileAlternate.Tests
{
    public class GetLatestLogFileInfoOrNew
    {
        [Fact]
        public void SequenceIsOneWhenNoPreviousFile()
        {
            using (var dir = new TestDirectory())
            {
                var latest = SizeLimitedLogFileInfo.GetLatestOrNew(new DateTime(2015, 01, 15), dir.LogDirectory,
                    string.Empty);
                Assert.Equal<uint>(1, latest.Sequence);
            }
        }

        [Fact]
        public void SequenceIsEqualToTheHighestFileWritten()
        {
            var date = new DateTime(2015, 01, 15);
            using (var dir = new TestDirectory())
            {
                dir.CreateLogFile(date, 1);
                dir.CreateLogFile(date, 2);
                dir.CreateLogFile(date, 3);
                var latest = SizeLimitedLogFileInfo.GetLatestOrNew(new DateTime(2015, 01, 15), dir.LogDirectory,
                    string.Empty);
                Assert.Equal<uint>(3, latest.Sequence);
            }
        }
    }
}