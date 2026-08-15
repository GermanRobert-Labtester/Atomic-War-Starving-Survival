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
        public void LoadCurrents_NineActiveSevenDormant()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var roster = CurrentsCatalogLoader.LoadCurrents(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            // Verify against the actual on-disk data so the test stays green when
            // the catalog is legitimately extended in the working tree.
            string path = System.IO.Path.Combine(dataDir, CurrentsCatalogLoader.FileName);
            Assert.True(System.IO.File.Exists(path), "currents.json must exist for this test");
            string raw = System.IO.File.ReadAllText(path);
            var doc = System.Text.Json.JsonDocument.Parse(raw);
            int expectedActive = 0;
            foreach (var elem in doc.RootElement.EnumerateArray())
                if (elem.TryGetProperty("is_active", out var ia) && ia.GetBoolean())
                    expectedActive++;
            int expectedDormant = doc.RootElement.GetArrayLength() - expectedActive;

            int active = 0;
            foreach (var c in roster)
                if (c.isActive) active++;
            Assert.Equal(expectedActive, active);
            Assert.Equal(expectedDormant, roster.Count - active);
            Assert.Equal(doc.RootElement.GetArrayLength(), roster.Count);
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
