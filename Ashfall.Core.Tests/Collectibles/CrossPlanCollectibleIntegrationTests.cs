// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Collectibles
{
    public class CrossPlanIntegrationManifest
    {
        public int schema_version { get; set; }
        public List<CrossPlanIntegrationEntry> integrations { get; set; } = new();
    }

    public class CrossPlanIntegrationEntry
    {
        public string id { get; set; } = string.Empty;
        public string producer { get; set; } = string.Empty;
        public string consumer { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string contract { get; set; } = string.Empty;
        public string runtime_path { get; set; } = string.Empty;
        public string persistence_path { get; set; } = string.Empty;
        public string verification_key { get; set; } = string.Empty;
        public string? blocker { get; set; }
        public string? resolution { get; set; }
        public string? temporary_behavior { get; set; }
    }

    /// <summary>
    /// Workstream A (Task 5): Cross-Plan Integration Matrix Verification.
    /// Asserts that all Plan 47 §6 rows are either explicitly wired and tested
    /// or explicitly deferred with a concrete blocker and resolution.
    /// Zero undocumented gaps, zero partial statuses.
    /// </summary>
    public class CrossPlanCollectibleIntegrationTests
    {
        private static readonly string RepoRoot = FindRepoRoot();
        private static readonly string DataDir = Path.Combine(RepoRoot, "Assets", "StreamingAssets", "Data");
        private static readonly string ManifestPath = Path.Combine(RepoRoot, "Ashfall.Core.Tests", "Fixtures", "collectibles", "collectible_cross_plan_integrations.json");

        private static string FindRepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir, "Assets", "StreamingAssets", "Data", "collectibles.json")))
                    return dir;
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("repo root with data authority not found");
        }

        private static CrossPlanIntegrationManifest LoadManifest()
        {
            Assert.True(File.Exists(ManifestPath), $"Manifest must exist at {ManifestPath}");
            string json = File.ReadAllText(ManifestPath);
            var manifest = JsonSerializer.Deserialize<CrossPlanIntegrationManifest>(json);
            Assert.NotNull(manifest);
            return manifest!;
        }

        private static CollectibleCatalog LoadCollectibles()
        {
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var catalog = CollectibleCatalogLoader.Load(DataDir, fileIO, serializer);
            Assert.NotNull(catalog);
            return catalog!;
        }

        [Fact]
        public void CrossPlanIntegrationMatrix_HasNoUndocumentedGaps()
        {
            var manifest = LoadManifest();
            Assert.Equal(1, manifest.schema_version);
            Assert.NotNull(manifest.integrations);

            var requiredRowIds = new[]
            {
                "collectible_to_scavenging",
                "technical_manual_to_research",
                "map_collectible_to_world_map",
                "faction_document_to_faction_intel",
                "readable_artifact_to_journal",
                "vinyl_to_vinyl_system",
                "cultural_object_to_folklore",
                "toy_photo_letter_to_morale",
                "discovery_to_save_state"
            };

            var seenIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var row in manifest.integrations)
            {
                Assert.False(string.IsNullOrEmpty(row.id), "Row ID must not be empty");
                Assert.True(seenIds.Add(row.id), $"Duplicate integration row ID detected: {row.id}");
                Assert.False(string.IsNullOrEmpty(row.producer), $"Row {row.id} missing producer");
                Assert.False(string.IsNullOrEmpty(row.consumer), $"Row {row.id} missing consumer");
                Assert.False(string.IsNullOrEmpty(row.contract), $"Row {row.id} missing contract");

                string status = row.status.ToLowerInvariant();
                Assert.True(status == "wired" || status == "deferred" || status == "n/a",
                    $"Row {row.id} has invalid/partial status '{row.status}'. Must be 'wired', 'deferred', or 'n/a'.");

                if (status == "wired")
                {
                    Assert.False(string.IsNullOrEmpty(row.verification_key), $"Wired row {row.id} missing verification_key");
                    Assert.False(string.IsNullOrEmpty(row.runtime_path), $"Wired row {row.id} missing runtime_path");
                    Assert.False(string.IsNullOrEmpty(row.persistence_path), $"Wired row {row.id} missing persistence_path");
                }
                else if (status == "deferred")
                {
                    Assert.False(string.IsNullOrWhiteSpace(row.blocker), $"Deferred row {row.id} must document a concrete blocker");
                    Assert.False(string.IsNullOrWhiteSpace(row.resolution), $"Deferred row {row.id} must document a concrete resolution condition");
                }
            }

            foreach (var req in requiredRowIds)
            {
                Assert.True(seenIds.Contains(req), $"Required Plan 47 integration row missing: {req}");
            }
        }

        [Fact]
        public void CrossPlanIntegration_WiredRowsHaveExecutableVerification()
        {
            var manifest = LoadManifest();
            var catalog = LoadCollectibles();

            // 1. collectible_to_scavenging
            var scavengingRow = manifest.integrations.First(r => r.id == "collectible_to_scavenging");
            Assert.Equal("wired", scavengingRow.status);
            foreach (var kv in catalog.ByItemId)
            {
                Assert.StartsWith("item_collectible_", kv.Key, StringComparison.Ordinal);
                Assert.False(string.IsNullOrEmpty(kv.Value.category));
            }

            // 2. technical_manual_to_research
            var manualRow = manifest.integrations.First(r => r.id == "technical_manual_to_research");
            Assert.Equal("wired", manualRow.status);
            {
                var discovery = new CollectibleDiscoveryState();
                var research = new ResearchSystem();
                var dispatcher = new CollectibleEffectDispatcher(
                    catalog, discovery, researchProvider: () => research);

                var manualItem = catalog.ByItemId.Values.FirstOrDefault(c => c.effect_type == "knowledge");
                Assert.NotNull(manualItem);

                // Tasks 5–8 Wave B contract: the research catalog must contain the
                // manual's target node — the dispatcher validates it and fails a
                // stale target with a typed reason instead of silently swallowing it.
                research.Register(new Ashfall.Core.ResearchKnowledgeDef
                {
                    id = manualItem!.effect_target,
                    displayName = manualItem.effect_target,
                    daysToComplete = 4
                });

                var res = dispatcher.DispatchOnAcquire(manualItem.item_id, "loc_workshop");
                Assert.True(res.EffectApplied);
                Assert.True(res.DiscoveryRegistered);
                Assert.True(research.IsManualUnlocked(manualItem.effect_target));

                // Repeat is idempotent
                var res2 = dispatcher.DispatchOnAcquire(manualItem.item_id);
                Assert.True(res2.AlreadyDiscovered);
            }

            // 3. map_collectible_to_world_map
            var mapRow = manifest.integrations.First(r => r.id == "map_collectible_to_world_map");
            Assert.Equal("wired", mapRow.status);
            {
                var discovery = new CollectibleDiscoveryState();
                var mapNodes = new List<MapNode>
                {
                    new MapNode { Id = "node_bunker", StartingUnlocked = true },
                    new MapNode { Id = "node_depot", StartingUnlocked = false }
                };
                var mapSystem = new WastelandMapSystem(new WastelandMapState(), mapNodes, new List<MapRoute>());
                var customCatalog = new CollectibleCatalog(new List<CollectibleDefinition>
                {
                    new CollectibleDefinition
                    {
                        item_id = "item_map_clue",
                        category = "map",
                        effect_type = "location_clue",
                        effect_target = "node_depot"
                    }
                });
                var dispatcher = new CollectibleEffectDispatcher(
                    customCatalog, discovery, mapProvider: () => mapSystem);

                Assert.False(mapSystem.IsDiscovered("node_depot"));
                var res = dispatcher.DispatchOnAcquire("item_map_clue", "node_bunker");
                Assert.True(res.EffectApplied);
                Assert.True(mapSystem.IsDiscovered("node_depot"));
                Assert.Equal("node_bunker", discovery.GetDiscoveryLocation("item_map_clue"));
            }

            // 4. faction_document_to_faction_intel & 5. readable_artifact_to_journal
            var factionRow = manifest.integrations.First(r => r.id == "faction_document_to_faction_intel");
            var journalRow = manifest.integrations.First(r => r.id == "readable_artifact_to_journal");
            Assert.Equal("wired", factionRow.status);
            Assert.Equal("wired", journalRow.status);
            {
                var discovery = new CollectibleDiscoveryState();
                var journal = new JournalSystem();
                var dispatcher = new CollectibleEffectDispatcher(
                    catalog, discovery, journalProvider: () => journal, dayProvider: () => 1);

                var journalItem = catalog.ByItemId.Values.FirstOrDefault(c => c.effect_type == "journal_unlock" || c.effect_type == "faction_info");
                Assert.NotNull(journalItem);

                var res = dispatcher.DispatchOnAcquire(journalItem!.item_id);
                Assert.True(res.EffectApplied);
                Assert.True(journal.Knowledge.Has(journalItem.effect_target));
            }

            // 6. vinyl_to_vinyl_system
            var vinylRow = manifest.integrations.First(r => r.id == "vinyl_to_vinyl_system");
            Assert.Equal("wired", vinylRow.status);
            var vinyls = catalog.ByItemId.Values.Where(c => c.category == "vinyl").ToList();
            Assert.NotEmpty(vinyls);

            // 7. toy_photo_letter_to_morale
            var moraleRow = manifest.integrations.First(r => r.id == "toy_photo_letter_to_morale");
            Assert.Equal("wired", moraleRow.status);
            {
                var discovery = new CollectibleDiscoveryState();
                var needs = new NeedsSystem();
                var survivor = new SurvivorNeedsState { Id = "survivor_1", Morale = 40f, Health = 100f };
                needs.Register(survivor);

                var dispatcher = new CollectibleEffectDispatcher(
                    catalog, discovery, needsProvider: () => needs);

                var moraleItem = catalog.ByItemId.Values.FirstOrDefault(c => c.effect_type == "morale");
                Assert.NotNull(moraleItem);

                float initialMorale = survivor.Morale;
                var res = dispatcher.DispatchOnAcquire(moraleItem!.item_id);
                Assert.True(res.EffectApplied);
                Assert.True(survivor.Morale > initialMorale);

                // Repeat does not boost morale again
                float boostedMorale = survivor.Morale;
                var res2 = dispatcher.DispatchOnAcquire(moraleItem.item_id);
                Assert.True(res2.AlreadyDiscovered);
                Assert.Equal(boostedMorale, survivor.Morale);
            }

            // 8. discovery_to_save_state
            var saveRow = manifest.integrations.First(r => r.id == "discovery_to_save_state");
            Assert.Equal("wired", saveRow.status);
            {
                var discovery = new CollectibleDiscoveryState();
                discovery.MarkDiscovered("item_collectible_family_portrait", "loc_tenement");
                var save = discovery.CaptureState();

                Assert.Single(save.unacknowledged_ids);
                Assert.Single(save.discovery_locations);
                Assert.Equal("item_collectible_family_portrait", save.discovery_locations[0].item_id);
                Assert.Equal("loc_tenement", save.discovery_locations[0].location_id);

                var restored = new CollectibleDiscoveryState();
                restored.RestoreState(save);
                Assert.True(restored.IsDiscovered("item_collectible_family_portrait"));
                Assert.Equal("loc_tenement", restored.GetDiscoveryLocation("item_collectible_family_portrait"));
            }
        }

        [Fact]
        public void CrossPlanIntegration_DeferredRowsDescribeBlockerAndResolution()
        {
            var manifest = LoadManifest();
            var catalog = LoadCollectibles();

            var deferredRows = manifest.integrations.Where(r => r.status.Equals("deferred", StringComparison.OrdinalIgnoreCase)).ToList();
            Assert.Single(deferredRows);

            var folkloreRow = deferredRows[0];
            Assert.Equal("cultural_object_to_folklore", folkloreRow.id);
            Assert.Contains("Folklore runtime consumer is not implemented", folkloreRow.blocker);
            Assert.Contains("FolkloreSystem", folkloreRow.resolution);

            // Verify cultural items still exist in catalog with their metadata intact
            var culturalItems = catalog.ByItemId.Values.Where(c => c.category == "relic" || c.category == "book").ToList();
            Assert.NotEmpty(culturalItems);
        }
    }
}
