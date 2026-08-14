using System.IO;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CurrentsCatalogTests
    {
        private static string FindDataDir()
        {
            string dataDir = string.Empty;
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) { dataDir = candidate; break; }
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return dataDir;
        }

        [Fact]
        public void LoadCurrents_FindsAllFifteen()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var roster = CurrentsCatalogLoader.LoadCurrents(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(roster.Count >= 15, $"Expected >= 15 currents, got {roster.Count}");
        }

        [Fact]
        public void LoadCurrents_HydroBaronsIsTheFifteenth()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var roster = CurrentsCatalogLoader.LoadCurrents(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var hydro = roster.Find(c => c.id == "faction_hydro_barons");
            Assert.NotNull(hydro);
            Assert.Equal("The Coastal Hydro-Barons", hydro.displayName);
            Assert.Equal("conditional", hydro.alignment);
            Assert.Equal("the_coast", hydro.homeRegion);
            Assert.False(hydro.isActive);
            Assert.Contains("desalination_access", hydro.offers);
            Assert.Contains("item_hydro_baron_queue_chit", hydro.wants);
        }

        [Fact]
        public void LoadCurrents_TenOfFifteenDormant()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var roster = CurrentsCatalogLoader.LoadCurrents(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            int active = 0;
            foreach (var c in roster)
                if (c.isActive) active++;
            Assert.True(active >= 3, $"Expected at least 3 active currents, got {active}");
            Assert.True(roster.Count - active >= 10, "At least 10 currents stay dormant until wired.");
        }

        [Fact]
        public void LoadCurrents_MissingDirectoryReturnsEmpty()
        {
            var roster = CurrentsCatalogLoader.LoadCurrents(
                "/nonexistent/path", new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(roster);
        }
    }
}
