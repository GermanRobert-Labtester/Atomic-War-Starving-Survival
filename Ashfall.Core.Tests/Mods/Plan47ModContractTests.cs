// SPDX-License-Identifier: MIT
// Plan 47 / C1[15] — The Mod & Content-Pack Contract
// Tests: compatibility governance, deterministic overlay, typed rejection, content-pack acceptance, modded replay.
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Ashfall.Core.Mods;
using Xunit;

namespace Ashfall.Core.Tests.Mods
{
    public sealed class Plan47ModContractTests
    {
        private static int _seq;

        // ── Compatibility Governance ─────────────────────────────────────

        [Fact]
        public void CompatibleGameVersion_Accepted()
        {
            var result = RunWithVersionRange(gameRange: ">=1.0 <2.0", currentGame: "1.5.0", contractRange: null, currentContract: 1);
            Assert.Contains("version_mod", result.AcceptedModIds);
            Assert.Empty(result.RejectedModIds);
        }

        [Fact]
        public void TooOldGameVersion_RejectedWithTypedCode()
        {
            var result = RunWithVersionRange(gameRange: ">=2.0", currentGame: "1.0.0", contractRange: null, currentContract: 1);
            Assert.Contains("version_mod", result.RejectedModIds);
            Assert.Contains(result.Diagnostics, d =>
                d.Code == ModRejectionCode.IncompatibleGameVersion && d.ModId == "version_mod");
        }

        [Fact]
        public void TooNewGameVersion_RejectedWithTypedCode()
        {
            var result = RunWithVersionRange(gameRange: "<1.5", currentGame: "2.0.0", contractRange: null, currentContract: 1);
            Assert.Contains("version_mod", result.RejectedModIds);
            Assert.Contains(result.Diagnostics, d => d.Code == ModRejectionCode.IncompatibleGameVersion);
        }

        [Fact]
        public void MalformedGameRange_RejectedWithMalformedCode()
        {
            var result = RunWithVersionRange(gameRange: ">=not_a_version", currentGame: "1.0.0", contractRange: null, currentContract: 1);
            Assert.Contains("version_mod", result.RejectedModIds);
            Assert.Contains(result.Diagnostics, d => d.Code == ModRejectionCode.MalformedVersionRange);
        }

        [Fact]
        public void CompatibleModContractVersion_Accepted()
        {
            var result = RunWithVersionRange(gameRange: null, currentGame: "1.0.0", contractRange: ">=1 <2", currentContract: 1);
            Assert.Contains("version_mod", result.AcceptedModIds);
        }

        [Fact]
        public void IncompatibleModContractVersion_RejectedWithTypedCode()
        {
            var result = RunWithVersionRange(gameRange: null, currentGame: "1.0.0", contractRange: ">=5", currentContract: 1);
            Assert.Contains("version_mod", result.RejectedModIds);
            Assert.Contains(result.Diagnostics, d => d.Code == ModRejectionCode.IncompatibleModContract);
        }

        [Fact]
        public void EmptyGameRange_AcceptsAllVersions()
        {
            var result = RunWithVersionRange(gameRange: "", currentGame: "99.0.0", contractRange: null, currentContract: 1);
            Assert.Contains("version_mod", result.AcceptedModIds);
        }

        [Fact]
        public void WildcardGameRange_AcceptsAllVersions()
        {
            var result = RunWithVersionRange(gameRange: "*", currentGame: "99.0.0", contractRange: null, currentContract: 1);
            Assert.Contains("version_mod", result.AcceptedModIds);
        }

        // ── Deterministic Overlay Order ──────────────────────────────────

        [Fact]
        public void SameModSetAndSeed_ProduceIdenticalOverlay()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                WriteMod(mods, "mod_alpha", 10, false, "item_alpha", "Alpha");
                WriteMod(mods, "mod_beta", 20, false, "item_beta", "Beta");

                var result1 = Build(data, mods);
                var result2 = Build(data, mods);

                Assert.True(result1.Success);
                Assert.Equal(result1.AcceptedModIds, result2.AcceptedModIds);
                Assert.Equal(result1.CatalogOverlays["items.json"], result2.CatalogOverlays["items.json"]);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        [Fact]
        public void LoadOrderDeterminesOverlayOrder_NotFilesystemOrder()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                // mod_zzz has higher load_order (50) so is applied after mod_aaa (10)
                WriteMod(mods, "mod_zzz", 50, false, "item_zzz", "ZZZ");
                WriteMod(mods, "mod_aaa", 10, false, "item_aaa", "AAA");

                var result = Build(data, mods);

