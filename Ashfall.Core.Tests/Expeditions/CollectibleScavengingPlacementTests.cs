using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Collectible scavenging placement contract (collectibles flagship,
    /// Task 1): the required plan-exact placements exist with exact
    /// weight/rarity, at least 15 placements are live (bound to expedition
    /// destinations), all placed ids resolve, unique collectibles rely on
    /// definition-level suppression, changed tables still yield ordinary
    /// loot, and seeded sampling is stable.
    /// </summary>
    public class CollectibleScavengingPlacementTests
    {
        private static readonly string RepoRoot = FindRepoRoot();
        private static readonly string DataDir = Path.Combine(RepoRoot, "Assets", "StreamingAssets", "Data");

        private static readonly IFileIO FileIO = new FileSystemIO();
        private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();

        private static CollectibleCatalog LoadCollectibles() =>
            CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer)
                ?? throw new InvalidOperationException("collectibles.json must load");

        private static ScavengingTableCatalog LoadTables()
        {
            string raw = FileIO.ReadAllText(Path.Combine(DataDir, "scavenging_tables.json"));
            return ScavengingTableCatalog.LoadFromJson(raw, Serializer);
        }

        private static HashSet<string> LoadBoundTables()
        {
            var bound = new HashSet<string>(StringComparer.Ordinal);
            string raw = FileIO.ReadAllText(Path.Combine(DataDir, "expeditions.json"));
            var json = System.Text.Json.JsonDocument.Parse(raw);
            foreach (var exp in json.RootElement.GetProperty("expeditions").EnumerateArray())
            {
                if (exp.TryGetProperty("scavenging_table_id", out var t) &&
                    t.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    bound.Add(t.GetString()!);
                }
            }
            return bound;
        }

        private static List<(string ItemId, string TableId, int Weight, string Rarity)> LoadCollectibleEntries(
            ScavengingTableCatalog catalog)
        {
            var list = new List<(string, string, int, string)>();
            foreach (var table in catalog.Tables)
            {
                foreach (var entry in table.entries)
                {
                    if (entry.item_id.StartsWith("item_collectible_", StringComparison.Ordinal))
                        list.Add((entry.item_id, table.id, entry.weight, entry.rarity_tier));
                }
            }
            return list;
        }

        private static string FindRepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir, "Assets", "StreamingAssets", "Data", "scavenging_tables.json")))
                    return dir;
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("repo root with data authority not found");
        }

        // ── Required plan-exact placements ─────────────────────────────

        public static IEnumerable<object[]> RequiredPlacements => new List<object[]>
        {
            new object[] { "item_collectible_family_portrait", "table_loot_apartment_block", 8, "common" },
            new object[] { "item_collectible_unit_photograph", "table_loot_military_depot", 5, "uncommon" },
            new object[] { "item_collectible_civil_defense_poster", "table_loot_fire_station", 6, "common" },
            new object[] { "item_collectible_propaganda_poster", "table_loot_police_station", 4, "uncommon" },
            new object[] { "item_collectible_concert_poster", "table_loot_school", 5, "common" },
            new object[] { "item_collectible_field_medicine_handbook", "table_loot_clinic", 4, "uncommon" },
            new object[] { "item_collectible_pre_war_novel", "table_loot_apartment_block", 6, "common" },
            new object[] { "item_collectible_diesel_service_manual", "table_loot_industrial_district", 3, "rare" },
            new object[] { "item_collectible_radio_repair_guide", "table_loot_metro_station", 3, "rare" },
            new object[] { "item_collectible_road_map", "table_loot_police_station", 3, "uncommon" },
            new object[] { "item_collectible_exchange_day_newspaper", "table_loot_school", 2, "rare" }
        };

        [Theory]
        [MemberData(nameof(RequiredPlacements))]
        public void RequiredPlacement_ExistsWithExactWeightAndRarity(
            string itemId, string tableId, int weight, string rarity)
        {
            var catalog = LoadTables();
            Assert.True(catalog.TryGetTable(tableId, out var table), $"table {tableId} must exist");
            var entry = table!.entries.FirstOrDefault(e => e.item_id == itemId);
            Assert.NotNull(entry);
            Assert.Equal(weight, entry!.weight);
            Assert.Equal(rarity, entry.rarity_tier);
            Assert.True(entry.weight > 0, "placement weight must be positive");
            Assert.Equal(1, entry.min_quantity);
        }

        [Fact]
        public void PlacedItemIds_AllResolve_AgainstCollectibleCatalog()
        {
            var collectibles = LoadCollectibles();
            var catalog = LoadTables();
            var broken = LoadCollectibleEntries(catalog)
                .Where(e => collectibles.GetByItemId(e.ItemId) == null)
                .Select(e => $"{e.ItemId}@{e.TableId}")
                .ToList();
            Assert.Empty(broken);
        }

        [Fact]
        public void AtLeastFifteenLivePlacements_ExistOnBoundTables()
        {
            var bound = LoadBoundTables();
            var catalog = LoadTables();
            int live = LoadCollectibleEntries(catalog).Count(e => bound.Contains(e.TableId));
            Assert.True(live >= 15, $"expected at least 15 live collectible placements, found {live}");
        }

        [Fact]
        public void AdditionalPlacements_UseRealCatalogIds_AtLeastFourMore()
        {
            var required = new HashSet<string>(StringComparer.Ordinal);
            foreach (var row in RequiredPlacements)
                required.Add((string)row[0]);

            var collectibles = LoadCollectibles();
            var catalog = LoadTables();
            var additional = LoadCollectibleEntries(catalog)
                .Select(e => e.ItemId)
                .Where(id => !required.Contains(id))
                .Distinct()
                .ToList();

            Assert.True(additional.Count >= 4, $"expected at least 4 additional collectible placements, found {additional.Count}");
            foreach (string id in additional)
                Assert.NotNull(collectibles.GetByItemId(id));
        }

        [Fact]
        public void UniqueCollectibles_PlacedWithoutTableLevelUniqueMetadata()
        {
            // Uniqueness is definition-level; table rarity_tier stays in the
            // common/uncommon/rare vocabulary so no contradictory metadata.
            var collectibles = LoadCollectibles();
            var catalog = LoadTables();
            foreach (var entry in LoadCollectibleEntries(catalog))
            {
                var def = collectibles.GetByItemId(entry.ItemId);
                Assert.NotNull(def);
                Assert.NotEqual("unique", entry.Rarity); // tier vocabulary stays finite
            }

            // All three authored uniques must actually be placed.
            var placed = LoadCollectibleEntries(LoadTables()).Select(e => e.ItemId).ToHashSet(StringComparer.Ordinal);
            foreach (var kv in collectibles.ByItemId)
            {
                if (kv.Value.unique)
                    Assert.True(placed.Contains(kv.Key), $"unique collectible {kv.Key} must be placed");
            }
        }

        [Fact]
        public void NoDuplicateContradictoryEntries_AcrossTables()
        {
            var catalog = LoadTables();
            var seen = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            foreach (var entry in LoadCollectibleEntries(catalog))
            {
                if (!seen.TryGetValue(entry.ItemId, out var tables))
                    seen[entry.ItemId] = tables = new List<string>();
                tables.Add(entry.TableId);
            }
            // A collectible may appear in multiple tables (primary + secondary
            // provenance), but never twice in the SAME table.
            foreach (var kv in seen)
            {
                Assert.True(kv.Value.Distinct().Count() == kv.Value.Count,
                    $"{kv.Key} duplicated within a single table: {string.Join(",", kv.Value)}");
            }
        }

        [Fact]
        public void ChangedTables_StillYieldOrdinaryLoot()
        {
            var catalog = LoadTables();
            var bound = LoadBoundTables();
            var touched = catalog.Tables
                .Where(t => t.entries.Any(e => e.item_id.StartsWith("item_collectible_", StringComparison.Ordinal)))
                .ToList();
                Assert.True(touched.Count >= 10, $"expected at least 10 touched tables, found {touched.Count}");

            foreach (var table in touched)
            {
                int total = table.entries.Where(e => e.weight > 0).Sum(e => e.weight);
                int collectible = table.entries
                    .Where(e => e.weight > 0 && e.item_id.StartsWith("item_collectible_", StringComparison.Ordinal))
                    .Sum(e => e.weight);
                double share = (double)collectible / total;
                Assert.True(share <= 0.12,
                    $"{table.id}: collectible weight share {share:P1} would crowd out survival loot");
                Assert.True(total - collectible > 0, $"{table.id} must retain ordinary loot entries");
            }
        }

        [Fact]
        public void DeterministicSampling_SameSeedYieldsIdenticalSequence()
        {
            var catalog = LoadTables();
            var rngA = new SeededRng(20260903);
            var rngB = new SeededRng(20260903);
            string tableId = "table_loot_apartment_block";

            for (int i = 0; i < 500; i++)
            {
                var a = catalog.RollLoot(tableId, rngA);
                var b = catalog.RollLoot(tableId, rngB);
                Assert.Equal(a!.ItemId, b!.ItemId);
                Assert.Equal(a.Quantity, b.Quantity);
            }
        }

        [Fact]
        public void DeterministicSampling_LiveDistributionTracksWeights()
        {
            var catalog = LoadTables();
            var rng = new SeededRng(20260903);
            const int rolls = 10000;
            string tableId = "table_loot_apartment_block";
            catalog.TryGetTable(tableId, out var table);

            int collectibleHits = 0;
            for (int i = 0; i < rolls; i++)
            {
                var roll = catalog.RollLoot(tableId, rng);
                if (roll != null && roll.ItemId.StartsWith("item_collectible_", StringComparison.Ordinal))
                    collectibleHits++;
            }

            int expected = table!.entries
                .Where(e => e.weight > 0 && e.item_id.StartsWith("item_collectible_", StringComparison.Ordinal))
                .Sum(e => e.weight);
            int total = table.entries.Where(e => e.weight > 0).Sum(e => e.weight);
            double expectedRate = (double)expected / total;

            Assert.InRange(collectibleHits / (double)rolls, expectedRate - 0.02, expectedRate + 0.02);
        }

        // ── Unique suppression through the loot roll (Task 4 mechanics) ──

        [Fact]
        public void RollLoot_ClaimedUnique_IsNeverSelected()
        {
            var catalog = LoadTables();
            var rng = new SeededRng(4242);
            const string uniqueId = "item_collectible_exchange_day_newspaper";

            for (int i = 0; i < 2000; i++)
            {
                var roll = catalog.RollLoot("table_loot_school", rng,
                    id => !string.Equals(id, uniqueId, StringComparison.Ordinal));
                Assert.NotNull(roll);
                Assert.NotEqual(uniqueId, roll!.ItemId);
            }
        }

        [Fact]
        public void RollLoot_AllUniquesClaimed_PoolStillTerminatesAndYieldsOrdinaryLoot()
        {
            var catalog = LoadTables();
            var claimed = new HashSet<string>(StringComparer.Ordinal)
            {
                "item_collectible_exchange_day_newspaper" // the only collectible in table_loot_school
            };
            var rng = new SeededRng(777);
            int ordinary = 0;
            for (int i = 0; i < 1000; i++)
            {
                var roll = catalog.RollLoot("table_loot_school", rng, id => !claimed.Contains(id));
                Assert.NotNull(roll); // clean termination — no recursion, no null starvation
                Assert.NotEqual("item_collectible_exchange_day_newspaper", roll!.ItemId);
                ordinary++;
            }
            Assert.Equal(1000, ordinary);
        }

        [Fact]
        public void RollLoot_NonUniqueItems_ContinueNormally_WithFilter()
        {
            var catalog = LoadTables();
            var rngA = new SeededRng(31337);
            var rngB = new SeededRng(31337);
            string tableId = "table_loot_industrial_district";
            for (int i = 0; i < 500; i++)
            {
                var unfiltered = catalog.RollLoot(tableId, rngA);
                var filtered = catalog.RollLoot(tableId, rngB, id => true);
                Assert.Equal(unfiltered!.ItemId, filtered!.ItemId);
                Assert.Equal(unfiltered.Quantity, filtered.Quantity);
            }
        }

        [Fact]
        public void RollLoot_ProvenancePlacements_AreReachableThroughLiveTables()
        {
            // The provenance doc's contract: every placed collectible is
            // reachable either directly (bound primary) or via a bound
            // secondary table.
            var bound = LoadBoundTables();
            var catalog = LoadTables();
            var unreachable = LoadCollectibleEntries(catalog)
                .GroupBy(e => e.ItemId)
                .Where(g => !g.Any(e => bound.Contains(e.TableId)))
                .Select(g => g.Key)
                .ToList();
            Assert.Empty(unreachable);
        }
    }
}
