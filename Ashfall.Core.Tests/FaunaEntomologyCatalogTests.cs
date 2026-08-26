using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class FaunaEntomologyCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public FaunaEntomologyCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void FaunaEntomologyCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = FaunaEntomologyCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.RoachEntries.Count);
            Assert.Equal(8, catalog.MoleratEntries.Count);
            Assert.Equal(7, catalog.VultureEntries.Count);
            Assert.Equal(7, catalog.MosquitoEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void FaunaEntomologyCatalog_Roaches_Integrity()
        {
            var catalog = FaunaEntomologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.RoachEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("roach_hive_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.NestLocation));
                Assert.False(string.IsNullOrWhiteSpace(item.SpecimenMorph));
                Assert.True(item.AverageLengthCm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.ThreatRating));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetRoach("roach_hive_chitinous_lead_plate_cuticle");
            Assert.NotNull(entry);
            Assert.Equal("TRANSFORMER_VAULT_SUB_FLOOR_02", entry.NestLocation);
        }

        [Fact]
        public void FaunaEntomologyCatalog_Molerats_Integrity()
        {
            var catalog = FaunaEntomologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MoleratEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("molerat_study_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ColonyId));
                Assert.False(string.IsNullOrWhiteSpace(item.CasteClassification));
                Assert.True(item.IncisorMohsHardness > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMolerat("molerat_study_enamel_hardness_concrete_burrowing");
            Assert.NotNull(entry);
            Assert.Equal("MOLERAT_COLONY_SHAFT_09", entry.ColonyId);
        }

        [Fact]
        public void FaunaEntomologyCatalog_Vultures_Integrity()
        {
            var catalog = FaunaEntomologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.VultureEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("vulture_sighting_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ObservationPost));
                Assert.False(string.IsNullOrWhiteSpace(item.AvianMorphology));
                Assert.True(item.EstimatedWingspanMeters > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.RadiationTrackingBehavior));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetVulture("vulture_sighting_lead_shadow_thermal_glider");
            Assert.NotNull(entry);
            Assert.Equal("SURFACE_CROWS_NEST_LOOKOUT_01", entry.ObservationPost);
        }

        [Fact]
        public void FaunaEntomologyCatalog_Mosquitoes_Integrity()
        {
            var catalog = FaunaEntomologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MosquitoEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("mosquito_vector_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SiloLocationId));
                Assert.False(string.IsNullOrWhiteSpace(item.VectorSpecies));
                Assert.True(item.LarvalDensityPerLiter > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.PathogenTransmitted));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMosquito("mosquito_vector_flooded_silo_breeding_swarm");
            Assert.NotNull(entry);
            Assert.Equal("MISSILE_SILO_BRAVO_04_WATER", entry.SiloLocationId);
        }
    }
}
