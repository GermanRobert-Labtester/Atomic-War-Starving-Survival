// SPDX-License-Identifier: MIT
// Plan 09 / 9B Content — detox-support items, buprenorphine taper pharma
// recipe, dependency-kind survivor backstories. Pure data delivery; no Core
// extensions, no schema bump. The OpenStressReported API (Plan 09 9B Core,
// commit 95c41b5d) is the consumption side; this commit is the content side.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class DependencyDetoxContentTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        // The four canonical detox-support items.

        public static IEnumerable<object[]> NewDetoxItemRows()
        {
            yield return new object[] { "item_taper_kit_opioid",    "opioid"   };
            yield return new object[] { "item_stabilization_tea",   "sedative" };
            yield return new object[] { "item_thiamine_dose",       "alcohol"  };
            yield return new object[] { "item_substitution_dose",   "opioid"   };
        }

        [Theory]
        [MemberData(nameof(NewDetoxItemRows))]
        public void DetoxItem_Exists_InItemsJson_WithClassHintInProse(string itemId, string className)
        {
            using var doc = LoadJson("items.json");
            JsonElement items = JsonExt.GetItemsArray(doc.RootElement);

            Assert.True(JsonExt.FindBy(items, "id", itemId, out var entry),
                itemId + " missing from items.json (Plan 09 9B Content)");

            Assert.Equal("Medical", JsonExt.Str(entry, "type"));

            string desc = JsonExt.Str(entry, "description");
            Assert.True(desc.Length >= 180,
                itemId + " description too short to house a 9B-style voice (" +
                desc.Length + " chars)");
            Assert.True(desc.IndexOf(className, StringComparison.OrdinalIgnoreCase) >= 0,
                itemId + " description should mention its dependency class (" +
                className + ") for player discoverability");
        }

        [Fact]
        public void DetoxItem_ReuseSchema_HasAll17StandardKeys()
        {
            string[] required =
            {
                "id", "displayName", "description", "type", "stackMax", "weight",
                "radProtection", "durability", "contamination", "hungerRestore",
                "thirstRestore", "healthEffect", "radCleanse", "moraleEffect",
                "isEquipable", "equipSlot", "tradeValue", "empShielded",
            };

            using var doc = LoadJson("items.json");
            JsonElement items = JsonExt.GetItemsArray(doc.RootElement);
            foreach (var row in NewDetoxItemRows())
            {
                var itemId = (string)row[0];
                Assert.True(JsonExt.FindBy(items, "id", itemId, out var entry), itemId + " missing");
                foreach (var key in required)
                    Assert.True(entry.TryGetProperty(key, out _),
                        itemId + " missing key '" + key + "'");
            }
        }

        [Fact]
        public void Recipe_BuprenorphineTaperKit_OutputsDetoxItem_WithSafeRisk()
        {
            const string recipeId = "recipe_buprenorphine_taper_kit";
            using var doc = LoadJson("pharma_recipes.json");
            JsonElement recipes = JsonExt.GetArrayNamed(doc.RootElement, "recipes");
            Assert.True(JsonExt.FindBy(recipes, "recipe_id", recipeId, out var recipe),
                recipeId + " missing in pharma_recipes.json");

            Assert.Equal("item_taper_kit_opioid", JsonExt.Str(recipe, "output_item_id"));
            Assert.Equal("antagonist", JsonExt.Str(recipe, "category"));
            Assert.True(JsonExt.Dbl(recipe, "dependency_risk") < 0.10,
                "taper kit recipe must carry sub-0.10 dependency_risk — " +
                "antagonists are explicitly designed to suppress relapse, not amplify it");
            Assert.Equal("pharma_bench", JsonExt.Str(recipe, "required_station"));

            using var itemsDoc = LoadJson("items.json");
            JsonElement itemsList = JsonExt.GetItemsArray(itemsDoc.RootElement);
            HashSet<string> existingIds = new HashSet<string>(
                itemsList.EnumerateArray().Select(e => JsonExt.Str(e, "id")));
            foreach (var inputId in recipe.GetProperty("input_ids").EnumerateArray()
                .Select(e => e.GetString()))
                Assert.True(existingIds.Contains(inputId),
                    recipeId + " references missing input '" + inputId + "'");
        }

        // ── Survivor backstories ───────────────────────────────────

        [Fact]
        public void BackstoriesCollection_LoadsAndListsSixSurvivors()
        {
            using var col = LoadJson("narrative/dweller_dependency_backstories.json");
            Assert.Equal("dweller_dependency_backstories",
                JsonExt.Str(col.RootElement, "collection_id"));
            JsonElement arr = JsonExt.GetArrayNamed(col.RootElement, "backstories");
            Assert.Equal(6, arr.GetArrayLength());

            HashSet<string> allowedKinds = new HashSet<string>(StringComparer.Ordinal)
            {
                "Opioid", "Alcohol", "Stimulant", "Sedative",
            };
            int proseFloorCount = 0;
            foreach (var s in arr.EnumerateArray())
            {
                Assert.True(s.TryGetProperty("backstory_id", out _),
                    "backstory missing backstory_id");
                Assert.True(s.TryGetProperty("kind", out _), "backstory missing kind");
                Assert.True(s.TryGetProperty("prose", out _), "backstory missing prose");
                string kind = JsonExt.Str(s, "kind");
                Assert.True(allowedKinds.Contains(kind),
                    "backstory '" + JsonExt.Str(s, "backstory_id") +
                    "' declared unknown kind '" + kind + "'");
                string prose = JsonExt.Str(s, "prose");
                Assert.True(prose.Length >= 400,
                    "backstory '" + JsonExt.Str(s, "backstory_id") +
                    "' prose too short to be a real survivor voice (" +
                    prose.Length + " chars)");
                if (prose.Length >= 400) proseFloorCount++;
            }
            Assert.Equal(6, proseFloorCount);

            foreach (string kind in allowedKinds)
            {
                Assert.True(arr.EnumerateArray().Any(s => JsonExt.Str(s, "kind") == kind),
                    "0 backstories for kind='" + kind + "'");
            }
        }

        [Fact]
        public void Backstory_AuthoredFor_BothMaintenanceAndTaperMoralTextures()
        {
            using var col = LoadJson("narrative/dweller_dependency_backstories.json");
            JsonElement arr = JsonExt.GetArrayNamed(col.RootElement, "backstories");
            int maintenanceCount = 0, taperCount = 0;
            foreach (var s in arr.EnumerateArray())
            {
                string lo = JsonExt.Str(s, "prose").ToLowerInvariant();
                if (lo.Contains("maintenance") || lo.Contains("leash")) maintenanceCount++;
                if (lo.Contains("taper") || lo.Contains("dose-down") ||
                    lo.Contains("drop a ") || lo.Contains("drop the last")) taperCount++;
            }
            Assert.True(maintenanceCount >= 1,
                "Plan 09 9B requires at least one maintenance voice — none found");
            Assert.True(taperCount >= 1,
                "Plan 09 9B requires at least one taper voice — none found");
        }

        // ── Helpers ────────────────────────────────────────────────

        private static JsonDocument LoadJson(string relativePath)
        {
            string path = Path.Combine(DataDir(), relativePath);
            Assert.True(File.Exists(path), relativePath + " not located at " + path);
            return JsonDocument.Parse(File.ReadAllText(path));
        }
    }

    /// <summary>Test-local JSON facade — fluent getter helpers over <see cref="JsonElement"/>.</summary>
    internal static class JsonExt
    {
        public static JsonElement GetItemsArray(JsonElement root)
            => root.ValueKind == JsonValueKind.Object && root.TryGetProperty("items", out var arr)
                ? arr
                : root;

        public static JsonElement GetArrayNamed(JsonElement root, string name)
        {
            if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty(name, out var arr))
                throw new Xunit.Sdk.XunitException($"root lacks '{name}' array");
            return arr;
        }

        public static bool FindBy(JsonElement array, string idField, string id, out JsonElement entry)
        {
            entry = default;
            if (array.ValueKind != JsonValueKind.Array) return false;
            foreach (var e in array.EnumerateArray())
            {
                if (e.ValueKind != JsonValueKind.Object) continue;
                if (e.TryGetProperty(idField, out var idEl) &&
                    string.Equals(idEl.GetString(), id, StringComparison.Ordinal))
                {
                    entry = e;
                    return true;
                }
            }
            return false;
        }

        public static string Str(JsonElement e, string name, string fallback = "")
        {
            if (e.ValueKind != JsonValueKind.Object) return fallback;
            foreach (var p in e.EnumerateObject())
            {
                if (!string.Equals(p.Name, name, StringComparison.Ordinal)) continue;
                return p.Value.ValueKind == JsonValueKind.String
                    ? p.Value.GetString() ?? fallback
                    : p.Value.GetRawText();
            }
            return fallback;
        }

        public static double Dbl(JsonElement e, string name, double fallback = 0)
        {
            if (e.ValueKind != JsonValueKind.Object) return fallback;
            foreach (var p in e.EnumerateObject())
            {
                if (!string.Equals(p.Name, name, StringComparison.Ordinal)) continue;
                if (p.Value.ValueKind == JsonValueKind.Number) return p.Value.GetDouble();
                if (double.TryParse(p.Value.GetRawText(),
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double d)) return d;
            }
            return fallback;
        }
    }
}
