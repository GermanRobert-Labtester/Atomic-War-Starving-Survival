using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class BrineWaterHeadlessDemoTests
    {
        [Fact]
        public void HeadlessDemoPasses()
        {
            var report = BrineWaterHeadlessDemo.Run();
            Assert.True(report.Passed, report.Summary);
            Assert.Equal(0, report.FailedCount);
            Assert.True(report.Checks.Count >= 15);
        }

        [Fact]
        public void DemoPreservesTrippedPipelineState()
        {
            var report = BrineWaterHeadlessDemo.Run();
            Assert.True(report.Brine != null);
            // The demo drives the plant into a trip, then repairs it; the report
            // must carry the post-repair state so a host can restore it verbatim.
            Assert.True(report.Brine.membraneSaved);
            Assert.False(report.Brine.steamTripped);
            Assert.True(report.Brine.membraneIntegrity >= 40f);
        }
    }
}
