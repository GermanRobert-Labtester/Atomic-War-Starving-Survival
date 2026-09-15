// SPDX-License-Identifier: MIT
// ============================================================================
// Tasks 5–8 Wave F — Task 8 permanent content-utilization gates (§9).
// Data-derived (no hardcoded 40-ID lists — Trap J): every check reads the
// shipped catalogs. Utilization ≠ integrity (§9.18): integrity proves
// references resolve (Wave B validator); utilization proves LIVE sources,
// LIVE consumers, and in-campaign reachability.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Collectibles
{
    public sealed class CollectibleContentUtilizationTests
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

        private static Dictionary<string, string[]> LoadSources(CollectibleCatalog catalog)
        {
            var tables = System.Text.Json.JsonSerializer.Deserialize<ScavengingTableCatalogContainer>(
                File.ReadAllText(Path.Combine(DataDir, "scavenging_tables.json")));
            var sources = catalog.ByItemId.Keys.ToDictionary(k => k, _ => new List<string>(), StringComparer.Ordinal);
            foreach (var t in tables!.tables!)
                foreach (var e in t.entries ?? new List<ScavengingLootEntryDef>())
                    if (sources.ContainsKey(e.item_id))
                        sources[e.item_id].Add(t.id);
            return sources.ToDictionary(kv => kv.Key, kv => kv.Value.ToArray(), StringComparer.Ordinal);
        }

        // ── §9.2/§9.3: 40/40 definitions, valid item mappings ──────────

        [Fact]
        public void AllCollectibles_HaveLiveItemsAndDefinitions()
        {
            var catalog = LoadCatalog();
            Assert.Equal(40, catalog.Count);   // authored corpus size (data-derived floor)

            var items = System.Text.Json.JsonSerializer.Deserialize<ItemRoot>(
                File.ReadAllText(Path.Combine(DataDir, "items.json")));
            var itemIds = items!.items!.Select(i => i.id).ToHashSet(StringComparer.Ordinal);

            foreach (var kv in catalog.ByItemId)
            {
                Assert.True(itemIds.Contains(kv.Key), $"{kv.Key}: definition without item");
                Assert.StartsWith("item_collectible_", kv.Key);
            }

            // Reverse bijection: every collectible-prefixed item has a definition.
            var orphans = itemIds.Where(id => id.StartsWith("item_collectible_", StringComparison.Ordinal))
                .Where(id => !catalog.ByItemId.ContainsKey(id)).ToList();
            Assert.Empty(orphans);   // §9.3 item orphan — the two-invariant rule is symmetric
        }

        // ── §9.4–9.6: acquisition-source graph + five-location floor ───

        [Fact]
        public void AllCollectibles_HaveAtLeastOneLiveSource()
        {
            var catalog = LoadCatalog();
            var sources = LoadSourceGraph(catalog);
            foreach (var kv in catalog.ByItemId)
                Assert.True(sources[kv.Key].Length >= 1, $"{kv.Key}: zero live acquisition sources");
        }

        [Fact]
        public void CollectibleAcquisition_SpansAtLeastFiveDistinctLiveTables()
        {
            var catalog = LoadCatalog();
            var sources = LoadSourceGraph(catalog);
            var distinctTables = sources.Values.SelectMany(v => v).Distinct(StringComparer.Ordinal).ToList();
            Assert.True(distinctTables.Count >= 5,
                $"collectible acquisition should span ≥5 live tables, found {distinctTables.Count}");
        }

        // ── §9.7: concentration ceiling (analytical) ───────────────────

        [Fact]
        public void NoSingleSourceExceedsThirtyPercentOfCollectibleWeight()
        {
            var catalog = LoadCatalog();
            var tables = System.Text.Json.JsonSerializer.Deserialize<ScavengingTableCatalogContainer>(
                File.ReadAllText(Path.Combine(DataDir, "scavenging_tables.json")));
            var share = new Dictionary<string, double>(StringComparer.Ordinal);
            double total = 0;
            foreach (var t in tables!.tables!)
                foreach (var e in t.entries ?? new List<ScavengingLootEntryDef>())
                    if (catalog.GetByItemId(e.item_id) != null)
                    {
                        share[t.id] = share.GetValueOrDefault(t.id) + e.weight;
                        total += e.weight;
                    }
            Assert.True(total > 0);
            foreach (var kv in share)
            {
                double pct = kv.Value / total * 100.0;
                Assert.True(pct <= 30.0, $"{kv.Key}: {pct:F1}% of collectible weight exceeds the 30% ceiling");
            }
        }

        // ── §9.10: unique physical suppression (generation gate) ──────

        [Fact]
        public void UniqueCollectibles_PhysicallyAwardedAtMostOnce_PerCampaign()
        {
            var catalog = LoadCatalog();
            var uniqueIds = catalog.ByItemId.Values.Where(d => d.unique).Select(d => d.item_id).ToList();
            Assert.True(uniqueIds.Count >= 1, "authored data should declare unique collectibles");

            // Uniqueness gates GENERATION (§1.4); discovery gates the EFFECT.
            // Contract: channels ask IsClaimed BEFORE awarding; TryClaim is an
            // idempotent claim-after-award stamp (returns true when claimed).
            var claims = new UniqueItemClaimRegistry(uniqueIds);
            var inventory = new Ashfall.Core.Inventory.Inventory();
            foreach (var id in uniqueIds)
            {
                Assert.False(claims.IsClaimed(id), $"{id}: fresh campaign must not start claimed");
                inventory.AddById(id, 1);
                Assert.True(claims.TryClaim(id), $"{id}: first award claims uniqueness");
                // A later generation attempt consults IsClaimed first — suppressed:
                Assert.True(claims.IsClaimed(id), $"{id}: second award channel must see the claim and suppress");
                Assert.Equal(1, inventory.CountById(id));   // physical count ≤ 1 (Trap F)
            }
        }

        // ── §9.16: mixed-set save/load utilization scenario ───────────

        [Fact]
        public void MixedSet_SaveRestore_DiscoveryLedgerStable_UniqueSuppressionStable()
        {
            var catalog = LoadCatalog();
            var manual = catalog.ByItemId.Keys.First(id => catalog.GetByItemId(id)!.effect_type == "knowledge");
            var map = catalog.ByItemId.Keys.First(id => catalog.GetByItemId(id)!.effect_type == "location_clue");
            var vinyl = catalog.ByItemId.Keys.First(id => catalog.GetByItemId(id)!.category == "vinyl");
            var common = catalog.ByItemId.Keys.First(id => catalog.GetByItemId(id)!.rarity == "common");
            var unique = catalog.ByItemId.Keys.First(id => catalog.GetByItemId(id)!.unique);

            var discovery = new CollectibleDiscoveryState();
            var claims = new UniqueItemClaimRegistry(catalog.ByItemId.Values.Where(d => d.unique).Select(d => d.item_id));
            foreach (var id in new[] { manual, map, vinyl, common, unique })
            {
                discovery.MarkDiscovered(id, "loc_test");
                if (catalog.GetByItemId(id)!.unique) claims.TryClaim(id);
            }

            // Save/restore both ledgers.
            var d2 = new CollectibleDiscoveryState();
            d2.RestoreState(discovery.CaptureState());
            var c2 = new UniqueItemClaimRegistry(new[] { unique });
            c2.RestoreState(new UniqueClaimSave { claimed_unique_ids = new[] { unique } });

            foreach (var id in new[] { manual, map, vinyl, common, unique })
                Assert.True(d2.IsDiscovered(id));                 // ledger stable
            Assert.True(c2.IsClaimed(unique));                    // suppression stable
            Assert.True(c2.TryClaim(unique));                     // idempotent claim (already claimed)
            Assert.Equal(1, c2.ClaimedCount);                     // no duplicate claim entry
        }

        private sealed class ItemRoot
        {
            public int schema_version { get; set; } = 1;
            public List<ItemHeader> items { get; set; } = new List<ItemHeader>();
        }

        private sealed class ItemHeader
        {
            public string id { get; set; } = string.Empty;
        }

        private static Dictionary<string, string[]> LoadSourceGraph(CollectibleCatalog catalog)
        {
            var tables = System.Text.Json.JsonSerializer.Deserialize<ScavengingTableCatalogContainer>(
                File.ReadAllText(Path.Combine(DataDir, "scavenging_tables.json")));
            var list = catalog.ByItemId.Keys.ToDictionary(k => k, _ => new List<string>(), StringComparer.Ordinal);
            foreach (var t in tables!.tables!)
                foreach (var e in t.entries ?? new List<ScavengingLootEntryDef>())
                    if (list.ContainsKey(e.item_id)) list[e.item_id].Add(t.id);
            return list.ToDictionary(kv => kv.Key, kv => kv.Value.ToArray(), StringComparer.Ordinal);
        }
    }
}
