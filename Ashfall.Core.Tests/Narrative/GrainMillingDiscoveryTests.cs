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
    /// PLAN 157 — Grain Milling, Storage & Food-Processing Knowledge Runtime Integration Tests.
    /// Pins:
    /// - 30-record baseline across 4 families (8 millstones, 8 bolting silk, 7 silo weevil, 7 dampener tempering)
    /// - Global ID uniqueness across all 4 families
    /// - Provenance matrix coverage (all 30 records mapped, exactly 20 activated, 10 deferred)
    /// - Producer routing and diversity
    /// - Discovery idempotence and single event firing
    /// - MinDay temporal progression gating
    /// - Strict epistemic privacy firewall for related records (no undiscovered spoilers)
    /// - Save/load round-trip and unknown future ID preservation
    /// - Authority firewall negative fixtures (zero food buffs, zero hunger shifts, zero inventory changes, zero spoilage mutations)
    /// - System API reflection purity check
    /// </summary>
    public sealed class GrainMillingDiscoveryTests : CatalogTestBase
    {
        private static string NarrativeDir => Path.Combine(DataDirectory, "narrative");

        private static GrainMillingCatalog LoadCatalog()
            => GrainMillingCatalog.LoadFromDirectory(DataDirectory);

        private static GrainMillingDiscoverySystem CreateSystem(GrainMillingCatalog? catalog = null)
        {
            catalog ??= LoadCatalog();
            return new GrainMillingDiscoverySystem(catalog);
        }

        // ── Baseline Catalog Load Tests ───────────────────────────────────

        [Fact]
        public void Catalog_LoadsAll30Records_FamilyCountsPreserved()
        {
            var catalog = LoadCatalog();
            Assert.Equal(8, catalog.MillstoneEntries.Count);
            Assert.Equal(8, catalog.SilkEntries.Count);
            Assert.Equal(7, catalog.SiloEntries.Count);
            Assert.Equal(7, catalog.TemperEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void RecordIds_GloballyUnique_AcrossAllFourFamilies()
        {
            var catalog = LoadCatalog();
            var all = catalog.MillstoneEntries.Select(e => e.Id)
                .Concat(catalog.SilkEntries.Select(e => e.Id))
                .Concat(catalog.SiloEntries.Select(e => e.Id))
                .Concat(catalog.TemperEntries.Select(e => e.Id))
                .ToList();

            Assert.Equal(30, all.Count);
            Assert.Equal(30, all.Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void AllRecords_HaveRequiredFields_AndValidRanges()
        {
            var catalog = LoadCatalog();

            // 1. Millstone Dressing Logs
            foreach (var m in catalog.MillstoneEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(m.Id));
                Assert.False(string.IsNullOrWhiteSpace(m.MillstonePairId));
                Assert.False(string.IsNullOrWhiteSpace(m.StoneMaterialType));
                Assert.True(m.CracksPerInchCount > 0f, $"CracksPerInchCount must be > 0 for {m.Id}");
                Assert.True(m.RunnerRotationalRpm > 0f, $"RunnerRotationalRpm must be > 0 for {m.Id}");
                Assert.False(string.IsNullOrWhiteSpace(m.TimestampRelative));
                Assert.NotEmpty(m.Tags);
                Assert.False(string.IsNullOrWhiteSpace(m.Prose));
            }

            // 2. Bolting Silk Reports
            foreach (var s in catalog.SilkEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(s.Id));
                Assert.False(string.IsNullOrWhiteSpace(s.SifterReelId));
                Assert.False(string.IsNullOrWhiteSpace(s.SilkGauzeGrade));
                Assert.True(s.MeshApertureMicrons > 0f, $"MeshApertureMicrons must be > 0 for {s.Id}");
                Assert.True(s.FlourExtractionYieldPct >= 0f && s.FlourExtractionYieldPct <= 100f,
                    $"FlourExtractionYieldPct must be between 0 and 100 for {s.Id}");
                Assert.False(string.IsNullOrWhiteSpace(s.TimestampRelative));
                Assert.NotEmpty(s.Tags);
                Assert.False(string.IsNullOrWhiteSpace(s.Prose));
            }

            // 3. Silo Weevil Audits
            foreach (var w in catalog.SiloEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(w.Id));
                Assert.False(string.IsNullOrWhiteSpace(w.GrainSiloBinId));
                Assert.False(string.IsNullOrWhiteSpace(w.GrainCropSpecies));
                Assert.True(w.GrainMoistureContentPct > 0f, $"GrainMoistureContentPct must be > 0 for {w.Id}");
                Assert.False(string.IsNullOrWhiteSpace(w.TimestampRelative));
                Assert.NotEmpty(w.Tags);
                Assert.False(string.IsNullOrWhiteSpace(w.Prose));
            }

            // 4. Dampener Tempering Assays
            foreach (var t in catalog.TemperEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(t.Id));
                Assert.False(string.IsNullOrWhiteSpace(t.ConditioningBinId));
                Assert.True(t.TemperingWaterAdditionPct >= 0f, $"TemperingWaterAdditionPct must be >= 0 for {t.Id}");
                Assert.True(t.TargetMillingMoisturePct > 0f, $"TargetMillingMoisturePct must be > 0 for {t.Id}");
                Assert.True(t.ConditioningDwellHours >= 0f, $"ConditioningDwellHours must be >= 0 for {t.Id}");
                Assert.False(string.IsNullOrWhiteSpace(t.TimestampRelative));
                Assert.NotEmpty(t.Tags);
                Assert.False(string.IsNullOrWhiteSpace(t.Prose));
            }
        }

        [Fact]
        public void RawJsonFiles_ContainSchemaVersion()
        {
            var files = new[]
            {
                "burr_millstone_dressing_logs.json",
                "bolting_silk_mesh_reports.json",
                "grain_silo_weevil_audits.json",
                "mill_dampener_tempering_assays.json"
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
            var metadataList = GrainMillingProjection.GetAllMetadata().ToList();
            Assert.Equal(30, metadataList.Count);

            var allCatalogIds = catalog.MillstoneEntries.Select(e => e.Id)
                .Concat(catalog.SilkEntries.Select(e => e.Id))
                .Concat(catalog.SiloEntries.Select(e => e.Id))
                .Concat(catalog.TemperEntries.Select(e => e.Id))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var id in allCatalogIds)
            {
                var meta = GrainMillingProjection.GetMetadata(id);
                Assert.NotNull(meta);
                Assert.Equal(id, meta!.RecordId, StringComparer.OrdinalIgnoreCase);
                Assert.False(string.IsNullOrWhiteSpace(meta.NamedSubject));
                Assert.False(string.IsNullOrWhiteSpace(meta.Channel));
                Assert.True(meta.MinDay >= 1, $"MinDay must be >= 1 for {id}");
                Assert.False(string.IsNullOrWhiteSpace(meta.MeasurementSummary));
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
            var metadataList = GrainMillingProjection.GetAllMetadata().ToList();
            int activatedCount = metadataList.Count(m => m.IsActivated);
            int deferredCount = metadataList.Count(m => !m.IsActivated);

            Assert.Equal(20, activatedCount);
            Assert.Equal(10, deferredCount);

            // Distribution across families
            int activatedMill = metadataList.Count(m => m.IsActivated && m.Family == GrainMillingRecordFamily.MillstoneDressing);
            int activatedSilk = metadataList.Count(m => m.IsActivated && m.Family == GrainMillingRecordFamily.BoltingSilk);
            int activatedSilo = metadataList.Count(m => m.IsActivated && m.Family == GrainMillingRecordFamily.SiloWeevil);
            int activatedTemper = metadataList.Count(m => m.IsActivated && m.Family == GrainMillingRecordFamily.DampenerTempering);

            Assert.Equal(6, activatedMill);
            Assert.Equal(6, activatedSilk);
            Assert.Equal(4, activatedSilo);
            Assert.Equal(4, activatedTemper);
        }

        // ── Discovery Idempotence & MinDay Gating ─────────────────────────

        [Fact]
        public void Discovery_Idempotent_And_EventsFiredOnce()
        {
            var system = CreateSystem();
            string testRecord = "burr_millstone_french_chert_chisel_cracking";
            int eventsFired = 0;
            int stateChangesFired = 0;
            GrainMillingRecordFamily firedFamily = GrainMillingRecordFamily.BoltingSilk;

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
            Assert.Equal(GrainMillingRecordFamily.MillstoneDressing, firedFamily);
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
            string testRecord = "burr_millstone_bridge_tree_tentering_screw_chatter"; // MinDay = 15
            var meta = GrainMillingProjection.GetMetadata(testRecord);
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
            string producer = "room_workshop";

            // room_workshop hosts burr_millstone_french_chert_chisel_cracking (MinDay 1)
            // and burr_millstone_bridge_tree_tentering_screw_chatter (MinDay 15)
            var discoveredDay1 = system.DiscoverAtProducer(producer, currentDay: 1);
            Assert.Contains("burr_millstone_french_chert_chisel_cracking", discoveredDay1);

            // Revisit same day produces empty list
            var revisit = system.DiscoverAtProducer(producer, currentDay: 1);
            Assert.Empty(revisit);

            // Revisit at Day 15 discovers the second record
            var discoveredDay15 = system.DiscoverAtProducer(producer, currentDay: 15);
            Assert.Contains("burr_millstone_bridge_tree_tentering_screw_chatter", discoveredDay15);

            // Both are now discovered
            Assert.True(system.IsDiscovered("burr_millstone_french_chert_chisel_cracking"));
            Assert.True(system.IsDiscovered("burr_millstone_bridge_tree_tentering_screw_chatter"));
        }

        // ── Deterministic Relations & Epistemic Privacy Firewall ──────────

        [Fact]
        public void RelatedRecords_StrictPrivacyFirewall_NeverSpoilsUndiscoveredRecords()
        {
            var system = CreateSystem();
            string recA = "burr_millstone_french_chert_chisel_cracking"; // MinDay 1
            string recB = "bolting_silk_gauze_number_mesh_selection";      // MinDay 1

            // 1. Neither discovered
            Assert.Empty(system.GetRelated(recA));
            Assert.Empty(system.GetRelated(recB));

            // 2. Discover recA only
            system.TryDiscoverRecord(recA, currentDay: 1);
            Assert.True(system.IsDiscovered(recA));
            Assert.False(system.IsDiscovered(recB));

            // Privacy firewall: GetRelated(recA) must NOT leak recB because recB is undiscovered!
            var relatedA1 = system.GetRelated(recA);
            Assert.Empty(relatedA1);

            // 3. Discover recB
            system.TryDiscoverRecord(recB, currentDay: 2);
            Assert.True(system.IsDiscovered(recB));

            // Now both are discovered, relation is revealed
            var relatedA2 = system.GetRelated(recA);
            Assert.Contains(relatedA2, r => r.RecordId == recB && r.RelationKind == "process_dependency");

            var relatedB = system.GetRelated(recB);
            Assert.Contains(relatedB, r => r.RecordId == recA && r.RelationKind == "process_dependency");
        }

        // ── Save / Load Round-Trip & Unknown ID Toleration ────────────────

        [Fact]
        public void SaveRoundTrip_PreservesDiscoveredRecords_AndOrdinalOrdering()
        {
            var system = CreateSystem();
            system.TryDiscoverRecord("burr_millstone_french_chert_chisel_cracking", currentDay: 30);
            system.TryDiscoverRecord("bolting_silk_gauze_number_mesh_selection", currentDay: 30);
            system.TryDiscoverRecord("grain_silo_granary_weevil_larva_hollow_berry", currentDay: 30);

            var captured = system.CaptureState();
            Assert.Equal(3, captured.discoveredRecordIds.Count);

            // Verify ordinal sorting
            var expectedSorted = captured.discoveredRecordIds.OrderBy(x => x, StringComparer.Ordinal).ToList();
            Assert.Equal(expectedSorted, captured.discoveredRecordIds);

            // Restore into fresh system
            var freshSystem = CreateSystem();
            freshSystem.RestoreState(captured);

            Assert.Equal(3, freshSystem.DiscoveredRecordIds.Count);
            Assert.True(freshSystem.IsDiscovered("burr_millstone_french_chert_chisel_cracking"));
            Assert.True(freshSystem.IsDiscovered("bolting_silk_gauze_number_mesh_selection"));
            Assert.True(freshSystem.IsDiscovered("grain_silo_granary_weevil_larva_hollow_berry"));
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
            var state = new GrainMillingArchiveState
            {
                discoveredRecordIds = new List<string>
                {
                    "burr_millstone_french_chert_chisel_cracking",
                    "future_expansion_grain_silo_batch_99",
                    "mill_dressing_future_runner_alpha"
                }
            };

            system.RestoreState(state);

            // Unknown IDs are preserved in state and do not throw
            Assert.Equal(3, system.DiscoveredRecordIds.Count);
            Assert.True(system.IsDiscovered("future_expansion_grain_silo_batch_99"));

            var reCaptured = system.CaptureState();
            Assert.Contains("future_expansion_grain_silo_batch_99", reCaptured.discoveredRecordIds);
        }

        // ── Authority Firewall Negative Fixtures ──────────────────────────

        [Fact]
        public void AuthorityFirewall_HighFlourYields_DoNotMutateInventoryOrFlourQuantities()
        {
            var catalog = LoadCatalog();
            var silk = catalog.GetSilk("bolting_silk_gauze_number_mesh_selection");
            Assert.NotNull(silk);
            Assert.Equal(72.5f, silk!.FlourExtractionYieldPct);

            var system = CreateSystem(catalog);
            bool discovered = system.TryDiscoverRecord(silk.Id, currentDay: 5);
            Assert.True(discovered);

            var systemType = typeof(GrainMillingDiscoverySystem);
            Assert.Null(systemType.GetMethod("AddFlour"));
            Assert.Null(systemType.GetMethod("GrantItem"));
            Assert.Null(systemType.GetMethod("ProduceFlour"));
            Assert.Null(systemType.GetMethod("SetFlourYieldMultiplier"));
        }

        [Fact]
        public void AuthorityFirewall_SiloWeevilInfestation_DoesNotMutateSpoilageOrCropStores()
        {
            var catalog = LoadCatalog();
            var weevil = catalog.GetSilo("grain_silo_granary_weevil_larva_hollow_berry");
            Assert.NotNull(weevil);
            Assert.Equal(13.8f, weevil!.GrainMoistureContentPct);

            var system = CreateSystem(catalog);
            system.TryDiscoverRecord(weevil.Id, currentDay: 10);

            var systemType = typeof(GrainMillingDiscoverySystem);
            Assert.Null(systemType.GetMethod("InflictPest"));
            Assert.Null(systemType.GetMethod("SpoilGrain"));
            Assert.Null(systemType.GetMethod("DestroyCrop"));
            Assert.Null(systemType.GetMethod("SetMoisture"));
        }

        [Fact]
        public void AuthorityFirewall_HighSpeedMillstones_DoNotMutatePowerGridOrFrictionHeat()
        {
            var catalog = LoadCatalog();
            var mill = catalog.GetMillstone("burr_millstone_french_chert_chisel_cracking");
            Assert.NotNull(mill);
            Assert.Equal(115.0f, mill!.RunnerRotationalRpm);

            var system = CreateSystem(catalog);
            system.TryDiscoverRecord(mill.Id, currentDay: 5);

            var systemType = typeof(GrainMillingDiscoverySystem);
            Assert.Null(systemType.GetMethod("SetPowerGridLoad"));
            Assert.Null(systemType.GetMethod("ConsumePower"));
            Assert.Null(systemType.GetMethod("SetFrictionHeat"));
            Assert.Null(systemType.GetMethod("DamageMill"));
        }

        [Fact]
        public void AuthorityFirewall_PublicApiSurface_StrictlyBounded()
        {
            var type = typeof(GrainMillingDiscoverySystem);
            var forbiddenMarkers = new[]
            {
                "AddFood", "RemoveFood", "Flour", "Grain", "Moisture", "Weevil", "Pest",
                "Craft", "AddItem", "GrantItem", "SpawnCrop", "Nutrition",
                "Spoil", "Temperature", "Price", "Barter",
                "Yield", "Damage", "Power", "Grid", "Unlock", "Grant"
            };

            foreach (var method in type.GetMethods())
            {
                foreach (var marker in forbiddenMarkers)
                {
                    Assert.False(method.Name.Contains(marker, StringComparison.OrdinalIgnoreCase),
                        $"GrainMillingDiscoverySystem method '{method.Name}' violates authority firewall with marker '{marker}'");
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
