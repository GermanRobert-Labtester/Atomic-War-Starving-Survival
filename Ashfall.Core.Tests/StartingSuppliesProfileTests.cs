using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class StartingSuppliesProfileTests
    {
        private static string DataDir()
        {
            string current = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate))
                    return candidate;

                string parent = Path.GetDirectoryName(current)!;
                if (parent == current) break;
                current = parent;
            }

            throw new DirectoryNotFoundException("Could not find Assets/StreamingAssets/Data.");
        }

        private static readonly Dictionary<string, int> LegacyBaseline =
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["clean_water"] = 12,
                ["canned_food"] = 16,
                ["irradiated_water"] = 4,
                ["item_air_filter_hepa"] = 2,
                ["item_desal_membrane"] = 1,
                ["iodine_pills"] = 4,
                ["bandage"] = 2,
                ["rad_away"] = 1,
                ["item_dosimeter_pen"] = 1,
                ["item_geiger_m3"] = 1,
                ["gas_mask"] = 1,
                ["hazmat_suit"] = 1,
                ["battery"] = 4,
                ["scrap_mechanical"] = 6,
                ["scrap_electronic"] = 3
            };

        [Fact]
        public void ProductionCatalog_HasSixProfiles_AndExactLegacyDefault()
        {
            string dataDir = DataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var itemCatalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);

            var loaded = ItemCatalogLoader.LoadStartingSuppliesCatalogDetailed(
                dataDir,
                fileIO,
                serializer,
                itemCatalog);

            Assert.True(loaded.IsUsable);
            Assert.False(loaded.UsedLegacyFallback);
            Assert.Empty(loaded.Errors);
            Assert.Equal(6, loaded.Catalog.Profiles.Count);
            Assert.Equal(
                StartingSuppliesCatalog.StandardProfileId,
                loaded.Catalog.DefaultProfileId);

            var standard = loaded.Catalog.DefaultProfile;
            Assert.Equal(LegacyBaseline, ToMap(standard));
            Assert.Equal(59, standard.supplies.Sum(s => s.amount));
        }

        [Fact]
        public void EveryProductionProfile_UsesCanonicalPositiveUniqueItems()
        {
            string dataDir = DataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var itemCatalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
            var catalog = ItemCatalogLoader.LoadStartingSuppliesCatalog(
                dataDir,
                fileIO,
                serializer,
                itemCatalog);

            foreach (var profile in catalog.Profiles)
            {
                Assert.StartsWith("origin_", profile.id, StringComparison.Ordinal);
                Assert.False(string.IsNullOrWhiteSpace(profile.display_name));
                Assert.False(string.IsNullOrWhiteSpace(profile.description));
                Assert.NotEmpty(profile.supplies);

                var ids = new HashSet<string>(StringComparer.Ordinal);
                foreach (var (itemId, amount) in profile.supplies)
                {
                    Assert.True(ids.Add(itemId), $"Duplicate {itemId} in {profile.id}");
                    Assert.True(amount > 0, $"{profile.id}/{itemId} has non-positive amount");
                    Assert.NotNull(itemCatalog.Get(itemId));
                }
            }
        }

        [Fact]
        public void LegacyFallback_MatchesTheLegacyProfileExactly()
        {
            var fallback = StartingSuppliesCatalog.CreateLegacyFallbackProfile();
            Assert.Equal(StartingSuppliesCatalog.StandardProfileId, fallback.id);
            Assert.Equal(LegacyBaseline, ToMap(fallback));
        }

        [Fact]
        public void ProfileMetrics_AreDistinctAndNoProfileDominatesAnother()
        {
            string dataDir = DataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var itemCatalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
            var catalog = ItemCatalogLoader.LoadStartingSuppliesCatalog(
                dataDir,
                fileIO,
                serializer,
                itemCatalog);
            var metrics = catalog.Profiles.ToDictionary(
                p => p.id,
                p => Calculate(p, itemCatalog),
                StringComparer.Ordinal);

            Assert.Equal(
                metrics.Count,
                metrics.Values.Select(m => m.Value).Distinct().Count());
            var standard = metrics[StartingSuppliesCatalog.StandardProfileId];
            foreach (var pair in metrics)
            {
                Assert.InRange(pair.Value.Value, standard.Value * 0.75f, standard.Value);
                Assert.False(
                    Dominates(pair.Value, standard) && pair.Key != StartingSuppliesCatalog.StandardProfileId,
                    $"{pair.Key} dominates Standard Holdfast.");
            }

            foreach (var left in metrics)
            foreach (var right in metrics)
            {
                if (string.Equals(left.Key, right.Key, StringComparison.Ordinal))
                    continue;
                Assert.False(
                    Dominates(left.Value, right.Value),
                    $"{left.Key} dominates {right.Key}.");
            }
        }

        [Fact]
        public void LegacyV1Shape_RemainsReadable()
        {
            var files = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["starting_supplies.json"] =
                    "{\"schema_version\":1,\"starting_supplies\":[" +
                    "{\"itemId\":\"clean_water\",\"amount\":2}," +
                    "{\"itemId\":\"canned_food\",\"amount\":3}]}"
            };
            var catalog = new ItemCatalog();
            catalog.Register(new ItemDefinition { id = "clean_water" });
            catalog.Register(new ItemDefinition { id = "canned_food" });

            var result = ItemCatalogLoader.LoadStartingSuppliesCatalogDetailed(
                "data",
                new DictionaryFileIO(files),
                new SystemTextJsonSerializer(),
                catalog);

            Assert.Empty(result.Errors);
            Assert.False(result.UsedLegacyFallback);
            Assert.Equal(
                new Dictionary<string, int>
                {
                    ["clean_water"] = 2,
                    ["canned_food"] = 3
                },
                ToMap(result.Catalog.DefaultProfile));
        }

        [Fact]
        public void InvalidAlternateProfile_IsSkippedWithoutRemovingStandard()
        {
            var files = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["starting_supplies.json"] =
                    "{\"schema_version\":2," +
                    "\"default_profile_id\":\"origin_standard_holdfast\"," +
                    "\"profiles\":[" +
                    "{\"id\":\"origin_standard_holdfast\",\"display_name\":\"Standard\"," +
                    "\"description\":\"Baseline\",\"supplies\":[{\"itemId\":\"clean_water\",\"amount\":2}]}," +
                    "{\"id\":\"origin_broken\",\"display_name\":\"Broken\"," +
                    "\"description\":\"Broken\",\"supplies\":[{\"itemId\":\"missing_item\",\"amount\":1}]}]}"
            };
            var catalog = new ItemCatalog();
            catalog.Register(new ItemDefinition { id = "clean_water" });

            var result = ItemCatalogLoader.LoadStartingSuppliesCatalogDetailed(
                "data",
                new DictionaryFileIO(files),
                new SystemTextJsonSerializer(),
                catalog);

            Assert.Empty(result.Errors);
            Assert.Contains(result.Warnings, warning => warning.Contains("missing_item", StringComparison.Ordinal));
            Assert.True(result.Catalog.TryGet(
                StartingSuppliesCatalog.StandardProfileId,
                out _));
            Assert.False(result.Catalog.TryGet("origin_broken", out _));
        }

        [Fact]
        public void ExplicitUnknownProfile_ResolvesToDefault()
        {
            var standard = StartingSuppliesCatalog.CreateLegacyFallbackProfile();
            var catalog = new StartingSuppliesCatalog(
                new[] { standard },
                StartingSuppliesCatalog.StandardProfileId);

            Assert.Same(
                catalog.DefaultProfile,
                catalog.ResolveOrDefault("origin_missing"));
            Assert.Same(
                catalog.DefaultProfile,
                catalog.ResolveOrDefault(null));
        }

        private static Dictionary<string, int> ToMap(StartingSuppliesProfile profile)
        {
            return profile.supplies.ToDictionary(
                item => item.itemId,
                item => item.amount,
                StringComparer.Ordinal);
        }

        private static ProfileMetrics Calculate(
            StartingSuppliesProfile profile,
            ItemCatalog catalog)
        {
            var result = new ProfileMetrics();
            foreach (var (itemId, amount) in profile.supplies)
            {
                var item = catalog.Get(itemId);
                Assert.NotNull(item);
                result.Value += item!.tradeValue * amount;
                result.Weight += item.weight * amount;
                result.Food += item.hungerRestore * amount;
                result.Water += item.thirstRestore * amount;
                result.Slots += (amount + item.stackMax - 1) / item.stackMax;
                result.Protection += item.radProtection * amount;

                if (item.type == ItemType.Medical ||
                    item.type == ItemType.AntiRad ||
                    item.type == ItemType.Iodine)
                    result.Medical += (item.healthEffect + item.radCleanse) * amount;

                if (item.type != ItemType.Food &&
                    item.type != ItemType.Water &&
                    item.type != ItemType.IrradiatedWater &&
                    item.type != ItemType.Medical &&
                    item.type != ItemType.AntiRad &&
                    item.type != ItemType.Iodine &&
                    item.type != ItemType.Protective)
                    result.Leverage += item.tradeValue * amount;
            }
            return result;
        }

        private static bool Dominates(ProfileMetrics left, ProfileMetrics right)
        {
            bool noWorse =
                left.Value >= right.Value &&
                left.Food >= right.Food &&
                left.Water >= right.Water &&
                left.Medical >= right.Medical &&
                left.Protection >= right.Protection &&
                left.Leverage >= right.Leverage &&
                left.Weight <= right.Weight;
            bool strictlyBetter =
                left.Value > right.Value ||
                left.Food > right.Food ||
                left.Water > right.Water ||
                left.Medical > right.Medical ||
                left.Protection > right.Protection ||
                left.Leverage > right.Leverage ||
                left.Weight < right.Weight;
            return noWorse && strictlyBetter;
        }

        private sealed class ProfileMetrics
        {
            public float Value;
            public float Weight;
            public float Food;
            public float Water;
            public float Medical;
            public float Protection;
            public float Leverage;
            public int Slots;
        }

        private sealed class DictionaryFileIO : IFileIO
        {
            private readonly Dictionary<string, string> _files;

            public DictionaryFileIO(Dictionary<string, string> files)
            {
                _files = files;
            }

            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => _files.ContainsKey(Path.GetFileName(path));
            public string ReadAllText(string path) => _files[Path.GetFileName(path)];
            public void WriteAllText(string path, string contents) => _files[Path.GetFileName(path)] = contents;
            public string Combine(params string[] parts) => parts.Length == 0 ? string.Empty : parts[^1];
            public string[] EnumerateFiles(string directory, string searchPattern, SearchOption searchOption) =>
                Array.Empty<string>();
            public string[] EnumerateJsonFiles(string dataDirectory, SearchOption searchOption) =>
                Array.Empty<string>();
        }
    }
}
