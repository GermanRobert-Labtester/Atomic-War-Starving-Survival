// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// PLAN 154 — Hydrogeology: Water, Caves & Geothermal Science Runtime Integration Tests.
    /// Pins:
    /// - 30-record baseline across 4 families (8 wells, 8 biota, 7 steam, 7 minerals)
    /// - Global ID uniqueness across all 4 families
    /// - Provenance matrix coverage (all 30 records mapped, exactly 20 activated, 10 deferred)
    /// - Producer diversity (<= 3 activated records per producer)
    /// - Discovery idempotence and single event firing
    /// - MinDay temporal progression gating
    /// - Strict privacy firewall for related records (no undiscovered spoilers)
    /// - Save/load round-trip and unknown future ID preservation
    /// - Authority firewall negative fixtures (no radiation dosing, no ore spawning, no water mutation, no power grid changes)
    /// - System API reflection purity check
    /// </summary>
    public sealed class HydroGeologyDiscoveryTests : CatalogTestBase
    {
        private static string NarrativeDir => Path.Combine(DataDirectory, "narrative");

        private static HydroGeologyCatalog LoadCatalog()
            => HydroGeologyCatalog.LoadFromDirectory(DataDirectory);

        private static HydroGeologyDiscoverySystem CreateSystem(HydroGeologyCatalog? catalog = null)
        {
            catalog ??= LoadCatalog();
            return new HydroGeologyDiscoverySystem(catalog);
        }

        // ── Baseline Catalog Load Tests ───────────────────────────────────

        [Fact]
        public void Catalog_LoadsAll30Records_FamilyCountsPreserved()
        {
            var catalog = LoadCatalog();
            Assert.Equal(8, catalog.WellEntries.Count);
            Assert.Equal(8, catalog.BiotaEntries.Count);
            Assert.Equal(7, catalog.SteamEntries.Count);
            Assert.Equal(7, catalog.MineralEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void RecordIds_GloballyUnique_AcrossAllFourFamilies()
        {
            var catalog = LoadCatalog();
            var all = catalog.WellEntries.Select(e => e.Id)
                .Concat(catalog.BiotaEntries.Select(e => e.Id))
                .Concat(catalog.SteamEntries.Select(e => e.Id))
                .Concat(catalog.MineralEntries.Select(e => e.Id))
                .ToList();

            Assert.Equal(30, all.Count);
            Assert.Equal(30, all.Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void AllRecords_HaveRequiredFields_AndValidRanges()
        {
            var catalog = LoadCatalog();

            // 1. Artesian Wells
            foreach (var w in catalog.WellEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(w.Id));
                Assert.False(string.IsNullOrWhiteSpace(w.WellIdentifier));
                Assert.False(string.IsNullOrWhiteSpace(w.AquiferStratum));
                Assert.False(string.IsNullOrWhiteSpace(w.ContaminantAgent));
                Assert.True(w.ActivityLevelBqL >= 0f, $"ActivityLevelBqL must be >= 0 for {w.Id}");
                Assert.False(string.IsNullOrWhiteSpace(w.TimestampRelative));
                Assert.NotEmpty(w.Tags);
                Assert.False(string.IsNullOrWhiteSpace(w.Prose));
            }

            // 2. Cave Biota
            foreach (var b in catalog.BiotaEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(b.Id));
                Assert.False(string.IsNullOrWhiteSpace(b.SpeciesDesignation));
                Assert.False(string.IsNullOrWhiteSpace(b.CavernLocation));
                Assert.False(string.IsNullOrWhiteSpace(b.BioluminescenceType));
                Assert.False(string.IsNullOrWhiteSpace(b.EcologicalNiche));
                Assert.False(string.IsNullOrWhiteSpace(b.TimestampRelative));
                Assert.NotEmpty(b.Tags);
                Assert.False(string.IsNullOrWhiteSpace(b.Prose));
            }

            // 3. Steam Vents
            foreach (var s in catalog.SteamEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(s.Id));
                Assert.False(string.IsNullOrWhiteSpace(s.VentManifoldId));
                Assert.True(s.SteamTemperatureCelsius > 0f, $"Steam temperature must be > 0 for {s.Id}");
                Assert.True(s.LinePressureBar >= 0f, $"Line pressure must be >= 0 for {s.Id}");
                Assert.False(string.IsNullOrWhiteSpace(s.FailureDiagnostic));
                Assert.False(string.IsNullOrWhiteSpace(s.TimestampRelative));
                Assert.NotEmpty(s.Tags);
                Assert.False(string.IsNullOrWhiteSpace(s.Prose));
            }

            // 4. Stalactite Mineral Assays
            foreach (var m in catalog.MineralEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(m.Id));
                Assert.False(string.IsNullOrWhiteSpace(m.SampleSpecimenId));
                Assert.False(string.IsNullOrWhiteSpace(m.MineralSpecies));
                Assert.True(m.PrimaryMetalAssayPct >= 0f && m.PrimaryMetalAssayPct <= 100f,
                    $"Assay percentage must be 0-100% for {m.Id}");
                Assert.True(m.RadiationEmissionUrHr >= 0f, $"Radiation emission must be >= 0 for {m.Id}");
                Assert.False(string.IsNullOrWhiteSpace(m.TimestampRelative));
                Assert.NotEmpty(m.Tags);
                Assert.False(string.IsNullOrWhiteSpace(m.Prose));
            }
        }

        [Fact]
        public void RawJsonFiles_ContainSchemaVersion()
        {
            var files = new[]
            {
                "artesian_well_contamination_logs.json",
                "cave_aquatic_biota_logs.json",
                "geothermal_steam_vent_diagnostics.json",
                "stalactite_mineral_assay_reports.json"
            };

            foreach (var f in files)
            {
                string path = Path.Combine(NarrativeDir, f);
                Assert.True(File.Exists(path), $"File does not exist: {path}");
                string content = File.ReadAllText(path);
                using var doc = JsonDocument.Parse(content);
                Assert.True(doc.RootElement.TryGetProperty("schema_version", out var prop),
                    $"{f} missing schema_version property");
                Assert.True(prop.GetInt32() >= 1, $"{f} schema_version must be >= 1");
            }
        }

        // ── Provenance Matrix & Activation Distribution ───────────────────

        [Fact]
        public void ProvenanceMatrix_All30Records_HaveValidMetadata()
        {
            var catalog = LoadCatalog();
            var metadataList = HydroGeologyProjection.GetAllMetadata();
            Assert.Equal(30, metadataList.Count);

            var allCatalogIds = catalog.WellEntries.Select(e => e.Id)
                .Concat(catalog.BiotaEntries.Select(e => e.Id))
                .Concat(catalog.SteamEntries.Select(e => e.Id))
                .Concat(catalog.MineralEntries.Select(e => e.Id))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var id in allCatalogIds)
            {
                var meta = HydroGeologyProjection.ResolveMetadata(id);
                Assert.NotNull(meta);
                Assert.Equal(id, meta!.RecordId, StringComparer.OrdinalIgnoreCase);
                Assert.False(string.IsNullOrWhiteSpace(meta.NamedSubject));
                Assert.False(string.IsNullOrWhiteSpace(meta.Channel));
                Assert.True(meta.MinDay >= 1, $"MinDay must be >= 1 for {id}");
                Assert.False(string.IsNullOrWhiteSpace(meta.MeasurementProvenance));
                if (meta.IsActivated)
                {
                    Assert.False(string.IsNullOrWhiteSpace(meta.ProducerId),
                        $"Activated record {id} must have a non-empty ProducerId");
                }
            }
        }

        [Fact]
        public void Activation_Matches20Activated_10Deferred_Distribution()
        {
            var metadataList = HydroGeologyProjection.GetAllMetadata();
            int activatedCount = metadataList.Count(m => m.IsActivated);
            int deferredCount = metadataList.Count(m => !m.IsActivated);

            Assert.Equal(20, activatedCount);
            Assert.Equal(10, deferredCount);

            // Distribution across families (each must have >= 4 activated)
            int activatedWells = metadataList.Count(m => m.IsActivated && m.Family == HydroGeologyRecordFamily.ArtesianWell);
            int activatedBiota = metadataList.Count(m => m.IsActivated && m.Family == HydroGeologyRecordFamily.CaveBiota);
            int activatedSteam = metadataList.Count(m => m.IsActivated && m.Family == HydroGeologyRecordFamily.GeothermalSteam);
            int activatedMineral = metadataList.Count(m => m.IsActivated && m.Family == HydroGeologyRecordFamily.StalactiteMineral);

            Assert.True(activatedWells >= 4, $"Artesian wells activated {activatedWells} < 4");
            Assert.True(activatedBiota >= 4, $"Cave biota activated {activatedBiota} < 4");
            Assert.True(activatedSteam >= 4, $"Geothermal steam activated {activatedSteam} < 4");
            Assert.True(activatedMineral >= 4, $"Stalactite mineral activated {activatedMineral} < 4");

            // Producer diversity: no single producer has > 3 activated records
            var producerCounts = metadataList
                .Where(m => m.IsActivated)
                .GroupBy(m => m.ProducerId, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var kvp in producerCounts)
            {
                Assert.True(kvp.Value <= 3,
                    $"Producer {kvp.Key} has {kvp.Value} records, exceeding max diversity cap of 3");
            }
        }

        // ── Discovery Idempotence & MinDay Gating ─────────────────────────

        [Fact]
        public void Discovery_Idempotent_And_EventsFiredOnce()
        {
            var system = CreateSystem();
            string testRecord = "well_contam_tritium_percolation_spike";
            int eventsFired = 0;
            int stateChangesFired = 0;
            HydroGeologyRecordFamily firedFamily = HydroGeologyRecordFamily.CaveBiota;

            system.OnRecordFirstDiscovered += (id, family) =>
            {
                eventsFired++;
                firedFamily = family;
            };
            system.OnStateChanged += () => stateChangesFired++;

            // Initial state
            Assert.False(system.IsDiscovered(testRecord));
            Assert.DoesNotContain(testRecord, system.DiscoveredRecordIds);

            // First discovery (at day 1 where MinDay = 1)
            bool firstResult = system.TryDiscoverRecord(testRecord, currentDay: 1);
            Assert.True(firstResult);
            Assert.Equal(1, eventsFired);
            Assert.Equal(1, stateChangesFired);
            Assert.Equal(HydroGeologyRecordFamily.ArtesianWell, firedFamily);
            Assert.True(system.IsDiscovered(testRecord));
            Assert.Contains(testRecord, system.DiscoveredRecordIds);

            // Second discovery attempt (idempotent no-op)
            bool secondResult = system.TryDiscoverRecord(testRecord, currentDay: 1);
            Assert.False(secondResult);
            Assert.Equal(1, eventsFired); // Did not fire again
            Assert.Equal(1, stateChangesFired);
            Assert.True(system.IsDiscovered(testRecord));
        }

        [Fact]
        public void MinDay_Gating_PreventsPrematureDiscovery()
        {
            var system = CreateSystem();
            string testRecord = "steam_vent_superheated_nozzle_erosion"; // MinDay = 15
            var meta = HydroGeologyProjection.ResolveMetadata(testRecord);
            Assert.NotNull(meta);
            Assert.Equal(15, meta!.MinDay);

            // Attempt at day 1 (too early)
            Assert.False(system.TryDiscoverRecord(testRecord, currentDay: 1));
            Assert.False(system.IsDiscovered(testRecord));

            // Attempt at day 14 (still too early)
            Assert.False(system.TryDiscoverRecord(testRecord, currentDay: 14));
            Assert.False(system.IsDiscovered(testRecord));

            // Attempt at day 15 (eligible)
            Assert.True(system.TryDiscoverRecord(testRecord, currentDay: 15));
            Assert.True(system.IsDiscovered(testRecord));
        }

        [Fact]
        public void DiscoverAtProducer_DiscoversMatchingRecords_AndRevisitsAreSilent()
        {
            var system = CreateSystem();
            string producer = "location_drainage_network";

            // Drainage network hosts biota_cave_bioluminescent_blue_amphipod (MinDay 8)
            // and stalactite_assay_galena_lead_soda_straw (MinDay 14)
            var discoveredDay8 = system.DiscoverAtProducer(producer, currentDay: 8);
            Assert.Single(discoveredDay8);
            Assert.Contains("biota_cave_bioluminescent_blue_amphipod", discoveredDay8);

            // Revisit same day produces empty list
            var revisit = system.DiscoverAtProducer(producer, currentDay: 8);
            Assert.Empty(revisit);

            // Revisit at Day 14 discovers the second record
            var discoveredDay14 = system.DiscoverAtProducer(producer, currentDay: 14);
            Assert.Single(discoveredDay14);
            Assert.Contains("stalactite_assay_galena_lead_soda_straw", discoveredDay14);

            // Both are now discovered
            Assert.True(system.IsDiscovered("biota_cave_bioluminescent_blue_amphipod"));
            Assert.True(system.IsDiscovered("stalactite_assay_galena_lead_soda_straw"));
        }

        // ── Deterministic Relations & Privacy Firewall ────────────────────

        [Fact]
        public void RelatedRecords_StrictPrivacyFirewall_NeverSpoilsUndiscoveredRecords()
        {
            var system = CreateSystem();
            string recA = "biota_cave_bioluminescent_blue_amphipod";   // drainage_network, MinDay 8
            string recB = "stalactite_assay_galena_lead_soda_straw"; // drainage_network, MinDay 14

            // 1. Neither discovered
            Assert.Empty(system.GetRelated(recA));
            Assert.Empty(system.GetRelated(recB));

            // 2. Discover recA only
            system.TryDiscoverRecord(recA, currentDay: 10);
            Assert.True(system.IsDiscovered(recA));
            Assert.False(system.IsDiscovered(recB));

            // Privacy firewall: GetRelated(recA) must NOT leak recB because recB is undiscovered!
            var relatedA1 = system.GetRelated(recA);
            Assert.Empty(relatedA1);

            // 3. Discover recB
            system.TryDiscoverRecord(recB, currentDay: 15);
            Assert.True(system.IsDiscovered(recB));

            // Now both are discovered, relation is revealed
            var relatedA2 = system.GetRelated(recA);
            Assert.Single(relatedA2);
            Assert.Equal(recB, relatedA2[0].RecordId);
            Assert.Equal("shared_producer_site", relatedA2[0].RelationKind);

            var relatedB = system.GetRelated(recB);
            Assert.Single(relatedB);
            Assert.Equal(recA, relatedB[0].RecordId);
            Assert.Equal("shared_producer_site", relatedB[0].RelationKind);
        }

        [Fact]
        public void RelatedRecords_RadionuclideLinkage_ConnectsIsotopeObservations()
        {
            var system = CreateSystem();
            // Well with Radionuclide (Strontium-90)
            string wellRecord = "well_contam_strontium_90_limestone_leach";
            // Mineral assay with Uranium
            string mineralRecord = "stalactite_assay_uranophane_canary_crust";

            system.TryDiscoverRecord(wellRecord, currentDay: 20);
            system.TryDiscoverRecord(mineralRecord, currentDay: 20);

            var related = system.GetRelated(wellRecord);
            Assert.Contains(related, r => r.RecordId == mineralRecord && r.RelationKind == "radionuclide_migration");
        }

        // ── Save / Load Round-Trip & Unknown ID Toleration ────────────────

        [Fact]
        public void SaveRoundTrip_PreservesDiscoveredRecords_AndOrdinalOrdering()
        {
            var system = CreateSystem();
            system.TryDiscoverRecord("steam_vent_superheated_nozzle_erosion", currentDay: 30);
            system.TryDiscoverRecord("biota_cave_eyeless_albino_trout", currentDay: 30);
            system.TryDiscoverRecord("well_contam_tritium_percolation_spike", currentDay: 30);

            var captured = system.CaptureState();
            Assert.Equal(3, captured.discoveredRecordIds.Count);

            // Verify ordinal sorting
            var expectedSorted = captured.discoveredRecordIds.OrderBy(x => x, StringComparer.Ordinal).ToList();
            Assert.Equal(expectedSorted, captured.discoveredRecordIds);

            // Restore into fresh system
            var freshSystem = CreateSystem();
            freshSystem.RestoreState(captured);

            Assert.Equal(3, freshSystem.DiscoveredRecordIds.Count);
            Assert.True(freshSystem.IsDiscovered("steam_vent_superheated_nozzle_erosion"));
            Assert.True(freshSystem.IsDiscovered("biota_cave_eyeless_albino_trout"));
            Assert.True(freshSystem.IsDiscovered("well_contam_tritium_percolation_spike"));
        }

        [Fact]
        public void RestoreState_Null_SafelyLeavesEmptyArchive()
        {
            var system = CreateSystem();
            system.RestoreState(null);
            Assert.Empty(system.DiscoveredRecordIds);
            Assert.NotNull(system.State);
        }

        [Fact]
        public void RestoreState_PreservesUnknownFutureIdsInertly()
        {
            var system = CreateSystem();
            var state = new HydroGeologyArchiveState
            {
                discoveredRecordIds = new List<string>
                {
                    "biota_cave_eyeless_albino_trout",
                    "future_expansion_stalactite_assay_99",
                    "well_contam_future_isotope_alpha"
                }
            };

            system.RestoreState(state);

            // Unknown IDs are preserved in state and do not throw
            Assert.Equal(3, system.DiscoveredRecordIds.Count);
            Assert.True(system.IsDiscovered("future_expansion_stalactite_assay_99"));

            var reCaptured = system.CaptureState();
            Assert.Contains("future_expansion_stalactite_assay_99", reCaptured.discoveredRecordIds);
        }

        // ── Authority Firewall Negative Fixtures ──────────────────────────

        [Fact]
        public void AuthorityFirewall_HighRadiationAssays_DoNotInflictSurvivorRadiation()
        {
            var catalog = LoadCatalog();
            // Uranophane mineral assay records 8,400.0 uR/hr emission at specimen time
            var uranophane = catalog.GetMineralAssay("stalactite_assay_uranophane_canary_crust");
            Assert.NotNull(uranophane);
            Assert.Equal(8400.0f, uranophane!.RadiationEmissionUrHr);

            // Discovery system simply holds the record ID; it has no survivors or radiation context
            var system = CreateSystem(catalog);
            bool discovered = system.TryDiscoverRecord(uranophane.Id, currentDay: 20);
            Assert.True(discovered);

            // Assert that HydroGeologyDiscoverySystem does not expose any radiation dosing API
            var systemType = typeof(HydroGeologyDiscoverySystem);
            Assert.Null(systemType.GetMethod("InflictRadiation"));
            Assert.Null(systemType.GetMethod("AddDose"));
            Assert.Null(systemType.GetMethod("ApplyIrradiation"));
        }

        [Fact]
        public void AuthorityFirewall_HighMetalAssays_DoNotGrantItemsOrOre()
        {
            var catalog = LoadCatalog();
            // Galena stalactite records 86.6% lead assay
            var galena = catalog.GetMineralAssay("stalactite_assay_galena_lead_soda_straw");
            Assert.NotNull(galena);
            Assert.Equal(86.6f, galena!.PrimaryMetalAssayPct);

            var system = CreateSystem(catalog);
            system.TryDiscoverRecord(galena.Id, currentDay: 15);

            // System has no inventory or item granting capability
            var systemType = typeof(HydroGeologyDiscoverySystem);
            Assert.Null(systemType.GetMethod("GrantItem"));
            Assert.Null(systemType.GetMethod("AddResource"));
            Assert.Null(systemType.GetMethod("SpawnOre"));
        }

        [Fact]
        public void AuthorityFirewall_ContaminatedWells_DoNotMutateShelterWaterPurityOrQuantity()
        {
            var catalog = LoadCatalog();
            // Tritium surge records 4,500 Bq/L
            var tritium = catalog.GetWellContamination("well_contam_tritium_percolation_spike");
            Assert.NotNull(tritium);
            Assert.Equal(4500.0f, tritium!.ActivityLevelBqL);

            var system = CreateSystem(catalog);
            system.TryDiscoverRecord(tritium.Id, currentDay: 10);

            var systemType = typeof(HydroGeologyDiscoverySystem);
            Assert.Null(systemType.GetMethod("SetWaterPurity"));
            Assert.Null(systemType.GetMethod("ConsumeWater"));
            Assert.Null(systemType.GetMethod("AddContaminant"));
        }

        [Fact]
        public void AuthorityFirewall_HighPressureSteamVents_DoNotMutatePowerGridOrBoiler()
        {
            var catalog = LoadCatalog();
            // Superheated dry steam 285C at 42 bar
            var steam = catalog.GetSteamVent("steam_vent_superheated_nozzle_erosion");
            Assert.NotNull(steam);
            Assert.Equal(285.0f, steam!.SteamTemperatureCelsius);
            Assert.Equal(42.0f, steam.LinePressureBar);

            var system = CreateSystem(catalog);
            system.TryDiscoverRecord(steam.Id, currentDay: 20);

            var systemType = typeof(HydroGeologyDiscoverySystem);
            Assert.Null(systemType.GetMethod("SetPowerGridOutput"));
            Assert.Null(systemType.GetMethod("SetBoilerPressure"));
            Assert.Null(systemType.GetMethod("SetTemperature"));
        }

        [Fact]
        public void AuthorityFirewall_PublicApiSurface_StrictlyBounded()
        {
            var type = typeof(HydroGeologyDiscoverySystem);
            var forbiddenMarkers = new[]
            {
                "AddWater", "RemoveWater", "Radiation", "Dose", "Inflict", "Damage",
                "Craft", "AddItem", "GrantItem", "SpawnOre", "Boiler",
                "Pressure", "Temperature", "Countdown", "Schedule",
                "Launch", "Arm", "Spawn", "Unlock", "Grant"
            };

            foreach (var method in type.GetMethods())
            {
                foreach (var marker in forbiddenMarkers)
                {
                    Assert.False(method.Name.Contains(marker, StringComparison.OrdinalIgnoreCase),
                        $"HydroGeologyDiscoverySystem method '{method.Name}' violates authority firewall with marker '{marker}'");
                }
            }

            var declaredPublicMethods = type.GetMethods()
                .Where(m => m.DeclaringType == type && !m.IsSpecialName)
                .Select(m => m.Name)
                .Distinct()
                .OrderBy(n => n, StringComparer.Ordinal)
                .ToList();

            // Explicit allowlist of declared public methods
            var expectedMethods = new[]
            {
                "CaptureState",
                "DeferredRecordIds",
                "DiscoverAtProducer",
                "GetProducer",
                "GetRelated",
                "HasProducer",
                "IsDiscovered",
                "RestoreState",
                "TryDiscoverRecord",
                "TryRegisterProducer"
            };

            Assert.Equal(expectedMethods, declaredPublicMethods);
        }
    }
}
