using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class IndustrialRuinsCatalogTests
    {
        private readonly string _narrativeDir;

        public IndustrialRuinsCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void IndustrialRuinsCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = IndustrialRuinsCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.DraglineEntries.Count);
            Assert.Equal(8, catalog.SubstationEntries.Count);
            Assert.Equal(7, catalog.LocomotiveEntries.Count);
            Assert.Equal(7, catalog.PipelineEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void IndustrialRuinsCatalog_Draglines_Integrity()
        {
            var catalog = IndustrialRuinsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.DraglineEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("dragline_ruin_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.MachineDesignation));
                Assert.True(item.OperatingWeightTons > 0);
                Assert.True(item.BoomLengthMeters > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.StructuralCondition));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetDragline("dragline_ruin_monighan_walking_goliath");
            Assert.NotNull(entry);
            Assert.Equal("BUCYRUS_ERIE_2570W_GOLIATH", entry.MachineDesignation);
        }

        [Fact]
        public void IndustrialRuinsCatalog_Substations_Integrity()
        {
            var catalog = IndustrialRuinsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SubstationEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("substation_fire_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SubstationId));
                Assert.True(item.TransformerMvaRating > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.ContaminantCombustionByproduct));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSubstation("substation_fire_500kv_pcb_pyranol_conflagration");
            Assert.NotNull(entry);
            Assert.Equal("VALLEY_GRID_SUBSTATION_500KV", entry.SubstationId);
        }

        [Fact]
        public void IndustrialRuinsCatalog_Locomotives_Integrity()
        {
            var catalog = IndustrialRuinsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.LocomotiveEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("locomotive_armored_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.LocomotiveId));
                Assert.False(string.IsNullOrWhiteSpace(item.LocomotiveType));
                Assert.True(item.ArmorThicknessMm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.CurrentOperationalStatus));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetLocomotive("locomotive_armored_steel_plow_dreadnought");
            Assert.NotNull(entry);
            Assert.Equal("ARMORED_ENGINE_704_IRON_WOLF", entry.LocomotiveId);
        }

        [Fact]
        public void IndustrialRuinsCatalog_Pipelines_Integrity()
        {
            var catalog = IndustrialRuinsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.PipelineEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("pipeline_sabotage_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.PipelineSector));
                Assert.True(item.PipeDiameterInches > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.SabotageMethod));
                Assert.False(string.IsNullOrWhiteSpace(item.EnvironmentalHazardSeverity));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetPipeline("pipeline_sabotage_shaped_charge_crude_breach");
            Assert.NotNull(entry);
            Assert.Equal("TRANSLINE_CRUDE_CORRIDOR_MILE_42", entry.PipelineSector);
        }
    }
}