                Assert.Equal(new[] { "mod_aaa", "mod_zzz" }, result.AcceptedModIds);
                string merged = result.CatalogOverlays["items.json"];
                Assert.True(merged.IndexOf("item_aaa", StringComparison.Ordinal) < merged.IndexOf("item_zzz", StringComparison.Ordinal));
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        [Fact]
        public void TieBrokenByModIdOrdinal_NotDirectoryOrder()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                // Both have load_order=0 — tie broken by ModId alphabetically
                WriteMod(mods, "mod_b", 0, false, "item_b", "B");
                WriteMod(mods, "mod_a", 0, false, "item_a", "A");

                var result = Build(data, mods);

                Assert.Equal("mod_a", result.AcceptedModIds[0]);
                Assert.Equal("mod_b", result.AcceptedModIds[1]);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        // ── Dependency Resolution ────────────────────────────────────────

        [Fact]
        public void ModWithMissingDependency_RejectedWithTypedCode()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                WriteManifestWithDeps(mods, "needy_mod", 0, false, new[] { "item_needy" }, deps: new[] { "nonexistent_mod" });

                var result = Build(data, mods);

                Assert.Contains("needy_mod", result.RejectedModIds);
                Assert.Contains(result.Diagnostics, d => d.Code == ModRejectionCode.MissingDependency);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        [Fact]
        public void CircularDependency_RejectedWithTypedCode()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                WriteManifestWithDeps(mods, "mod_x", 0, false, new[] { "item_x" }, deps: new[] { "mod_y" });
                WriteManifestWithDeps(mods, "mod_y", 0, false, new[] { "item_y" }, deps: new[] { "mod_x" });

                var result = Build(data, mods);

                // Both should be rejected due to circular dependency
                Assert.NotEmpty(result.RejectedModIds);
                Assert.Contains(result.Diagnostics, d => d.Code == ModRejectionCode.CircularDependency);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        // ── Typed Rejection: No Silent Skip ─────────────────────────────

        [Fact]
        public void UnsafeModId_RejectedWithUnsafeCode()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                // Write manifest with uppercase mod_id
                string modDir = Path.Combine(mods, "BadMod");
                Directory.CreateDirectory(modDir);
                File.WriteAllText(Path.Combine(modDir, "manifest.json"),
                    "{\"schema_version\":1,\"mod_id\":\"BadMod\",\"load_order\":0,\"allow_overrides\":false,\"catalogs\":[\"items.json\"]}");

                var result = Build(data, mods);

                Assert.NotEmpty(result.RejectedModIds);
                Assert.Contains(result.Diagnostics, d => d.Code == ModRejectionCode.UnsafeModId);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        [Fact]
        public void DuplicateModId_RejectedWithDuplicateCode()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                // Two directories with same mod_id
                string dir1 = Path.Combine(mods, "dir_one");
                string dir2 = Path.Combine(mods, "dir_two");
                Directory.CreateDirectory(dir1);
                Directory.CreateDirectory(dir2);
                string manifest = "{\"schema_version\":1,\"mod_id\":\"same_id\",\"load_order\":0,\"allow_overrides\":false,\"catalogs\":[\"items.json\"]}";
                File.WriteAllText(Path.Combine(dir1, "manifest.json"), manifest);
                File.WriteAllText(Path.Combine(dir2, "manifest.json"), manifest);
                File.WriteAllText(Path.Combine(dir1, "items.json"),
                    "{\"schema_version\":1,\"items\":[{\"id\":\"item_first\",\"displayName\":\"First\"}]}");
                File.WriteAllText(Path.Combine(dir2, "items.json"),
                    "{\"schema_version\":1,\"items\":[{\"id\":\"item_second_mod\",\"displayName\":\"Second\"}]}");

                var result = Build(data, mods);

                // first directory accepted, second rejected as duplicate
                Assert.Contains(result.Diagnostics, d => d.Code == ModRejectionCode.DuplicateModId);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        [Fact]
        public void OverrideNotAllowed_RejectedWithOverrideCode()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                // Try to override item_base when allow_overrides=false
                WriteMod(mods, "override_mod", 0, allowOverrides: false, "item_base", "Override Attempt");

                var result = Build(data, mods);

                Assert.Contains("override_mod", result.RejectedModIds);
                Assert.Contains(result.Diagnostics, d => d.Code == ModRejectionCode.OverrideNotAllowed);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        // ── Content Acceptance Pipeline Integration ──────────────────────

        [Fact]
        public void ContentPackWithPassingAcceptanceValidator_Accepted()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                WriteContentPack(mods, "good_pack", 0, new[] { "item_pack1" });

                // Validator always passes
                var result = Build(data, mods,
                    customPackAcceptanceValidator: (catalog, content) => true);

