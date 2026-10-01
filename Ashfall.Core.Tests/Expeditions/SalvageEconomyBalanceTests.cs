// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Salvage economy telemetry (W2) — analytic expected-value banding for the
    /// five authored wasteland-salvage tables against their closest authored
    /// analogs. Fully deterministic: no rolls, exact weight-share arithmetic
    /// over the authored catalogs (scavenging_tables.json + *items*.json).
    ///
    /// Bands protect the tier promise the map makes to the player: a "rare"
    /// site pays like a restricted zone, a weapons site like a military depot,
    /// a caravanserai like a convoy cache — and never inverted.
    /// </summary>
    public sealed class SalvageEconomyBalanceTests
    {
        private static string DataDir()
        {
            var dir = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (!Directory.Exists(dir))
                dir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            return dir;
        }

        private sealed class ItemDto
        {
            public string id { get; set; } = string.Empty;
            public float tradeValue { get; set; }
            public float weight { get; set; }
        }

        private sealed class ItemCatalogDto
        {
            public List<ItemDto>? items { get; set; }
        }

        private static Dictionary<string, (float TradeValue, float Weight)> LoadItems()
        {
            // The canonical value source: items.json carries tradeValue and
            // weight for every physical item the salvage tables roll.
            var raw = File.ReadAllText(Path.Combine(DataDir(), "items.json"));
            var catalog = new SystemTextJsonSerializer().Deserialize<ItemCatalogDto>(raw);
            Assert.NotNull(catalog?.items);
            var map = new Dictionary<string, (float, float)>(StringComparer.Ordinal);
            foreach (var item in catalog!.items!)
            {
                if (item != null && !string.IsNullOrEmpty(item.id))
                    map[item.id] = (item.tradeValue, item.weight);
            }
            Assert.True(map.Count > 0, "items.json parsed empty");
            return map;
        }

        /// <summary>Analytic expected barter value of one roll on a table.</summary>
        private static float ExpectedValue(ScavengingTableDef table, Dictionary<string, (float TradeValue, float Weight)> items)
        {
            int totalWeight = table.entries.Sum(e => Math.Max(0, e.weight));
            Assert.True(totalWeight > 0, $"table {table.id} has no positive weights");
            double ev = 0.0;
            foreach (var e in table.entries)
            {
                if (string.IsNullOrEmpty(e.item_id)) continue; // codex/map-fragment-only entries carry no physical item
                Assert.True(items.ContainsKey(e.item_id),
                    $"table {table.id} entry '{e.item_id}' does not resolve in the merged item catalogs");
                double avgQty = (e.min_quantity + e.max_quantity) / 2.0;
                ev += (e.weight / (double)totalWeight) * avgQty * items[e.item_id].TradeValue;
            }
            return (float)ev;
        }

        private static ScavengingTableDef Table(string id)
        {
            var catalog = ScavengingTableCatalog.LoadFromJson(
                File.ReadAllText(Path.Combine(DataDir(), "scavenging_tables.json")),
                new SystemTextJsonSerializer());
            Assert.True(catalog.TryGetTable(id, out var t), $"table '{id}' missing from Plan 46 authority");
            return t;
        }

        private static string Telemetry(Dictionary<string, float> ev)
            => string.Join("\n", ev.OrderBy(k => k.Value).Select(k => $"  {k.Key,-36} EV/roll = {k.Value,8:F2} barter"));

        [Fact]
        public void SalvageTiers_PayInMonotonicOrder()
        {
            var items = LoadItems();
            var ev = new Dictionary<string, float>
            {
                ["salvage_common"] = ExpectedValue(Table("salvage_common"), items),
                ["salvage_electronic"] = ExpectedValue(Table("salvage_electronic"), items),
                ["trade_goods"] = ExpectedValue(Table("trade_goods"), items),
                ["salvage_rare"] = ExpectedValue(Table("salvage_rare"), items),
                ["salvage_weapons"] = ExpectedValue(Table("salvage_weapons"), items),
            };

            // Common structural < electronics < caravanserai stock < restricted deep salvage < munitions.
            Assert.True(ev["salvage_common"] < ev["salvage_electronic"],
                "tier inversion: electronics must outpay common structural salvage.\n" + Telemetry(ev));
            Assert.True(ev["salvage_electronic"] < ev["trade_goods"],
                "tier inversion: caravanserai stock must outpay field electronics.\n" + Telemetry(ev));
            Assert.True(ev["trade_goods"] < ev["salvage_rare"],
                "tier inversion: restricted deep salvage must outpay trade stock.\n" + Telemetry(ev));
            Assert.True(ev["salvage_rare"] < ev["salvage_weapons"],
                "tier inversion: munitions sites must outpay deep salvage.\n" + Telemetry(ev));
        }

        [Theory]
        [InlineData("salvage_common", "table_loot_collapsed_structure", 0.5f, 2.0f)]
        [InlineData("salvage_weapons", "table_loot_military_depot", 0.75f, 2.0f)]
        [InlineData("trade_goods", "table_loot_convoy_cache", 0.6f, 2.0f)]
        public void SalvageTables_PayLikeTheirAuthoredAnalogs(string authored, string analog, float minRatio, float maxRatio)
        {
            var items = LoadItems();
            float a = ExpectedValue(Table(authored), items);
            float b = ExpectedValue(Table(analog), items);
            Assert.True(b > 0f, $"analog table '{analog}' computed zero value");
            float ratio = a / b;
            Assert.True(ratio >= minRatio && ratio <= maxRatio,
                $"'{authored}' pays {a:F2}/roll vs analog '{analog}' at {b:F2}/roll (ratio {ratio:F2}); " +
                $"band [{minRatio}, {maxRatio}] keeps the map's reward promise honest.");
        }

        [Fact]
        public void HazardTracksReward_AcrossSalvageTiers()
        {
            var common = Table("salvage_common");
            var rare = Table("salvage_rare");
            var weapons = Table("salvage_weapons");
            // The two richest tables must be at least as hazardous as field salvage.
            Assert.True(rare.base_hazard_chance >= common.base_hazard_chance,
                "restricted-zone salvage must not be safer than common field salvage");
            Assert.True(weapons.base_hazard_chance >= common.base_hazard_chance,
                "munitions salvage must not be safer than common field salvage");
        }
    }
}
