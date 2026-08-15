using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class MetallurgyToolingCatalogTests
    {
        private readonly string _narrativeDir;

        public MetallurgyToolingCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void MetallurgyToolingCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = MetallurgyToolingCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.SlagEntries.Count);
            Assert.Equal(8, catalog.ToolEntries.Count);
            Assert.Equal(7, catalog.GearEntries.Count);
            Assert.Equal(7, catalog.BulletEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void MetallurgyToolingCatalog_Slag_Integrity()
        {
            var catalog = MetallurgyToolingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SlagEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("slag_leach_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.FurnaceUnitId));
                Assert.True(item.MeltTemperatureCelsius > 0);
                Assert.True(item.SlagBasicityRatio > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.MetallurgicalDefect));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSlag("slag_leach_high_alumina_refractory_erosion");
            Assert.NotNull(entry);
            Assert.Equal("CUPOLA_STACK_FOUNDRY_01", entry.FurnaceUnitId);
        }

        [Fact]
        public void MetallurgyToolingCatalog_Tools_Integrity()
        {
            var catalog = MetallurgyToolingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.ToolEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("carbide_tool_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ToolIdentifier));
                Assert.False(string.IsNullOrWhiteSpace(item.CarbideGrade));
                Assert.True(item.CuttingSpeedMMin > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.WearMechanism));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetTool("carbide_tool_crater_wear_rake_face_breakout");
            Assert.NotNull(entry);
            Assert.Equal("TURNING_INSERT_CNMG_432", entry.ToolIdentifier);
        }

        [Fact]
        public void MetallurgyToolingCatalog_Gears_Integrity()
        {
            var catalog = MetallurgyToolingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.GearEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("gear_quench_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.GearComponentId));
                Assert.False(string.IsNullOrWhiteSpace(item.SteelAlloyGrade));
                Assert.True(item.CaseHardnessHrc > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.QuenchingMedium));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetGear("gear_quench_retained_austenite_flank_spalling");
            Assert.NotNull(entry);
            Assert.Equal("MAIN_TRANSMISSION_BULL_GEAR_01", entry.GearComponentId);
        }

        [Fact]
        public void MetallurgyToolingCatalog_Bullets_Integrity()
        {
            var catalog = MetallurgyToolingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BulletEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("bullet_alloy_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.AlloyBatchCode));
                Assert.True(item.LeadPercentage > 0);
                Assert.True(item.BrinellHardnessBhn > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetBullet("bullet_alloy_linotype_antimony_enrichment");
            Assert.NotNull(entry);
            Assert.Equal("ALLOY_BATCH_LINO_084", entry.AlloyBatchCode);
        }
    }
}
