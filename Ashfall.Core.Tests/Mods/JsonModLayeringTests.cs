using System;
using System.IO;
using System.Linq;
using System.Threading;
using Ashfall.Core.Mods;
using Xunit;

namespace Ashfall.Core.Tests.Mods
{
    public sealed class JsonModLayeringTests
    {
        private static int _tempRootSequence;

        [Fact]
        public void EmptyModsDirectory_LeavesBaseUntouched()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());

                var result = Build(data, mods);

                Assert.Empty(result.CatalogOverlays);
                Assert.Empty(result.AcceptedModIds);
                Assert.Empty(result.Diagnostics);
            }
            finally
            {
                Directory.Delete(root, recursive: true);
            }
        }

        [Fact]
        public void ValidMods_LayerInStableLoadOrderAndPreserveReplacementPosition()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                WriteMod(mods, "late_supply", 20, true, "item_late", "Late Supply", "item_base", "Base Override");
                WriteMod(mods, "early_supply", 10, false, "item_early", "Early Supply");

                var result = Build(data, mods);

                Assert.True(result.Diagnostics.Count == 0, string.Join(" | ", result.Diagnostics.Select(d => d.ToString())));
                Assert.Equal(new[] { "early_supply", "late_supply" }, result.AcceptedModIds);
                Assert.Empty(result.Diagnostics);
                string merged = result.CatalogOverlays["items.json"];
                int baseIndex = merged.IndexOf("\"id\":\"item_base\"", StringComparison.Ordinal);
                int earlyIndex = merged.IndexOf("\"id\":\"item_early\"", StringComparison.Ordinal);
                int lateIndex = merged.IndexOf("\"id\":\"item_late\"", StringComparison.Ordinal);
                Assert.True(baseIndex >= 0 && earlyIndex > baseIndex && lateIndex > earlyIndex);
                Assert.Contains("Base Override", merged, StringComparison.Ordinal);
            }
            finally
            {
                Directory.Delete(root, recursive: true);
            }
        }

        [Fact]
        public void InvalidMod_IsolatedWithoutDiscardingValidMod()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                WriteMod(mods, "valid_supply", 0, false, "item_valid", "Valid");
                WriteManifest(mods, "bad_path", 1, false, "../items.json");
                File.WriteAllText(
                    Path.Combine(mods, "bad_path", "items.json"),
                    "{\"schema_version\":1,\"items\":[{\"id\":\"item_bad\"}]}");

                var result = Build(data, mods);

                Assert.Contains("valid_supply", result.AcceptedModIds);
                Assert.Contains("bad_path", result.RejectedModIds);
                Assert.Contains(result.Diagnostics, diagnostic => diagnostic.ModId == "bad_path");
                Assert.Contains("\"id\":\"item_valid\"", result.CatalogOverlays["items.json"], StringComparison.Ordinal);
                Assert.DoesNotContain("\"id\":\"item_bad\"", result.CatalogOverlays["items.json"], StringComparison.Ordinal);
            }
            finally
            {
                Directory.Delete(root, recursive: true);
            }
        }

        [Fact]
        public void DuplicateDefinitionAndUnknownPrefix_AreRejected()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                WriteMod(mods, "bad_definitions", 0, true, "item_duplicate", "One", "item_duplicate", "Two");
                File.WriteAllText(
                    Path.Combine(mods, "bad_definitions", "items.json"),
                    "{\"schema_version\":1,\"items\":[{\"id\":\"modded_item\"}]}");
                WriteManifest(mods, "bad_prefix", 1, false, "items.json");
                File.WriteAllText(
                    Path.Combine(mods, "bad_prefix", "items.json"),
                    "{\"schema_version\":1,\"items\":[{\"id\":\"unsafe_item\"}]}");

                var result = Build(data, mods);

                Assert.Empty(result.AcceptedModIds);
                Assert.Equal(2, result.RejectedModIds.Count);
                Assert.Equal(2, result.Diagnostics.Count);
            }
            finally
            {
                Directory.Delete(root, recursive: true);
            }
        }

        [Fact]
        public void SafetyGuards_RejectTraversalAndExecutableExtensions()
        {
            Assert.False(JsonModLayering.IsSafeModId("../escape"));
            Assert.False(JsonModLayering.IsSafeModId("UpperCase"));
            Assert.False(JsonModLayering.IsSafeCatalogFileName("../items.json"));
            Assert.False(JsonModLayering.IsSafeCatalogFileName("script.dll"));
            Assert.True(JsonModLayering.IsSafeCatalogFileName("items.json"));
        }

        private static ModLayerResult Build(string data, string mods)
        {
            return new JsonModLayering().Build(
                data,
                mods,
                new FileSystemIO(),
                new SystemTextJsonSerializer());
        }

        private static string MakeTempRoot()
        {
            string root = Path.Combine(
                Path.GetTempPath(),
                "ashfall-mod-tests-" + Interlocked.Increment(ref _tempRootSequence));
            Directory.CreateDirectory(root);
            return root;
        }

        private static string BaseCatalog() =>
            "{\"schema_version\":1,\"items\":[{\"id\":\"item_base\",\"displayName\":\"Base\"},{\"id\":\"item_second\",\"displayName\":\"Second\"}]}";

        private static void WriteMod(
            string mods,
            string id,
            int loadOrder,
            bool allowOverrides,
            params string[] definitions)
        {
            string directory = Path.Combine(mods, id);
            Directory.CreateDirectory(directory);
            WriteManifest(mods, id, loadOrder, allowOverrides, "items.json");
            var items = Enumerable.Range(0, definitions.Length / 2)
                .Select(index =>
                    $"{{\"id\":\"{definitions[index * 2]}\",\"displayName\":\"{definitions[index * 2 + 1]}\"}}")
                .ToArray();
            File.WriteAllText(
                Path.Combine(directory, "items.json"),
                "{\"schema_version\":1,\"items\":[" + string.Join(",", items) + "]}");
        }

        private static void WriteManifest(
            string mods,
            string id,
            int loadOrder,
            bool allowOverrides,
            params string[] catalogs)
        {
            string directory = Path.Combine(mods, id);
            Directory.CreateDirectory(directory);
            File.WriteAllText(
                Path.Combine(directory, "manifest.json"),
                $"{{\"schema_version\":1,\"mod_id\":\"{id}\",\"load_order\":{loadOrder},\"allow_overrides\":{(allowOverrides ? "true" : "false")},\"catalogs\":[\"{string.Join("\",\"", catalogs)}\"]}}");
        }
    }
}
