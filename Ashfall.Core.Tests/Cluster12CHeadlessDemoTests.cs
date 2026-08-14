using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class Cluster12CHeadlessDemoTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void HeadlessDemoPasses()
        {
            var report = Cluster12CHeadlessDemo.Run(DataDir());
            Assert.True(report.Passed, report.Summary);
            Assert.Equal(0, report.FailedCount);
            Assert.True(report.Checks.Count >= 15);
        }

        [Fact]
        public void DemoCarriesCurrentVersionEnvelope()
        {
            var report = Cluster12CHeadlessDemo.Run(DataDir());
            Assert.True(report.Save != null);
            Assert.Equal(HoldfastSave.CurrentSaveVersion, report.Save.saveVersion);
            Assert.True(report.Save.census.order12cActive);
            Assert.True(report.Save.quests.quests.Count > 0);
        }
    }
}
