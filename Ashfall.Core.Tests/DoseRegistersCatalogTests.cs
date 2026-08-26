using System.IO;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DoseRegistersCatalogTests
    : CatalogTestBase{
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
        public void Load_FindsFourBandsThreePlansThreeGuesses()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseRegistersCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(4, catalog.bands.Count);
            Assert.Equal(3, catalog.plans.Count);
            Assert.Equal(3, catalog.guesses.Count);
            Assert.Equal("band_green", catalog.bands[0].id);
            Assert.Equal("band_black", catalog.bands[3].id);
        }

        [Fact]
        public void Load_FindsTheFourAntagonists()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseRegistersCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(4, catalog.npcs.Count);
            Assert.Contains(catalog.npcs, n => n.id == "npc_dr_irina_vel");
            Assert.Contains(catalog.npcs, n => n.id == "npc_wyn_omah");
            Assert.Contains(catalog.npcs, n => n.id == "npc_piet_abar");
            Assert.Contains(catalog.npcs, n => n.id == "npc_saria_voss");
            foreach (var n in catalog.npcs)
            {
                Assert.False(string.IsNullOrEmpty(n.disposition));
                Assert.False(string.IsNullOrEmpty(n.action));
                // Binding parity: snake_case JSON keys must reach the DTO fields
                // (Unity's JsonUtility binds these case-insensitively; the Godot
                // serializer needs the exact snake_case names).
                Assert.False(string.IsNullOrEmpty(n.action_label),
                    n.id + " action_label unbound");
            }
        }

        [Fact]
        public void Load_BandThresholdsBind()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseRegistersCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(600f, catalog.bands[3].threshold_msv);
            Assert.Equal(300f, catalog.bands[2].threshold_msv);
            Assert.Equal(100f, catalog.bands[1].threshold_msv);
        }

        [Fact]
        public void BandLabel_MapsCoreBandsToVocabulary()
        {
            var catalog = DoseRegistersCatalogLoader.Load(
                FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            if (catalog.bands.Count == 0) return;
            Assert.Equal("Green", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandGreen));
            Assert.Equal("Amber", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandAmber));
            Assert.Equal("Red", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandRed));
            Assert.Equal("Black", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandBlack));
        }

        [Fact]
        public void Load_MissingDirectoryReturnsEmptyCatalog()
        {
            var catalog = DoseRegistersCatalogLoader.Load(
                "/nonexistent/path", new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(catalog.bands);
            Assert.Empty(catalog.npcs);
        }

        [Fact]
        public void Characters_RegisterTheFourAntagonists()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string raw = fileIO.ReadAllText(fileIO.Combine(dataDir, "characters.json"));
            var chars = CatalogLocator.LoadWrappedList<CharacterEntry>(raw, SystemTextJsonSerializer.Options);
            int found = 0;
            foreach (var c in chars)
                if (c.id == "npc_dr_irina_vel" || c.id == "npc_wyn_omah" ||
                    c.id == "npc_piet_abar" || c.id == "npc_saria_voss")
                    found++;
            Assert.Equal(4, found);
        }

        private class CharacterEntry
        {
            public string id = string.Empty;
        }
    }
}
