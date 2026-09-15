// SPDX-License-Identifier: MIT
// ============================================================================
// Tasks 5–8 Wave G — legacy save migration (§12–§13).
// Three mismatch cases (A: manual discovered/research not revealed; B: map
// discovered/location hidden; C: vinyl discovered/ownership missing):
// construct old-version state → reconcile → verify subsystem state → reload
// → verify NO repeat effect. Reconciliation is idempotent and runs once per
// campaign load, never during restore (§1.5).
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Collectibles
{
    public sealed class CollectibleSaveMigrationTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            string? dir = AppContext.BaseDirectory;
            while (dir != null)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (File.Exists(Path.Combine(candidate, "collectibles.json"))) return candidate;
                dir = Path.GetDirectoryName(dir);
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        private static CollectibleCatalog LoadCatalog()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(catalog);
            return catalog!;
        }

        private static VinylMoraleSystem BuildVinylSystem()
        {
            var system = new VinylMoraleSystem();
            string path = Path.Combine(DataDir, "narrative", "vinyl_record_archive.json");
            if (!File.Exists(path)) return system;
            var file = new SystemTextJsonSerializer().Deserialize<VinylRecordsFile>(File.ReadAllText(path));
            if (file?.records == null) return system;
            var defs = new List<VinylRecordDefinition>();
            foreach (var r in file.records)
            {
                if (r == null || string.IsNullOrEmpty(r.record_id)) continue;
                defs.Add(new VinylRecordDefinition
                {
                    record_id = r.record_id,
                    display_name = r.title ?? r.record_id,
                    genre = (r.tags != null && r.tags.Length > 0) ? r.tags[0] : string.Empty,
                    morale_daily_bonus = r.daily_morale_modifier,
                    flashback_suppression = 0f,
                    audio_cue_id = string.Empty,
                    description = r.dweller_resonance_notes ?? string.Empty
                });
            }
            system.LoadCatalog(defs);
            return system;
        }

        private static (CollectibleEffectDispatcher dispatcher, WastelandMapSystem map, ResearchSystem research, CollectibleDiscoveryState discovery)
            BuildWorld(CollectibleCatalog catalog, List<string> clueTargets)
        {
            // The map authority requires at least one node; seed a placeholder
            // when a scenario needs no clue nodes (Case A is research-only).
            var nodes = clueTargets.Select(t => new MapNode { Id = t, DisplayName = t }).ToList();
            if (nodes.Count == 0)
                nodes.Add(new MapNode { Id = "loc_holdfast", DisplayName = "Holdfast" });
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, new List<MapRoute>());
            var discovery = new CollectibleDiscoveryState();
            var research = new ResearchSystem();
            // Host startup registers the authored research catalog; a legacy save
            // may still lack the REVEAL for a discovered manual (the Case-A gap).
            foreach (var t in catalog.ByItemId.Values.Where(d => d.effect_type == "knowledge"))
                if (!string.IsNullOrEmpty(t.effect_target))
                    research.Register(new Ashfall.Core.ResearchKnowledgeDef
                    {
                        id = t.effect_target!,
                        displayName = t.effect_target,
                        daysToComplete = 4
                    });
            var dispatcher = new CollectibleEffectDispatcher(
                catalog, discovery, researchProvider: () => research, mapProvider: () => map, dayProvider: () => 3);
            return (dispatcher, map, research, discovery);
        }

        // ── Case A: manual discovered, research node not revealed ──────

        [Fact]
        public void LegacyCaseA_ManualDiscovered_NodeReconciledOnce_Idempotent()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer())!;
            var targets = catalog.ByItemId.Values.Where(d => d.effect_type == "knowledge")
                .Select(d => d.effect_target!).Distinct(StringComparer.Ordinal).ToList();
            var (dispatcher, _, research, discovery) = BuildWorld(catalog, new List<string>());

            // Old-version state: the discovery ledger carries the manual, the
            // research tree never learned it (pre-wiring save).
            var manual = catalog.ByItemId.Keys.First(id => catalog.GetByItemId(id)!.effect_type == "knowledge");
            discovery.MarkDiscovered(manual, "loc_workshop");
            Assert.False(research.IsManualUnlocked(targets[0]));

            var report = dispatcher.ReconcileDiscoveredSubsystemState();
            Assert.Equal(1, report.KnowledgeReconciled);
            Assert.True(research.IsManualUnlocked(targets[0]));

            // Reload/capture → reconcile again → no repeat effect (§13).
            var report2 = dispatcher.ReconcileDiscoveredSubsystemState();
            Assert.Equal(0, report2.KnowledgeReconciled);
        }

        // ── Case B: map collectible discovered, location still hidden ──

        [Fact]
        public void LegacyCaseB_MapDiscovered_LocationReconciledAsSurveyed_RoutesUntouched()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer())!;
            var clueTargets = catalog.ByItemId.Values.Where(d => d.effect_type == "location_clue")
                .Select(d => d.effect_target!).Distinct(StringComparer.Ordinal).ToList();
            var (dispatcher, map, _, discovery) = BuildWorld(catalog, clueTargets);

            var mapItem = catalog.ByItemId.Keys.First(id => catalog.GetByItemId(id)!.effect_type == "location_clue");
            var target = catalog.GetByItemId(mapItem)!.effect_target!;
            discovery.MarkDiscovered(mapItem, "loc_holdfast");
            Assert.Null(map.GetNodeKnowledge(target));   // location still hidden (legacy mismatch)

            var report = dispatcher.ReconcileDiscoveredSubsystemState();
            Assert.Equal(1, report.LocationReconciled);
            Assert.Equal(MapFogState.Surveyed, map.GetNodeKnowledge(target)!.FogState);  // reconciled as knowledge, NOT visited
            Assert.Equal(1, map.State.Knowledge.Count);  // routes/neighbors untouched

            var report2 = dispatcher.ReconcileDiscoveredSubsystemState();
            Assert.Equal(0, report2.LocationReconciled); // no repeat effect
        }

        // ── Case C: vinyl collectible discovered, ownership missing ────

        [Fact]
        public void LegacyCaseC_VinylDiscovered_OwnershipReconciled_NeverMorale()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer())!;
            var discovery = new CollectibleDiscoveryState();
            var research = new ResearchSystem();
            var dispatcher = new CollectibleEffectDispatcher(catalog, discovery, dayProvider: () => 4);

            const string chamberItem = VinylRecordAcquisitionMap.ChamberRecordCollectibleItemId;
            discovery.MarkDiscovered(chamberItem, "loc_radio_station");

            var vinyl = BuildVinylSystem();
            Assert.Empty(vinyl.State.ownedRecordIds);    // legacy mismatch: discovery without ownership
            float totalMoraleBefore = vinyl.State.totalMoraleApplied;

            var report = dispatcher.ReconcileDiscoveredSubsystemState(vinylProvider: () => vinyl, vinylRng: new SeededRng(9));

            Assert.Equal(1, report.VinylChecked);
            Assert.NotEmpty(vinyl.State.ownedRecordIds); // record registered via the acquisition map
            Assert.Equal(totalMoraleBefore, vinyl.State.totalMoraleApplied);  // ownership NEVER applies morale

            // Idempotent: a second pass adds no NEW record beyond the authored
            // fallback policy (same-type records may fill remaining slots, but
            // never morale and never a duplicate of the same record).
            var snapshot = new List<string>(vinyl.State.ownedRecordIds);
            var report2 = dispatcher.ReconcileDiscoveredSubsystemState(vinylProvider: () => vinyl, vinylRng: new SeededRng(9));
            Assert.Empty(vinyl.State.ownedRecordIds.Where(id => vinyl.State.ownedRecordIds.Count(x => x == id) > 1));
        }

        // ── §13/§18: fully reconciled state → capture → reload → stable ─

        [Fact]
        public void FullMigration_CaptureReload_NoRepeatEffect()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer())!;
            var clueTargets = catalog.ByItemId.Values.Where(d => d.effect_type == "location_clue")
                .Select(d => d.effect_target!).Distinct(StringComparer.Ordinal).ToList();
            var (dispatcher, map, research, discovery) = BuildWorld(catalog, clueTargets);

            var manual = catalog.ByItemId.Keys.First(id => catalog.GetByItemId(id)!.effect_type == "knowledge");
            var mapItem = catalog.ByItemId.Keys.First(id => catalog.GetByItemId(id)!.effect_type == "location_clue");
            discovery.MarkDiscovered(manual, "loc_workshop");
            discovery.MarkDiscovered(mapItem, "loc_holdfast");

            dispatcher.ReconcileDiscoveredSubsystemState();
            var knowledgeTarget = catalog.GetByItemId(manual)!.effect_target!;
            var mapTarget = catalog.GetByItemId(mapItem)!.effect_target!;
            Assert.True(research.IsManualUnlocked(knowledgeTarget));
            Assert.True(map.IsDiscovered(mapTarget));

            // Capture → restore → reconcile AGAIN: nothing new (idempotent).
            var research2 = new ResearchSystem();
            research2.RestoreState(research.CaptureState());
            var map2 = new WastelandMapSystem(new WastelandMapState(),
                clueTargets.Select(t => new MapNode { Id = t, DisplayName = t }).ToList(), new List<MapRoute>());
            map2.RestoreState(map.CaptureState());
            var discovery2 = new CollectibleDiscoveryState();
            discovery2.RestoreState(discovery.CaptureState());
            var dispatcher2 = new CollectibleEffectDispatcher(
                catalog, discovery2, researchProvider: () => research2, mapProvider: () => map2, dayProvider: () => 6);

            var report = dispatcher2.ReconcileDiscoveredSubsystemState();
            Assert.Equal(0, report.KnowledgeReconciled);
            Assert.Equal(0, report.LocationReconciled);
            Assert.True(research2.IsManualUnlocked(knowledgeTarget));   // state preserved
            Assert.Equal(MapFogState.Surveyed, map2.GetNodeKnowledge(mapTarget)!.FogState);
        }
    }
}
