using System.IO;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>Runs the Plan 25 vertical-slice demo (real data authority) as a
    /// unit-test gate — the same checks the host --faction-ecology-selftest verb runs.</summary>
    public class FactionEcologySelftestTests
    {
        [Fact]
        public void FactionEcologyVerticalSlice_PassesEndToEnd()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return; // CI without the data tree

            var report = FactionEcologyHeadlessDemo.Run(dataDir);
            Assert.True(report.FailedCount == 0,
                $"faction ecology slice failures: {string.Join("; ", report.Checks.FindAll(c => !c.Passed).ConvertAll(c => c.Name))}");
        }

        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }
    }
}
