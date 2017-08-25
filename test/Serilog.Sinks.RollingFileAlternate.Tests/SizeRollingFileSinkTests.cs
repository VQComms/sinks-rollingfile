using Xunit;
using Serilog.Formatting.Raw;
using Serilog.Sinks.RollingFileAlternate.Sinks.SizeRollingFileSink;
using Serilog.Sinks.RollingFileAlternate.Tests.Support;

namespace Serilog.Sinks.RollingFileAlternate.Tests
{
    public class SizeRollingFileSinkTests
    {
        [Fact]
        public void ItCreatesNewFileWhenSizeLimitReached()
        {
            using (var dir = new TestDirectory())
            using (var sizeRollingSink = new AlternateRollingFileSink(dir.LogDirectory, new RawFormatter(), 10))
            {
                var logEvent = Some.InformationEvent();
                sizeRollingSink.Emit(logEvent);
                Assert.Equal<uint>(1, sizeRollingSink.CurrentLogFile.LogFileInfo.Sequence);
                sizeRollingSink.Emit(logEvent);
                Assert.Equal<uint>(2, sizeRollingSink.CurrentLogFile.LogFileInfo.Sequence);
            }
        }
    }
}
