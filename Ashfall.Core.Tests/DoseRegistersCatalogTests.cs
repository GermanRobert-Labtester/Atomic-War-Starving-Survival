// SPDX-License-Identifier: MIT
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
            // Plan 90 expanded bands to 12 and plans to 8; assert minimums for backward compat.
            Assert.True(catalog.bands.Count >= 4, $"Expected >= 4 bands, got {catalog.bands.Count}");
            Assert.True(catalog.plans.Count >= 3, $"Expected >= 3 plans, got {catalog.plans.Count}");
            Assert.Equal(3, catalog.guesses.Count);
            Assert.Equal("band_green", catalog.bands[0].id);
            // band_black is now at index 9 (after Plan 90 expansion); use Contains.
            Assert.Contains(catalog.bands, b => b.id == "band_black");
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
            // Look up by id — Plan 90 changed indices when 8 new bands were inserted.
            var black = catalog.bands.Find(b => b.id == "band_black");
            var red   = catalog.bands.Find(b => b.id == "band_red");
            var amber = catalog.bands.Find(b => b.id == "band_amber");
            Assert.NotNull(black); Assert.Equal(600f, black!.threshold_msv);
            Assert.NotNull(red);   Assert.Equal(300f, red!.threshold_msv);
            Assert.NotNull(amber); Assert.Equal(100f, amber!.threshold_msv);
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
