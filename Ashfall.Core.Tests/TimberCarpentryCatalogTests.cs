using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class TimberCarpentryCatalogTests
    {
        private readonly string _narrativeDir;

        public TimberCarpentryCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void TimberCarpentryCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = TimberCarpentryCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.TreatmentEntries.Count);
            Assert.Equal(8, catalog.ShoringEntries.Count);
            Assert.Equal(7, catalog.RotEntries.Count);
            Assert.Equal(7, catalog.MortiseEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void TimberCarpentryCatalog_Treatment_Integrity()
        {
            var catalog = TimberCarpentryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.TreatmentEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("timber_creosote_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.TreatmentRetortId));
                Assert.False(string.IsNullOrWhiteSpace(item.WoodSpeciesTreated));
                Assert.True(item.PenetrationDepthMm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetTreatment("timber_creosote_full_cell_bethell_process");
            Assert.NotNull(entry);
            Assert.Equal("AUTOCLAVE_PRESSURE_CYLINDER_01", entry.TreatmentRetortId);
        }

        [Fact]
        public void TimberCarpentryCatalog_Shoring_Integrity()
        {
            var catalog = TimberCarpentryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.ShoringEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("square_set_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.StopeLocationId));
                Assert.False(string.IsNullOrWhiteSpace(item.TimberFramingSystem));
                Assert.True(item.MeasuredRockPressureMpa > 0);
                Assert.True(item.SetDeflectionMm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetShoring("square_set_heavy_ground_crushing_horn_failure");
            Assert.NotNull(entry);
            Assert.Equal("DEEP_VEIN_STOPE_LEVEL_8", entry.StopeLocationId);
        }

        [Fact]
        public void TimberCarpentryCatalog_Rot_Integrity()
        {
            var catalog = TimberCarpentryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.RotEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("dry_rot_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.InfestationSiteId));
                Assert.False(string.IsNullOrWhiteSpace(item.FungalSpeciesIdentified));
                Assert.True(item.TimberMoistureContentPct > 0);
                Assert.True(item.AffectedAreaSqMeters > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetRot("dry_rot_serpula_hyphae_brick_masonry_tunnel");
            Assert.NotNull(entry);
            Assert.Equal("SUB_BASEMENT_BUNKER_WALL_02", entry.InfestationSiteId);
        }

        [Fact]
        public void TimberCarpentryCatalog_Mortise_Integrity()
        {
            var catalog = TimberCarpentryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MortiseEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("mortise_tenon_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.FramingAssemblyId));
                Assert.False(string.IsNullOrWhiteSpace(item.JointGeometryType));
                Assert.True(item.JointLoadKilonewtons > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMortise("mortise_tenon_drawbore_pin_shear_green_oak");
            Assert.NotNull(entry);
            Assert.Equal("MAIN_ASSEMBLY_HALL_BENT_01", entry.FramingAssemblyId);
        }
    }
}
