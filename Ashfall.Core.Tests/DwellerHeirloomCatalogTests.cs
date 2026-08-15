using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class DwellerHeirloomCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void DwellerHeirlooms_LoadsAll30CanonicalKeepsakes()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "dweller_heirlooms_master.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new DwellerHeirloomCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(30, catalog.AllHeirlooms.Count);

            // Test first heirloom (Dr. Vel's Stethoscope)
            var h1 = catalog.GetById("heirloom_01_vel_stethoscope_silver");
            Assert.NotNull(h1);
            Assert.Equal("survivor_dr_irina_vel", h1.owner_id);
            Assert.Contains("Silver Acoustic Stethoscope", h1.item_name);
            Assert.Equal(4, h1.daily_morale_modifier);
            Assert.Contains("steady, galloping rhythm", h1.sensory_memory_text);

            // Test Sonya's keepsakes
            var sonyaItems = catalog.GetByOwner("survivor_sonya_vel");
            Assert.Equal(2, sonyaItems.Count);

            // Test final heirloom (Sonya's Seed Box)
            var h30 = catalog.GetById("heirloom_30_sonya_grandfather_carved_seed_box");
            Assert.NotNull(h30);
            Assert.Equal(10, h30.daily_morale_modifier);
            Assert.Contains("Century Tree", h30.sensory_memory_text);

            // Test tag search
            var music = catalog.GetByTag("music");
            Assert.True(music.Count >= 2);
        }

        [Fact]
        public void DwellerHeirlooms_AllEntriesHaveValidFieldsAndMemoryTexts()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "dweller_heirlooms_master.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new DwellerHeirloomCatalog();
            catalog.Load(json, serializer);

            foreach (var h in catalog.AllHeirlooms)
            {
                Assert.False(string.IsNullOrWhiteSpace(h.heirloom_id), "Missing heirloom_id");
                Assert.False(string.IsNullOrWhiteSpace(h.owner_id), $"Missing owner_id on {h.heirloom_id}");
                Assert.False(string.IsNullOrWhiteSpace(h.owner_name), $"Missing owner_name on {h.heirloom_id}");
                Assert.False(string.IsNullOrWhiteSpace(h.item_name), $"Missing item_name on {h.heirloom_id}");
                Assert.False(string.IsNullOrWhiteSpace(h.pre_war_origin), $"Missing origin on {h.heirloom_id}");
                Assert.False(string.IsNullOrWhiteSpace(h.physical_condition), $"Missing condition on {h.heirloom_id}");
                Assert.True(h.daily_morale_modifier > 0, $"Invalid morale modifier on {h.heirloom_id}");
                Assert.True(h.trauma_trigger_risk_percent >= 0 && h.trauma_trigger_risk_percent <= 100, $"Invalid trauma risk on {h.heirloom_id}");
                Assert.False(string.IsNullOrWhiteSpace(h.sensory_memory_text), $"Missing sensory memory on {h.heirloom_id}");
                Assert.True(h.sensory_memory_text.Length > 30, $"Memory text too short on {h.heirloom_id}");
                Assert.False(string.IsNullOrWhiteSpace(h.item_loss_event_text), $"Missing loss text on {h.heirloom_id}");
                Assert.NotNull(h.tags);
                Assert.True(h.tags.Length > 0, $"Tags empty on {h.heirloom_id}");
            }
        }
    }
}
