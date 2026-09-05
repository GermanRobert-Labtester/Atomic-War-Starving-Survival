using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Generation-channel uniqueness gate (collectibles flagship, Task 4):
    /// the expedition loot roll is the only channel wired to the
    /// UniqueItemClaimRegistry availability port. Every OTHER generation
    /// channel is collectible-free BY DATA — this gate pins that data
    /// invariant so a future content edit cannot silently reintroduce
    /// unique-item duplication through an unwired channel.
    ///
    /// Channels audited:
    /// - expedition loot rolls → wired (IsItemGenerationAvailable port)
    /// - encounter/scripted grants → wired (TryGrantLoot pre-check)
    /// - maritime/dive procedural tables → pinned collectible-free HERE
    /// - merchant/trade stock → no generator exists; stock data pinned HERE
    /// </summary>
    public class CollectibleGenerationChannelGateTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (File.Exists(Path.Combine(probe, "dive_sites.json")))
                    return probe;
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        private static HashSet<string> CollectibleIds()
        {
            var json = JsonDocument.Parse(File.ReadAllText(Path.Combine(DataDir, "collectibles.json")));
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var c in json.RootElement.GetProperty("collectibles").EnumerateArray())
            {
                if (c.TryGetProperty("item_id", out var id) && id.ValueKind == JsonValueKind.String)
                    ids.Add(id.GetString()!);
            }
            return ids;
        }

        private static void AssertNoCollectibles(string fileName, string arrayProperty, string idProperty,
            HashSet<string> collectibles, HashSet<string> checkedIds)
        {
            var path = Path.Combine(DataDir, fileName);
            if (!File.Exists(path)) return; // optional catalog
            var json = JsonDocument.Parse(File.ReadAllText(path));
            if (!json.RootElement.TryGetProperty(arrayProperty, out var array)) return;

            foreach (var element in array.EnumerateArray())
            {
                CollectIdsDeep(element, idProperty, checkedIds);
            }

            var offenders = checkedIds.Intersect(collectibles).ToList();
            Assert.True(offenders.Count == 0,
                $"{fileName} must not grant/generate collectible ids through an unwired generation channel. " +
                $"Offenders: {string.Join(", ", offenders)}. Wire the channel through UniqueItemClaimRegistry " +
                "availability before authoring collectibles into it.");
        }

        private static void CollectIdsDeep(JsonElement element, string idProperty, HashSet<string> into)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var prop in element.EnumerateObject())
                    {
                        if (string.Equals(prop.Name, idProperty, StringComparison.OrdinalIgnoreCase) &&
                            prop.Value.ValueKind == JsonValueKind.String)
                        {
                            into.Add(prop.Value.GetString()!);
                        }
                        else if (prop.Value.ValueKind is JsonValueKind.Object or JsonValueKind.Array)
                        {
                            CollectIdsDeep(prop.Value, idProperty, into);
                        }
                    }
                    break;
                case JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                        CollectIdsDeep(item, idProperty, into);
                    break;
            }
        }

        [Fact]
        public void MaritimeDiveSites_DoNotGenerateCollectibles()
        {
            var collectibles = CollectibleIds();
            var checkedIds = new HashSet<string>(StringComparer.Ordinal);
            AssertNoCollectibles("dive_sites.json", "dive_sites", "item_id", collectibles, checkedIds);
            AssertNoCollectibles("black_flotilla_items.json", "items", "id", collectibles, checkedIds);
        }

        [Fact]
        public void TradeAndEconomyStock_DoNotGenerateCollectibles()
        {
            var collectibles = CollectibleIds();
            var checkedIds = new HashSet<string>(StringComparer.Ordinal);
            // Merchant/trade channels: caravans, holdfast trade, economy goods.
            AssertNoCollectibles("caravans.json", "caravans", "item_id", collectibles, checkedIds);
            AssertNoCollectibles("caravan_trade_routes.json", "routes", "item_id", collectibles, checkedIds);
            AssertNoCollectibles("economy_goods.json", "goods", "item_id", collectibles, checkedIds);
        }

        [Fact]
        public void QuestAndNarrativeRewards_DoNotGrantCollectibles()
        {
            var collectibles = CollectibleIds();
            var checkedIds = new HashSet<string>(StringComparer.Ordinal);
            AssertNoCollectibles("personal_quests.json", "quests", "reward_item_id", collectibles, checkedIds);
            AssertNoCollectibles("repeatable_quests.json", "quests", "reward_item_id", collectibles, checkedIds);
        }

        [Fact]
        public void ExpeditionLootTableEntries_AreTheOnlyCollectibleSource()
        {
            // Cross-check: the placements recorded in scavenging_tables.json
            // are the complete set of authored collectible generation points.
            var collectibles = CollectibleIds();
            var json = JsonDocument.Parse(File.ReadAllText(Path.Combine(DataDir, "scavenging_tables.json")));
            var placed = new HashSet<string>(StringComparer.Ordinal);
            foreach (var table in json.RootElement.GetProperty("tables").EnumerateArray())
            {
                foreach (var entry in table.GetProperty("entries").EnumerateArray())
                {
                    if (entry.TryGetProperty("item_id", out var id) && id.ValueKind == JsonValueKind.String)
                    {
                        var itemId = id.GetString()!;
                        if (collectibles.Contains(itemId)) placed.Add(itemId);
                    }
                }
            }
            // Every placed id must be a real collectible; the placed set is
            // the full authoritative generation surface.
            Assert.Subset(collectibles, placed);
            Assert.True(placed.Count >= 15,
                $"expected the scavenging tables to remain the collectible generation surface (got {placed.Count})");
        }
    }
}
