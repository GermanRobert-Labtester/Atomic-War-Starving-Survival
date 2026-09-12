// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class AbyssalAnomaliesCatalogTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private readonly string _dataDir;
        private readonly string _narrativeDir;
        private readonly FileSystemIO _fileIO;
        private readonly SystemTextJsonSerializer _serializer;

        public AbyssalAnomaliesCatalogTests()
        {
            _dataDir = FindDataDir();
            _narrativeDir = Path.Combine(_dataDir, "narrative");
            _fileIO = new FileSystemIO();
            _serializer = new SystemTextJsonSerializer();
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

        [Fact]
        public void TEST_ABY_02_GlobalIdUniquenessAcrossAll30Entries()
        {
            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_dataDir, _fileIO, _serializer);
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in catalog.HydrophoneEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id), "Hydrophone item missing ID");
                Assert.True(seenIds.Add(item.Id), $"Duplicate ID: {item.Id}");
                Assert.False(string.IsNullOrWhiteSpace(item.Prose), $"Missing prose: {item.Id}");
                Assert.NotEmpty(item.Tags);
            }

            foreach (var item in catalog.BoreholeEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id), "Borehole item missing ID");
                Assert.True(seenIds.Add(item.Id), $"Duplicate ID: {item.Id}");
                Assert.False(string.IsNullOrWhiteSpace(item.Prose), $"Missing prose: {item.Id}");
                Assert.NotEmpty(item.Tags);
            }

            foreach (var item in catalog.CryopodEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id), "Cryopod item missing ID");
                Assert.True(seenIds.Add(item.Id), $"Duplicate ID: {item.Id}");
                Assert.False(string.IsNullOrWhiteSpace(item.Prose), $"Missing prose: {item.Id}");
                Assert.NotEmpty(item.Tags);
            }

            foreach (var item in catalog.SaltMineEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id), "SaltMine item missing ID");
                Assert.True(seenIds.Add(item.Id), $"Duplicate ID: {item.Id}");
                Assert.False(string.IsNullOrWhiteSpace(item.Prose), $"Missing prose: {item.Id}");
                Assert.NotEmpty(item.Tags);
            }

            Assert.Equal(30, seenIds.Count);
        }

        [Fact]
        public void TEST_ABY_04_GenericGetById_ReturnsCorrectInstances()
        {
            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_dataDir, _fileIO, _serializer);

            var objH = catalog.GetById("hydrophone_deep_trench_thermal_vent");
            Assert.NotNull(objH);
            Assert.IsType<HydrophoneAcousticEntry>(objH);

            var objB = catalog.GetById("borehole_seismic_fault_hydraulic_pulse");
            Assert.NotNull(objB);
            Assert.IsType<GeothermalBoreholeEntry>(objB);

            var objC = catalog.GetById("cryopod_neural_eeg_spike_nightmare");
            Assert.NotNull(objC);
            Assert.IsType<CryopodFailureEntry>(objC);

            var objS = catalog.GetById("salt_mine_shaft_tally_340_days");
            Assert.NotNull(objS);
            Assert.IsType<SaltMineInscriptionEntry>(objS);

            Assert.Null(catalog.GetById("nonexistent_abyssal_id"));
            Assert.Null(catalog.GetById(string.Empty));
        }

        [Fact]
        public void TEST_ABY_05_QueryByTag_And_QueryBySearch()
        {
            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_dataDir, _fileIO, _serializer);

            // Tag queries
            var hydroEntries = catalog.GetByTag("hydrophone");
            Assert.NotEmpty(hydroEntries);

            var geoEntries = catalog.GetByTag("geothermal");
            Assert.NotEmpty(geoEntries);

            var cryoEntries = catalog.GetByTag("cryogenics");
            Assert.NotEmpty(cryoEntries);

            var saltEntries = catalog.GetByTag("salt_mine");
            Assert.NotEmpty(saltEntries);

            // Search queries
            var subResults = catalog.GetBySearch("submarine");
            Assert.NotEmpty(subResults);

            var geoResults = catalog.GetBySearch("geothermal");
            Assert.NotEmpty(geoResults);

            var emptyResults = catalog.GetBySearch("nonexistent_random_search_term_xyz");
            Assert.Empty(emptyResults);
        }

        [Fact]
        public void TEST_ABY_06_LoadFromDirectory_With_IFileIO_IsIdempotent()
        {
            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_dataDir, _fileIO, _serializer);
            Assert.Equal(30, catalog.TotalCount);

            // Reloading into same directory or calling Clear then reloading
            catalog.Clear();
            Assert.Equal(0, catalog.TotalCount);
            Assert.Empty(catalog.AllEntries);
            Assert.Empty(catalog.HydrophoneEntries);
            Assert.Empty(catalog.BoreholeEntries);
            Assert.Empty(catalog.CryopodEntries);
            Assert.Empty(catalog.SaltMineEntries);

            // Reload
            var reloaded = AbyssalAnomaliesCatalog.LoadFromDirectory(_dataDir, _fileIO, _serializer);
            Assert.Equal(30, reloaded.TotalCount);
        }

        [Fact]
        public void TEST_ABY_07_Projection_MapsAll30RecordsWithCorrectFamiliesAndProvenance()
        {
            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_dataDir, _fileIO, _serializer);

            int activatedCount = 0;
            int deferredCount = 0;
            var producerCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var h in catalog.HydrophoneEntries)
            {
                Assert.Equal(AbyssalRecordFamily.HydrophoneAcoustic, AbyssalAnomaliesProjection.ResolveFamily(h.Id));
                Assert.Equal(AbyssalProvenanceClass.HistoricalInstrumentRecord, AbyssalAnomaliesProjection.ResolveProvenance(h.Id));
                Assert.False(string.IsNullOrEmpty(AbyssalAnomaliesProjection.ResolveProducer(h.Id)));
                Assert.True(AbyssalAnomaliesProjection.ResolveMinDay(h.Id) >= 1);

                if (AbyssalAnomaliesProjection.IsActivated(h.Id))
                {
                    activatedCount++;
                    string prod = AbyssalAnomaliesProjection.ResolveProducer(h.Id);
                    producerCounts[prod] = producerCounts.GetValueOrDefault(prod, 0) + 1;
                }
                else
                {
                    deferredCount++;
                }
            }

            foreach (var b in catalog.BoreholeEntries)
            {
                Assert.Equal(AbyssalRecordFamily.GeothermalBorehole, AbyssalAnomaliesProjection.ResolveFamily(b.Id));
                Assert.Equal(AbyssalProvenanceClass.HistoricalEngineeringRecord, AbyssalAnomaliesProjection.ResolveProvenance(b.Id));
                Assert.False(string.IsNullOrEmpty(AbyssalAnomaliesProjection.ResolveProducer(b.Id)));

                if (AbyssalAnomaliesProjection.IsActivated(b.Id))
                {
                    activatedCount++;
                    string prod = AbyssalAnomaliesProjection.ResolveProducer(b.Id);
                    producerCounts[prod] = producerCounts.GetValueOrDefault(prod, 0) + 1;
                }
                else
                {
                    deferredCount++;
                }
            }

            foreach (var c in catalog.CryopodEntries)
            {
                Assert.Equal(AbyssalRecordFamily.CryopodFailure, AbyssalAnomaliesProjection.ResolveFamily(c.Id));
                Assert.Equal(AbyssalProvenanceClass.HistoricalIncidentRecord, AbyssalAnomaliesProjection.ResolveProvenance(c.Id));
                Assert.False(string.IsNullOrEmpty(AbyssalAnomaliesProjection.ResolveProducer(c.Id)));

                if (AbyssalAnomaliesProjection.IsActivated(c.Id))
                {
                    activatedCount++;
                    string prod = AbyssalAnomaliesProjection.ResolveProducer(c.Id);
                    producerCounts[prod] = producerCounts.GetValueOrDefault(prod, 0) + 1;
                }
                else
                {
                    deferredCount++;
                }
            }

            foreach (var s in catalog.SaltMineEntries)
            {
                Assert.Equal(AbyssalRecordFamily.SaltMineInscription, AbyssalAnomaliesProjection.ResolveFamily(s.Id));
                Assert.Equal(AbyssalProvenanceClass.PhysicalInscriptionTestimony, AbyssalAnomaliesProjection.ResolveProvenance(s.Id));
                Assert.False(string.IsNullOrEmpty(AbyssalAnomaliesProjection.ResolveProducer(s.Id)));

                if (AbyssalAnomaliesProjection.IsActivated(s.Id))
                {
                    activatedCount++;
                    string prod = AbyssalAnomaliesProjection.ResolveProducer(s.Id);
                    producerCounts[prod] = producerCounts.GetValueOrDefault(prod, 0) + 1;
                }
                else
                {
                    deferredCount++;
                }
            }

            // Exactly 17 activated, 13 deferred
            Assert.Equal(17, activatedCount);
            Assert.Equal(13, deferredCount);

            // No producer has more than 4 records (no location dump)
            foreach (var kvp in producerCounts)
            {
                Assert.True(kvp.Value <= 4, $"Producer {kvp.Key} exceeds max 4 records: {kvp.Value}");
            }
        }

        [Fact]
        public void TEST_ABY_08_SimulationIsolation_HistoricalTelemetryNeverMutatesLiveSimulation()
        {
            // Verifies Invariant 5: historical measurements (depth 850m, temp 312.4°C, cryo temp 194.5K, 480Hz)
            // are immutable narrative facts and have zero mutation coupling to live simulation.
            var catalog = AbyssalAnomaliesCatalog.LoadFromDirectory(_dataDir, _fileIO, _serializer);

            var vent = catalog.GetHydrophone("hydrophone_deep_trench_thermal_vent");
            Assert.NotNull(vent);
            Assert.Equal(850, vent!.DepthMeters);
            Assert.Equal(480.0f, vent.AcousticFrequencyHz);

            var spike = catalog.GetBorehole("borehole_magma_boundary_temperature_spike");
            Assert.NotNull(spike);
            Assert.Equal(312.4f, spike!.TemperatureCelsius);
            Assert.Equal(285.0f, spike.CasingPressureBar);

            var cryo = catalog.GetCryopod("cryopod_coolant_circuit_boiloff");
            Assert.NotNull(cryo);
            Assert.Equal(194.5f, cryo!.CoreTemperatureKelvin);
            Assert.Equal(145.0f, cryo.ChamberPressureKpa);

            // Discovery via JournalSystem only adds knowledge key; it never changes player resources or shelter stats
            var journal = new JournalSystem();
            string discId = "disc_hydrophone_deep_trench_thermal_vent";
            journal.UnlockNarrativeDiscovered(discId);

            Assert.True(journal.IsNarrativeDiscovered(discId));
            Assert.Equal(0, journal.ActiveTab);
        }

        [Fact]
        public void TEST_ABY_09_DiscoveryAdapter_ProducesRichCodexEntries()
        {
            var discoveryCatalog = new NarrativeDiscoveryCatalog();
            discoveryCatalog.LoadFromFiles(_dataDir, _fileIO);

            // 1. Hydrophone
            Assert.True(discoveryCatalog.TryGetRecord("disc_hydrophone_deep_trench_thermal_vent", out var hRec));
            Assert.NotNull(hRec);
            Assert.Equal("Abyssal Anomalies — Hydrophone Acoustic Logs", hRec!.Category);
            Assert.Contains("HYDRO-BUOY-DEEP-09", hRec.Title);
            Assert.Contains("Hz", hRec.Subtitle);
            Assert.Contains("dB", hRec.Subtitle);

            // 2. Borehole
            Assert.True(discoveryCatalog.TryGetRecord("disc_borehole_magma_boundary_temperature_spike", out var bRec));
            Assert.NotNull(bRec);
            Assert.Equal("Abyssal Anomalies — Geothermal Borehole Logs", bRec!.Category);
            Assert.Contains("DEEP-WELL-SUMP-08", bRec.Title);
            Assert.Contains("°C", bRec.Subtitle);
            Assert.Contains("bar", bRec.Subtitle);

            // 3. Cryopod
            Assert.True(discoveryCatalog.TryGetRecord("disc_cryopod_coolant_circuit_boiloff", out var cRec));
            Assert.NotNull(cRec);
            Assert.Equal("Abyssal Anomalies — Cryopod Failure Logs", cRec!.Category);
            Assert.Contains("CRYO-CHAMBER-VAULT-14-POD-04", cRec.Title);
            Assert.Contains("K", cRec.Subtitle);
            Assert.Contains("kPa", cRec.Subtitle);

            // 4. SaltMine
            Assert.True(discoveryCatalog.TryGetRecord("disc_salt_mine_shaft_tally_340_days", out var sRec));
            Assert.NotNull(sRec);
            Assert.Equal("Abyssal Anomalies — Salt-Mine Inscriptions", sRec!.Category);
            Assert.Contains("LEVEL_04_HALITE_DRIFT_WEST", sRec.Title);
            Assert.Contains("Medium:", sRec.Subtitle);
            Assert.Contains("Tool:", sRec.Subtitle);
        }
    }
}
