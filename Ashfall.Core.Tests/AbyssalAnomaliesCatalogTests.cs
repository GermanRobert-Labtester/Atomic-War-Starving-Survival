using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class AbyssalAnomaliesCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public AbyssalAnomaliesCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void AbyssalAnomaliesCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.HydrophoneEntries.Count);
            Assert.Equal(7, catalog.BoreholeEntries.Count);
            Assert.Equal(8, catalog.CryopodEntries.Count);
            Assert.Equal(7, catalog.SaltMineEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void AbyssalAnomaliesCatalog_Hydrophone_Integrity()
        {
            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.HydrophoneEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("hydrophone_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BuoyCallsign));
                Assert.False(string.IsNullOrWhiteSpace(item.SignalClassification));
                Assert.True(item.AcousticFrequencyHz > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetHydrophone("hydrophone_submarine_cavitation_ghost");
            Assert.NotNull(entry);
            Assert.Equal("HYDRO-BUOY-NORTH-02", entry.BuoyCallsign);
        }

        [Fact]
        public void AbyssalAnomaliesCatalog_Borehole_Integrity()
        {
            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BoreholeEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("borehole_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BoreholeId));
                Assert.True(item.DepthMeters > 0);
                Assert.True(item.TemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetBorehole("borehole_magma_boundary_temperature_spike");
            Assert.NotNull(entry);
            Assert.Equal("DEEP-WELL-SUMP-08", entry.BoreholeId);
        }

        [Fact]
        public void AbyssalAnomaliesCatalog_Cryopod_Integrity()
        {
            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CryopodEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("cryopod_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.PodId));
                Assert.False(string.IsNullOrWhiteSpace(item.SubjectDesignation));
                Assert.False(string.IsNullOrWhiteSpace(item.SystemAlert));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCryopod("cryopod_coolant_circuit_boiloff");
            Assert.NotNull(entry);
            Assert.Equal("CRYO-CHAMBER-VAULT-14-POD-04", entry.PodId);
        }

        [Fact]
        public void AbyssalAnomaliesCatalog_SaltMine_Integrity()
        {
            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SaltMineEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("salt_mine_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.MineGallery));
                Assert.False(string.IsNullOrWhiteSpace(item.RockMedium));
                Assert.False(string.IsNullOrWhiteSpace(item.RecorderIdentity));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSaltMine("salt_mine_blind_mule_memorial");
            Assert.NotNull(entry);
            Assert.Equal("LEVEL_02_HAULAGE_STABLE", entry.MineGallery);
        }
    }
}
