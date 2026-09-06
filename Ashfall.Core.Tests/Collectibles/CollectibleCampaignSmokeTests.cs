using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Collectibles
{
    /// <summary>
    /// Workstream D (Task 8): Deterministic End-to-End Campaign Smoke Test.
    /// Proves that under seed 42, the complete 20-scavenge lifecycle (scavenge, acquire,
    /// discover, dispatch, persist, restore, deduplicate, continue, hash) operates
    /// deterministically across subsystem boundaries with zero collateral state drift.
    /// </summary>
    public class CollectibleCampaignSmokeTests
    {
        private const int CampaignSeed = 42;
        private static readonly string RepoRoot = FindRepoRoot();
        private static readonly string DataDir = Path.Combine(RepoRoot, "Assets", "StreamingAssets", "Data");

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

        private static CollectibleCatalog LoadCollectibles()
        {
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var catalog = CollectibleCatalogLoader.Load(DataDir, fileIO, serializer);
            Assert.NotNull(catalog);
            return catalog!;
        }

        private class CampaignStateSnapshot
        {
            public List<string> Inventory { get; set; } = new();
            public CollectibleDiscoverySave DiscoverySave { get; set; } = null!;
            public UniqueClaimSave UniqueClaimSave { get; set; } = null!;
            public CollectibleTutorialSave TutorialSave { get; set; } = null!;
            public float SurvivorMorale { get; set; }
            public float SurvivorHealth { get; set; }
            public List<string> UnlockedManuals { get; set; } = new();
            public JournalSave JournalSave { get; set; } = null!;
            public WastelandMapState MapState { get; set; } = null!;
            public ulong RngState { get; set; }
            public int ScavengeStep { get; set; }
        }

        private class CampaignRuntime
        {
            public Ashfall.Core.SeededRng Rng;
            public CollectibleCatalog Catalog;
            public CollectibleDiscoveryState Discovery;
            public UniqueItemClaimRegistry UniqueClaims;
            public CollectibleTutorialTracker TutorialTracker;
            public NeedsSystem Needs;
            public SurvivorNeedsState Survivor;
            public ResearchSystem Research;
            public JournalSystem Journal;
            public WastelandMapSystem Map;
            public CollectibleEffectDispatcher Dispatcher;
            public List<string> Inventory = new();
            public List<string> EventTrace = new();
            public int ScavengeStep;

            public CampaignRuntime(int seed, CollectibleCatalog catalog)
            {
                Rng = new Ashfall.Core.SeededRng(seed);
                Catalog = catalog;
                Discovery = new CollectibleDiscoveryState();

                var uniqueIds = catalog.ByItemId.Values
                    .Where(c => c.unique)
                    .Select(c => c.item_id)
                    .ToList();
                UniqueClaims = new UniqueItemClaimRegistry(uniqueIds);

                TutorialTracker = new CollectibleTutorialTracker();

                Needs = new NeedsSystem();
                Survivor = new SurvivorNeedsState
                {
                    Id = "survivor_alpha",
                    Morale = 50f,
                    Health = 100f
                };
                Needs.Register(Survivor);

                Research = new ResearchSystem();
                Journal = new JournalSystem();

                var mapNodes = new List<MapNode>();
                for (int i = 1; i <= 20; i++)
                {
                    mapNodes.Add(new MapNode
                    {
                        Id = $"loc_sector_{i:D2}",
                        DisplayName = $"Sector {i:D2}",
                        StartingUnlocked = (i == 1)
                    });
                }
                // Add clue nodes referenced by location_clue collectibles
                foreach (var c in catalog.ByItemId.Values.Where(c => c.effect_type == "location_clue"))
                {
                    if (!string.IsNullOrEmpty(c.effect_target) && mapNodes.All(n => n.Id != c.effect_target))
                    {
                        mapNodes.Add(new MapNode { Id = c.effect_target, DisplayName = $"Clue target {c.effect_target}" });
                    }
                }

                Map = new WastelandMapSystem(new WastelandMapState(), mapNodes, new List<MapRoute>());

                Dispatcher = new CollectibleEffectDispatcher(
                    Catalog,
                    Discovery,
                    needsProvider: () => Needs,
                    researchProvider: () => Research,
                    journalProvider: () => Journal,
                    mapProvider: () => Map,
                    dayProvider: () => 1 + (ScavengeStep / 2));

                Dispatcher.OnCollectibleDiscovered += TutorialTracker.OnCollectibleDiscovered;
            }

            public void ExecuteScavenge(string locationId, IReadOnlyList<string> candidateItems)
            {
                ScavengeStep++;
                EventTrace.Add($"Scavenge({locationId})");

                // Deterministic selection: pick 1-2 items from candidate pool
                int itemCount = 1 + Rng.Next(0, 2);
                var selectedLoot = new List<string>();

                for (int i = 0; i < itemCount; i++)
                {
                    int index = Rng.Next(0, candidateItems.Count);
                    string candidate = candidateItems[index];

                    // Uniqueness enforcement: if unique and already claimed, skip generation
                    if (UniqueClaims.IsUniqueItem(candidate))
                    {
                        if (!UniqueClaims.IsAvailable(candidate))
                        {
                            // Fallback to common sustenance item
                            candidate = "clean_water";
                        }
                        else
                        {
                            UniqueClaims.TryClaim(candidate);
                        }
                    }

                    selectedLoot.Add(candidate);
                }

                selectedLoot.Sort(StringComparer.Ordinal);
                EventTrace.Add($"LootSelected({string.Join(",", selectedLoot)})");

                foreach (var item in selectedLoot)
                {
                    Inventory.Add(item);

                    if (Catalog.IsCollectible(item))
                    {
                        var result = Dispatcher.DispatchOnAcquire(item, locationId);
                        if (result.DiscoveryRegistered)
                        {
                            EventTrace.Add($"CollectibleDiscovered({item},{locationId})");
                            EventTrace.Add($"EffectDispatched({result.EffectType},{Catalog.GetByItemId(item)?.effect_target ?? ""},{result.EffectApplied})");
                        }
                    }
                }

                while (TutorialTracker.TryDequeue(out var tut))
                {
                    EventTrace.Add($"TutorialQueued({tut!.Id})");
                }

                var markers = CollectibleMapProjector.ProjectMarkers(Discovery, Catalog);
                foreach (var m in markers.Where(m => m.LocationId == locationId))
                {
                    EventTrace.Add($"MapMarkerProjected({m.MarkerId})");
                }
            }

            public CampaignStateSnapshot CaptureSnapshot()
            {
                var inv = new List<string>(Inventory);

                var manuals = new List<string>(Research.State.unlockedIds);
                manuals.Sort(StringComparer.Ordinal);

                return new CampaignStateSnapshot
                {
                    Inventory = inv,
                    DiscoverySave = Discovery.CaptureState(),
                    UniqueClaimSave = UniqueClaims.CaptureState(),
                    TutorialSave = TutorialTracker.CaptureState(),
                    SurvivorMorale = Survivor.Morale,
                    SurvivorHealth = Survivor.Health,
                    UnlockedManuals = manuals,
                    JournalSave = Journal.CaptureState(),
                    MapState = Map.CaptureState(),
                    RngState = Rng.PeekState(),
                    ScavengeStep = ScavengeStep
                };
            }

            public void RestoreFromSnapshot(CampaignStateSnapshot snapshot)
            {
                // CRITICAL RESTORE ORDER:
                // 1. Restore authoritative domain stores
                Inventory = new List<string>(snapshot.Inventory);
                Discovery.RestoreState(snapshot.DiscoverySave);
                UniqueClaims.RestoreState(snapshot.UniqueClaimSave);
                TutorialTracker.RestoreState(snapshot.TutorialSave);
                Survivor.Morale = snapshot.SurvivorMorale;
                Survivor.Health = snapshot.SurvivorHealth;

                var researchState = new ResearchState
                {
                    unlockedIds = new List<string>(snapshot.UnlockedManuals)
                };
                Research = new ResearchSystem(state: researchState);

                Journal = new JournalSystem();
                Journal.RestoreState(snapshot.JournalSave);

                Map.RestoreState(snapshot.MapState);
                Rng.SeekState(snapshot.RngState);
                ScavengeStep = snapshot.ScavengeStep;

                EventTrace.Add("Restore");
            }
        }

        private static List<List<string>> BuildLocationCandidateTables(CollectibleCatalog catalog)
        {
            // Distribute all 40 collectibles across 20 locations (2 distinct collectibles per location)
            // plus common sustenance items
            var collectibles = catalog.ByItemId.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList();
            Assert.Equal(40, collectibles.Count);

            var tables = new List<List<string>>(20);
            for (int i = 0; i < 20; i++)
            {
                var candidates = new List<string>
                {
                    collectibles[i * 2],
                    collectibles[i * 2 + 1],
                    "clean_water",
                    "canned_food",
                    "bandage"
                };
                tables.Add(candidates);
            }
            return tables;
        }

        [Fact]
        public void CollectibleCampaignSmoke_CatalogValuesWithinBounds()
        {
            var catalog = LoadCollectibles();
            Assert.Equal(40, catalog.ByItemId.Count);

            // Load items.json for trade value and weight
            string itemsJson = File.ReadAllText(Path.Combine(DataDir, "items.json"));
            var doc = JsonDocument.Parse(itemsJson);
            var itemMap = new Dictionary<string, (float tradeValue, float weight)>(StringComparer.Ordinal);

            foreach (var elem in doc.RootElement.GetProperty("items").EnumerateArray())
            {
                string id = elem.GetProperty("id").GetString()!;
                float tv = elem.TryGetProperty("tradeValue", out var tvProp) ? tvProp.GetSingle() : 0f;
                float w = elem.TryGetProperty("weight", out var wProp) ? wProp.GetSingle() : 0f;
                itemMap[id] = (tv, w);
            }

            foreach (var kv in catalog.ByItemId)
            {
                string itemId = kv.Key;
                Assert.True(itemMap.TryGetValue(itemId, out var stats), $"Item {itemId} must exist in items.json");
                Assert.True(stats.tradeValue > 0f && stats.tradeValue < 100f,
                    $"Item {itemId} trade value {stats.tradeValue} must be > 0 and < 100");
                Assert.True(stats.weight >= 0f && stats.weight < 5.0f,
                    $"Item {itemId} weight {stats.weight} must be >= 0 and < 5.0 kg");
            }
        }

        [Fact]
        public void CollectibleCampaignSmoke_Seed42_CompletesFullLifecycle()
        {
            var catalog = LoadCollectibles();
            var tables = BuildLocationCandidateTables(catalog);
            var runtime = new CampaignRuntime(CampaignSeed, catalog);

            // STAGE 1: First 10 scavenges
            int collectibleCountBeforeSave = 0;
            int effectBearingCountBeforeSave = 0;

            for (int i = 0; i < 10; i++)
            {
                string loc = $"loc_sector_{(i + 1):D2}";
                int prevDiscCount = runtime.Discovery.Count;
                runtime.ExecuteScavenge(loc, tables[i]);

                int newDisc = runtime.Discovery.Count - prevDiscCount;
                if (newDisc > 0)
                {
                    collectibleCountBeforeSave += newDisc;
                }
            }

            foreach (var kv in runtime.Discovery.DiscoveryLocations)
            {
                var def = catalog.GetByItemId(kv.Key);
                if (def != null && !string.IsNullOrEmpty(def.effect_type) && def.effect_type != "none")
                {
                    effectBearingCountBeforeSave++;
                }
            }

            Assert.True(collectibleCountBeforeSave >= 3,
                $"Expected >= 3 collectibles in first 10 scavenges, got {collectibleCountBeforeSave}");
            Assert.True(effectBearingCountBeforeSave >= 1,
                $"Expected >= 1 effect-bearing collectible in first 10 scavenges, got {effectBearingCountBeforeSave}");

            // STAGE 2: Capture Snapshot
            var snapshot = runtime.CaptureSnapshot();
            runtime.EventTrace.Add($"SaveHash({snapshot.DiscoverySave.discovered_ids.Length})");

            // STAGE 3: Restore into fresh runtime
            var freshRuntime = new CampaignRuntime(CampaignSeed, catalog);
            freshRuntime.RestoreFromSnapshot(snapshot);

            // Assert exact semantic equality after restore
            Assert.Equal(runtime.Inventory, freshRuntime.Inventory);
            Assert.Equal(runtime.Discovery.Count, freshRuntime.Discovery.Count);
            Assert.Equal(runtime.Discovery.DiscoveryLocations.Count, freshRuntime.Discovery.DiscoveryLocations.Count);
            Assert.Equal(runtime.Survivor.Morale, freshRuntime.Survivor.Morale);
            Assert.Equal(runtime.Survivor.Health, freshRuntime.Survivor.Health);
            Assert.Equal(runtime.TutorialTracker.SeenCount, freshRuntime.TutorialTracker.SeenCount);
            Assert.Equal(runtime.TutorialTracker.QueueCount, freshRuntime.TutorialTracker.QueueCount);
            Assert.Equal(runtime.Research.State.unlockedIds, freshRuntime.Research.State.unlockedIds);
            Assert.Equal(runtime.Journal.CaptureState().Knowledge.DiscoveredKeys, freshRuntime.Journal.CaptureState().Knowledge.DiscoveredKeys);
            Assert.Equal(runtime.Map.DiscoveredNodes, freshRuntime.Map.DiscoveredNodes);

            // STAGE 4: Continue next 10 scavenges on restored runtime
            for (int i = 10; i < 20; i++)
            {
                string loc = $"loc_sector_{(i + 1):D2}";
                freshRuntime.ExecuteScavenge(loc, tables[i]);
            }

            Assert.Equal(20, freshRuntime.ScavengeStep);
            Assert.True(freshRuntime.Discovery.Count >= collectibleCountBeforeSave);

            // STAGE 5: Explicit Reacquisition Probe
            // Pick an already-discovered item from the first 10 scavenges
            string discoveredItem = snapshot.DiscoverySave.discovered_ids[0];
            float moraleBeforeReacquire = freshRuntime.Survivor.Morale;
            int tutorialQueueBefore = freshRuntime.TutorialTracker.QueueCount;
            int markersBefore = CollectibleMapProjector.ProjectMarkers(freshRuntime.Discovery, catalog).Count;

            var probeResult = freshRuntime.Dispatcher.DispatchOnAcquire(discoveredItem, "loc_probe_market");
            Assert.True(probeResult.AlreadyDiscovered);
            Assert.False(probeResult.DiscoveryRegistered);
            Assert.False(probeResult.EffectApplied);
            Assert.Equal(moraleBeforeReacquire, freshRuntime.Survivor.Morale);
            Assert.Equal(tutorialQueueBefore, freshRuntime.TutorialTracker.QueueCount);
            Assert.Equal(markersBefore, CollectibleMapProjector.ProjectMarkers(freshRuntime.Discovery, catalog).Count);
        }

        [Fact]
        public void CollectibleCampaignSmoke_UniqueCollectiblesAppearAtMostOnce()
        {
            var catalog = LoadCollectibles();
            var tables = BuildLocationCandidateTables(catalog);
            var runtime = new CampaignRuntime(CampaignSeed, catalog);

            for (int i = 0; i < 10; i++)
                runtime.ExecuteScavenge($"loc_sector_{(i + 1):D2}", tables[i]);

            var snap = runtime.CaptureSnapshot();
            var fresh = new CampaignRuntime(CampaignSeed, catalog);
            fresh.RestoreFromSnapshot(snap);

            for (int i = 10; i < 20; i++)
                fresh.ExecuteScavenge($"loc_sector_{(i + 1):D2}", tables[i]);

            // Across 20 scavenges, verify unique items appear at most once in inventory
            var uniqueItems = catalog.ByItemId.Values.Where(c => c.unique).Select(c => c.item_id).ToHashSet();
            var uniqueCounts = new Dictionary<string, int>(StringComparer.Ordinal);

            foreach (var item in fresh.Inventory)
            {
                if (uniqueItems.Contains(item))
                {
                    uniqueCounts[item] = uniqueCounts.GetValueOrDefault(item) + 1;
                }
            }

            foreach (var (uniqueId, count) in uniqueCounts)
            {
                Assert.True(count <= 1, $"Unique item {uniqueId} was generated {count} times (must be <= 1)");
            }
        }

        [Fact]
        public void CollectibleCampaignSmoke_ThreeRunsProduceIdenticalTrace()
        {
            var catalog = LoadCollectibles();
            var tables = BuildLocationCandidateTables(catalog);

            string RunScenario()
            {
                var runtime = new CampaignRuntime(CampaignSeed, catalog);

                for (int i = 0; i < 10; i++)
                    runtime.ExecuteScavenge($"loc_sector_{(i + 1):D2}", tables[i]);

                var snapshot = runtime.CaptureSnapshot();
                runtime.EventTrace.Add($"SaveHash({snapshot.DiscoverySave.discovered_ids.Length})");

                var fresh = new CampaignRuntime(CampaignSeed, catalog);
                fresh.EventTrace = new List<string>(runtime.EventTrace);
                fresh.RestoreFromSnapshot(snapshot);

                for (int i = 10; i < 20; i++)
                    fresh.ExecuteScavenge($"loc_sector_{(i + 1):D2}", tables[i]);

                return string.Join("\n", fresh.EventTrace);
            }

            string trace1 = RunScenario();
            string trace2 = RunScenario();
            string trace3 = RunScenario();

            Assert.Equal(trace1, trace2);
            Assert.Equal(trace2, trace3);
        }

        [Fact]
        public void CollectibleCampaignSmoke_ThreeRunsProduceIdenticalFinalHash()
        {
            var catalog = LoadCollectibles();
            var tables = BuildLocationCandidateTables(catalog);

            (string hash, string canonicalJson) RunAndHash()
            {
                var runtime = new CampaignRuntime(CampaignSeed, catalog);

                for (int i = 0; i < 10; i++)
                    runtime.ExecuteScavenge($"loc_sector_{(i + 1):D2}", tables[i]);

                var snapshot = runtime.CaptureSnapshot();

                var fresh = new CampaignRuntime(CampaignSeed, catalog);
                fresh.RestoreFromSnapshot(snapshot);

                for (int i = 10; i < 20; i++)
                    fresh.ExecuteScavenge($"loc_sector_{(i + 1):D2}", tables[i]);

                // Canonical state dictionary with deterministic ordinal keys
                var canonicalState = new SortedDictionary<string, object>(StringComparer.Ordinal)
                {
                    ["1_inventory"] = fresh.Inventory.OrderBy(x => x, StringComparer.Ordinal).ToList(),
                    ["2_discovered_ids"] = fresh.Discovery.CaptureState().discovered_ids.OrderBy(x => x, StringComparer.Ordinal).ToList(),
                    ["3_discovery_locations"] = fresh.Discovery.DiscoveryLocations.OrderBy(kv => kv.Key, StringComparer.Ordinal).ToDictionary(kv => kv.Key, kv => kv.Value),
                    ["4_morale"] = fresh.Survivor.Morale.ToString("F4", CultureInfo.InvariantCulture),
                    ["5_health"] = fresh.Survivor.Health.ToString("F4", CultureInfo.InvariantCulture),
                    ["6_research_manuals"] = fresh.Research.State.unlockedIds.OrderBy(x => x, StringComparer.Ordinal).ToList(),
                    ["7_journal_knowledge"] = fresh.Journal.CaptureState().Knowledge.DiscoveredKeys.OrderBy(x => x, StringComparer.Ordinal).ToList(),
                    ["8_map_discovered"] = fresh.Map.DiscoveredNodes.OrderBy(x => x, StringComparer.Ordinal).ToList(),
                    ["9_unique_claims"] = fresh.UniqueClaims.CaptureState().claimed_unique_ids.OrderBy(x => x, StringComparer.Ordinal).ToList(),
                    ["a_tutorial_seen"] = fresh.TutorialTracker.CaptureState().seen_tutorials.OrderBy(x => x, StringComparer.Ordinal).ToList(),
                    ["b_rng_state"] = fresh.Rng.PeekState().ToString(CultureInfo.InvariantCulture)
                };

                string json = JsonSerializer.Serialize(canonicalState);
                using var sha256 = SHA256.Create();
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
                string hashStr = Convert.ToHexString(hashBytes);

                return (hashStr, json);
            }

            var (hash1, json1) = RunAndHash();
            var (hash2, json2) = RunAndHash();
            var (hash3, json3) = RunAndHash();

            Assert.True(hash1 == hash2, $"Hash mismatch between run 1 and run 2:\nRun 1: {hash1}\nRun 2: {hash2}\nDiff: {FindDiff(json1, json2)}");
            Assert.True(hash2 == hash3, $"Hash mismatch between run 2 and run 3:\nRun 2: {hash2}\nRun 3: {hash3}\nDiff: {FindDiff(json2, json3)}");
        }

        private static string FindDiff(string s1, string s2)
        {
            if (s1 == s2) return "No diff";
            int minLen = Math.Min(s1.Length, s2.Length);
            for (int i = 0; i < minLen; i++)
            {
                if (s1[i] != s2[i])
                {
                    int start = Math.Max(0, i - 30);
                    int len = Math.Min(60, minLen - start);
                    return $"Divergence at char {i}:\n  s1: ...{s1.Substring(start, len)}...\n  s2: ...{s2.Substring(start, len)}...";
                }
            }
            return $"Length difference: {s1.Length} vs {s2.Length}";
        }
    }
}
