using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class EndingsHeadlessDemoTests
    {
        [Fact]
        public void HeadlessDemoPasses()
        {
            var report = EndingsHeadlessDemo.Run();
            Assert.True(report.Passed, report.Summary);
            Assert.Equal(0, report.FailedCount);
            Assert.True(report.Checks.Count >= 10);
        }

        [Fact]
        public void MasterListIsStableAndComplete()
        {
            Assert.Equal(5, HoldfastEndings.All.Length);
            Assert.Equal("ending_holdfast_schedule", HoldfastEndings.Schedule);
            Assert.Equal("ending_holdfast_reserve", HoldfastEndings.Reserve);
            Assert.Equal("ending_holdfast_dark_road", HoldfastEndings.DarkRoad);
            Assert.Equal("ending_holdfast_tender", HoldfastEndings.Tender);
            Assert.Equal("ending_holdfast_white", HoldfastEndings.White);
            Assert.False(HoldfastEndings.IsKnown(HoldfastEndings.None), "empty id is not a known ending");
        }

        [Fact]
        public void DemoCarriesDarkRoadEndingInEnvelope()
        {
            var report = EndingsHeadlessDemo.Run();
            Assert.True(report.Save != null);
            Assert.Equal("ending_holdfast_dark_road", report.Save.quests.endingId);
            Assert.Equal(HoldfastSave.CurrentSaveVersion, report.Save.saveVersion);
        }
    }
}