                Assert.Contains("good_pack", result.AcceptedModIds);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        [Fact]
        public void ContentPackWithFailingAcceptanceValidator_RejectedWithTypedCode()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                WriteContentPack(mods, "bad_pack", 0, new[] { "item_bad1" });

                // Validator always fails
                var result = Build(data, mods,
                    customPackAcceptanceValidator: (catalog, content) => false);

                Assert.Contains("bad_pack", result.RejectedModIds);
                Assert.Contains(result.Diagnostics, d => d.Code == ModRejectionCode.AcceptancePipelineFailed);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        [Fact]
        public void RegularMod_SkipPackAcceptanceEvenWithValidatorPresent()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                // Regular mod (pack_type=mod, default)
                WriteMod(mods, "regular_mod", 0, false, "item_regular", "Regular");

                // Even with a failing validator, regular mods must not be rejected by it
                var result = Build(data, mods,
                    customPackAcceptanceValidator: (catalog, content) => false);

                Assert.Contains("regular_mod", result.AcceptedModIds);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        // ── Modded Replay Stability ──────────────────────────────────────

        [Fact]
        public void ModdedReplay_SameModSet_ProducesIdenticalCatalogFingerprint()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                WriteMod(mods, "replay_mod", 10, false, "item_replay", "Replay");

                var fingerprints = Enumerable.Range(0, 5)
                    .Select(_ => Build(data, mods))
                    .Select(r => r.CatalogOverlays.TryGetValue("items.json", out var v) ? v : "")
                    .ToArray();

                // All 5 runs must produce identical fingerprint
                Assert.All(fingerprints, fp => Assert.Equal(fingerprints[0], fp));
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        // ── Backward Compatibility: Old Manifest Defaults ────────────────

        [Fact]
        public void OldManifest_WithoutVersionFields_AcceptedWithDefaults()
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                // Old manifest without game_range/mod_contract_range — should default to accept-all
                string modDir = Path.Combine(mods, "old_mod");
                Directory.CreateDirectory(modDir);
                File.WriteAllText(Path.Combine(modDir, "manifest.json"),
                    "{\"schema_version\":1,\"mod_id\":\"old_mod\",\"load_order\":0,\"allow_overrides\":false,\"catalogs\":[\"items.json\"]}");
                File.WriteAllText(Path.Combine(modDir, "items.json"),
                    "{\"schema_version\":1,\"items\":[{\"id\":\"item_old\",\"displayName\":\"Old\"}]}");

                var result = Build(data, mods, currentGameVersion: "9.9.9", currentContractVersion: 99);

                Assert.Contains("old_mod", result.AcceptedModIds);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        // ── ModCompatibilityEvaluator Unit Tests ─────────────────────────

        [Theory]
        [InlineData("1.2.0", ">=1.0 <2.0", true)]
        [InlineData("2.0.0", ">=1.0 <2.0", false)]
        [InlineData("0.9.0", ">=1.0 <2.0", false)]
        [InlineData("1.0.0", ">=1.0 <2.0", true)]
        [InlineData("1.9.9", ">=1.0 <2.0", true)]
        [InlineData("3.0.0", "*", true)]
        [InlineData("99.0.0", "", true)]
        [InlineData("1.5.0", "^1.0.0", true)]
        [InlineData("2.0.0", "^1.0.0", false)]
        [InlineData("1.2.3", "~1.2.0", true)]
        [InlineData("1.3.0", "~1.2.0", false)]
        public void EvaluateGameVersion_Theory(string currentVersion, string range, bool expectedResult)
        {
            bool result = ModCompatibilityEvaluator.EvaluateGameVersion(currentVersion, range, out _);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, ">=1 <2", true)]
        [InlineData(2, ">=1 <2", false)]
        [InlineData(1, "*", true)]
        [InlineData(5, ">=3", true)]
        [InlineData(2, "==2", true)]
        [InlineData(3, "==2", false)]
        public void EvaluateModContractVersion_Theory(int contractVersion, string range, bool expectedResult)
        {
            bool result = ModCompatibilityEvaluator.EvaluateModContractVersion(contractVersion, range, out _);
            Assert.Equal(expectedResult, result);
        }

        // ── Helpers ──────────────────────────────────────────────────────

        private static ModLayerResult Build(
            string data,
            string mods,
            string currentGameVersion = ModCompatibilityEvaluator.DefaultGameVersion,
            int currentContractVersion = ModCompatibilityEvaluator.DefaultModContractVersion,
            Func<string, string, bool>? customPackAcceptanceValidator = null)
        {
            return new JsonModLayering().Build(
                data,
                mods,
                new FileSystemIO(),
                new SystemTextJsonSerializer(),
                enabledModIds: null,
                validatePackAcceptance: false,
                customPackAcceptanceValidator: customPackAcceptanceValidator,
                currentGameVersion: currentGameVersion,
                currentContractVersion: currentContractVersion);
        }

