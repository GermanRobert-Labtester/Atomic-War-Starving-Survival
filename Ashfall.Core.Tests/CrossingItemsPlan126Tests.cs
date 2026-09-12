// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 126 — Crossing item catalog expansion. The tests keep the original
    /// eleven definitions stable while proving the fourteen additions remain
    /// valid in both the local Crossing catalog and the merged global registry.
    /// </summary>
    public sealed class CrossingItemsPlan126Tests
    {
        private static readonly string[] OriginalIds =
        {
            "item_vouch_token_crossing",
            "item_calibration_weight",
            "item_crossing_traded_grain",
            "item_crossing_traded_salt",
            "item_crossing_pledge_slip",
            "item_charter_three_pages",
            "item_debt_contract_copy",
            "item_marker_rubbing",
            "item_duty_log_fragment",
            "item_trade_manifest_blank",
            "item_wyn_receipt_paid"
        };

        private static readonly string[] NewIds =
        {
            "item_arbitration_token",
            "item_charter_stamp",
            "item_weighbridge_chit",
            "item_smuggled_medicine",
            "item_crossing_bread",
            "item_lamp_oil_crossing",
            "item_filtered_water_crossing",
            "item_quarantine_bands",
            "item_granary_receipt",
            "item_smugglers_ledger",
            "item_rejection_notice",
            "item_crossing_map",
            "item_black_market_pouch",
            "item_charter_draft"
        };

        private static readonly HashSet<string> AcceptedTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Food", "Water", "IrradiatedWater", "Medical", "AntiRad", "Iodine",
            "Protective", "Tool", "Fuel", "Filter", "Material", "Trade", "Comfort",
            "Quest", "Device", "Weapon", "Corpse", "ContaminatedFood", "Relic"
        };

        private static string ResolveDataDir()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dataDir = Path.GetFullPath(Path.Combine(baseDir, "../../../../Assets/StreamingAssets/Data"));
            if (!Directory.Exists(dataDir))
                dataDir = Path.GetFullPath(Path.Combine(baseDir, "../../../Assets/StreamingAssets/Data"));
            return dataDir;
        }

        private static JsonElement LoadRoot(string file)
        {
            using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(ResolveDataDir(), file)));
            return document.RootElement.Clone();
        }

        private static JsonElement LoadItemsArray()
        {
            var root = LoadRoot("crossing_items.json");
            Assert.Equal(JsonValueKind.Object, root.ValueKind);
            return root.GetProperty("items");
        }

        private static JsonElement FindEntry(string id)
        {
            foreach (var entry in LoadItemsArray().EnumerateArray())
            {
                if (entry.GetProperty("id").GetString() == id)
                    return entry;
            }

            throw new Xunit.Sdk.XunitException($"Crossing item '{id}' is missing");
        }

        private static ItemCatalog LoadGlobalCatalog()
        {
            return ItemCatalogLoader.LoadCatalog(
                ResolveDataDir(),
                new FileSystemIO(),
                new SystemTextJsonSerializer());
        }

        [Fact]
        public void CrossingCatalog_ContainsExactlyTwentyFiveItems()
        {
            Assert.Equal(25, LoadItemsArray().GetArrayLength());

            var loader = new CrossingCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(ResolveDataDir());
            Assert.Equal(25, catalog.Items.Count);
        }

        [Fact]
        public void CrossingCatalog_PreservesOriginalElevenAndAddsExactlyFourteen()
        {
            var ids = LoadItemsArray()
                .EnumerateArray()
                .Select(e => e.GetProperty("id").GetString()!)
                .ToList();

            Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
            Assert.Equal(11, OriginalIds.Intersect(ids, StringComparer.Ordinal).Count());
            Assert.Equal(14, NewIds.Intersect(ids, StringComparer.Ordinal).Count());
            Assert.Equal(25, OriginalIds.Concat(NewIds).Intersect(ids, StringComparer.Ordinal).Count());
        }

        [Fact]
        public void CrossingCatalog_AllEntriesUseSupportedTypesAndNumericRanges()
        {
            foreach (var entry in LoadItemsArray().EnumerateArray())
            {
                string id = entry.GetProperty("id").GetString()!;
                string type = entry.GetProperty("type").GetString()!;
                Assert.Contains(type, AcceptedTypes);
                Assert.True(entry.GetProperty("stackMax").GetInt32() >= 1, $"{id} stackMax must be positive");
                Assert.True(float.IsFinite(entry.GetProperty("weight").GetSingle()), $"{id} weight must be finite");
                Assert.True(entry.GetProperty("weight").GetSingle() >= 0f, $"{id} weight must be non-negative");
                Assert.True(float.IsFinite(entry.GetProperty("tradeValue").GetSingle()), $"{id} tradeValue must be finite");
                Assert.True(entry.GetProperty("tradeValue").GetSingle() >= 0f, $"{id} tradeValue must be non-negative");
                Assert.False(string.IsNullOrWhiteSpace(entry.GetProperty("displayName").GetString()), $"{id} display name missing");
                Assert.False(string.IsNullOrWhiteSpace(entry.GetProperty("description").GetString()), $"{id} description missing");
            }
        }

        [Fact]
        public void CrossingCatalog_OriginalNumericDefinitionsRemainUnchanged()
        {
            var expected = new Dictionary<string, (string type, int stack, float weight, float value, float thirst, float hunger, float morale)>
            {
                ["item_vouch_token_crossing"] = ("Quest", 1, 0.1f, 50f, 0f, 0f, 0f),
                ["item_calibration_weight"] = ("Tool", 1, 2f, 80f, 0f, 0f, 0f),
                ["item_crossing_traded_grain"] = ("Trade", 10, 12f, 30f, 0f, 0f, 0f),
                ["item_crossing_traded_salt"] = ("Trade", 8, 3f, 22f, 0f, 0f, 0f),
                ["item_crossing_pledge_slip"] = ("Quest", 1, 0.1f, 5f, 0f, 0f, 0f),
                ["item_charter_three_pages"] = ("Quest", 1, 0.1f, 100f, 0f, 0f, 10f),
                ["item_debt_contract_copy"] = ("Quest", 1, 0.1f, 10f, 0f, 0f, 0f),
                ["item_marker_rubbing"] = ("Quest", 1, 0.1f, 15f, 0f, 0f, 0f),
                ["item_duty_log_fragment"] = ("Quest", 1, 0.1f, 25f, 0f, 0f, 0f),
                ["item_trade_manifest_blank"] = ("Tool", 5, 0.2f, 12f, 0f, 0f, 0f),
                ["item_wyn_receipt_paid"] = ("Quest", 1, 0.1f, 5f, 0f, 0f, 5f)
            };

            foreach (var id in OriginalIds)
            {
                var entry = FindEntry(id);
                var actual = (
                    entry.GetProperty("type").GetString()!,
                    entry.GetProperty("stackMax").GetInt32(),
                    entry.GetProperty("weight").GetSingle(),
                    entry.GetProperty("tradeValue").GetSingle(),
                    entry.GetProperty("thirstRestore").GetSingle(),
                    entry.GetProperty("hungerRestore").GetSingle(),
                    entry.GetProperty("moraleEffect").GetSingle());
                Assert.Equal(expected[id], actual);
            }
        }

        [Fact]
        public void GlobalCatalog_RegistersAllFourteenNewItems()
        {
            var catalog = LoadGlobalCatalog();
            foreach (string id in NewIds)
                Assert.True(catalog.Contains(id), $"global item registry missing '{id}'");
        }

        [Fact]
        public void GlobalCatalog_UsesCanonicalConsumableSemantics()
        {
            var catalog = LoadGlobalCatalog();

            var bread = catalog.Get("item_crossing_bread")!;
            Assert.Equal(ItemType.Food, bread.type);
            Assert.Equal(22f, bread.hungerRestore);
            Assert.Equal(0f, bread.thirstRestore);

            var water = catalog.Get("item_filtered_water_crossing")!;
            Assert.Equal(ItemType.Water, water.type);
            Assert.Equal(40f, water.thirstRestore);
            Assert.Equal(0f, water.hungerRestore);

            var medicine = catalog.Get("item_smuggled_medicine")!;
            Assert.Equal(ItemType.Medical, medicine.type);
            Assert.Equal(20f, medicine.healthEffect);
            Assert.Equal(0f, medicine.hungerRestore);
            Assert.Equal(0f, medicine.thirstRestore);
        }

        [Fact]
        public void ProposedIdsDoNotCollideAcrossGlobalItemFiles()
        {
            var occurrences = NewIds.ToDictionary(id => id, _ => 0, StringComparer.Ordinal);
            foreach (string file in Directory.GetFiles(ResolveDataDir(), "*items.json"))
            {
                using var document = JsonDocument.Parse(File.ReadAllText(file));
                if (!document.RootElement.TryGetProperty("items", out var items) || items.ValueKind != JsonValueKind.Array)
                    continue;
                foreach (var entry in items.EnumerateArray())
                {
                    string? id = entry.TryGetProperty("id", out var idElement) ? idElement.GetString() : null;
                    if (id != null && occurrences.ContainsKey(id))
                        occurrences[id]++;
                }
            }

            foreach (var pair in occurrences)
                Assert.Equal(1, pair.Value);
        }
    }
}
