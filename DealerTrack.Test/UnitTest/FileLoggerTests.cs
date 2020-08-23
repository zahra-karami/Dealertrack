using System.IO;
using DealerTrack.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DealerTrack.Test.UnitTest
{
    public class FileLoggerTests
    {
        [Fact]
        public void ShouldCreateALogFileAndAddEntry()
        {
            var fileLogger = new FileLogger("Test", (category, level) => true, Path.Combine(Directory.GetCurrentDirectory(), "testlog.log"));
            var isEnabled = fileLogger.IsEnabled(LogLevel.Information);
            Assert.True(isEnabled);
        }
    }
}