        private ModLayerResult RunWithVersionRange(
            string? gameRange,
            string currentGame,
            string? contractRange,
            int currentContract)
        {
            string root = MakeTempRoot();
            try
            {
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(Path.Combine(data, "items.json"), BaseCatalog());
                WriteModWithVersionRange(mods, "version_mod", gameRange, contractRange);
                return Build(data, mods, currentGameVersion: currentGame, currentContractVersion: currentContract);
            }
            finally { Directory.Delete(root, recursive: true); }
        }

        private static void WriteModWithVersionRange(string mods, string id, string? gameRange, string? contractRange)
        {
            string dir = Path.Combine(mods, id);
            Directory.CreateDirectory(dir);
            string gameRangePart = gameRange != null ? $",\"game_range\":\"{gameRange}\"" : "";
            string contractRangePart = contractRange != null ? $",\"mod_contract_range\":\"{contractRange}\"" : "";
            File.WriteAllText(Path.Combine(dir, "manifest.json"),
                $"{{\"schema_version\":1,\"mod_id\":\"{id}\",\"load_order\":0,\"allow_overrides\":false,\"catalogs\":[\"items.json\"]{gameRangePart}{contractRangePart}}}");
            File.WriteAllText(Path.Combine(dir, "items.json"),
                "{\"schema_version\":1,\"items\":[{\"id\":\"item_ver\",\"displayName\":\"Ver\"}]}");
        }

        private static void WriteContentPack(string mods, string id, int loadOrder, string[] itemIds)
        {
            string dir = Path.Combine(mods, id);
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, "manifest.json"),
                $"{{\"schema_version\":1,\"mod_id\":\"{id}\",\"load_order\":{loadOrder},\"allow_overrides\":false,\"catalogs\":[\"items.json\"],\"pack_type\":\"content_pack\"}}");
            var items = itemIds.Select(iid => $"{{\"id\":\"{iid}\",\"displayName\":\"{iid}\"}}");
            File.WriteAllText(Path.Combine(dir, "items.json"),
                $"{{\"schema_version\":1,\"items\":[{string.Join(",", items)}]}}");
        }

        private static void WriteManifestWithDeps(string mods, string id, int loadOrder, bool allowOverrides, string[] itemIds, string[] deps)
        {
            string dir = Path.Combine(mods, id);
            Directory.CreateDirectory(dir);
            string depsJson = "[" + string.Join(",", deps.Select(d => $"\"{d}\"")) + "]";
            File.WriteAllText(Path.Combine(dir, "manifest.json"),
                $"{{\"schema_version\":1,\"mod_id\":\"{id}\",\"load_order\":{loadOrder},\"allow_overrides\":{(allowOverrides ? "true" : "false")},\"catalogs\":[\"items.json\"],\"dependencies\":{depsJson}}}");
            var items = itemIds.Select(iid => $"{{\"id\":\"{iid}\",\"displayName\":\"{iid}\"}}");
            File.WriteAllText(Path.Combine(dir, "items.json"),
                $"{{\"schema_version\":1,\"items\":[{string.Join(",", items)}]}}");
        }

        private static void WriteMod(string mods, string id, int loadOrder, bool allowOverrides, params string[] definitions)
        {
            string dir = Path.Combine(mods, id);
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, "manifest.json"),
                $"{{\"schema_version\":1,\"mod_id\":\"{id}\",\"load_order\":{loadOrder},\"allow_overrides\":{(allowOverrides ? "true" : "false")},\"catalogs\":[\"items.json\"]}}");
            var items = Enumerable.Range(0, definitions.Length / 2)
                .Select(i => $"{{\"id\":\"{definitions[i * 2]}\",\"displayName\":\"{definitions[i * 2 + 1]}\"}}");
            File.WriteAllText(Path.Combine(dir, "items.json"),
                $"{{\"schema_version\":1,\"items\":[{string.Join(",", items)}]}}");
        }

        private static string BaseCatalog() =>
            "{\"schema_version\":1,\"items\":[{\"id\":\"item_base\",\"displayName\":\"Base\"},{\"id\":\"item_second\",\"displayName\":\"Second\"}]}";

        private static string MakeTempRoot()
        {
            string root = Path.Combine(Path.GetTempPath(),
                "ashfall-plan47-tests-" + Interlocked.Increment(ref _seq));
            Directory.CreateDirectory(root);
            return root;
        }
    }
}
