// SPDX-License-Identifier: MIT
// Plan 165 — strict mod-manifest specification loader, registry, and census tests.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Mods;
using Xunit;

namespace Ashfall.Core.Tests.Mods
{
    public sealed class ModManifestSpecificationLoaderTests
    {
        private static string RepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        private static string AuthoredSpecJson() =>
            File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data", "mod_manifest_schema.json"));

        private static ModManifest Manifest(
            string id, int loadOrder = 0, bool allowOverrides = true,
            IEnumerable<string>? catalogs = null, IEnumerable<string>? deps = null, string? gameRange = null)
        {
            return new ModManifest
            {
                SchemaVersion = 1,
                ModId = id,
                DisplayName = id,
                Version = "1.0.0",
                LoadOrder = loadOrder,
                AllowOverrides = allowOverrides,
                Catalogs = new List<string>(catalogs ?? new[] { "items.json" }),
                Dependencies = new List<string>(deps ?? Array.Empty<string>()),
                GameRange = gameRange
            };
        }

        [Fact]
        public void Authored_Specification_Loads_And_Binds()
        {
            var spec = ModManifestSpecificationLoader.LoadFromJson(AuthoredSpecJson());
            Assert.True(spec.AllowedCatalogs.Count >= 9);
            Assert.True(spec.RequiredManifestFields.Count >= 5);

            var system = new ModSupportSystem();
            system.BindSpecification(spec);
            Assert.Equal(spec.AllowedCatalogs.Count, system.Specification.AllowedCatalogs.Count);
        }

        [Theory]
        [InlineData("{\"schema_version\":1,\"schema_name\":\"s\",\"allowed_catalogs\":[\"a.json\",\"a.json\"],\"required_manifest_fields\":[\"mod_id\"]}")]
        [InlineData("{\"schema_version\":1,\"schema_name\":\"s\",\"allowed_catalogs\":[],\"required_manifest_fields\":[\"mod_id\"]}")]
        [InlineData("{\"schema_version\":1,\"schema_name\":\"\",\"allowed_catalogs\":[\"a.json\"],\"required_manifest_fields\":[\"mod_id\"]}")]
        [InlineData("{\"schema_version\":9,\"schema_name\":\"s\",\"allowed_catalogs\":[\"a.json\"],\"required_manifest_fields\":[\"mod_id\"]}")]
        [InlineData("{\"schema_version\":1,\"schema_name\":\"s\",\"allowed_catalogs\":[\"a.json\"],\"required_manifest_fields\":[]}")]
        public void Malformed_Specification_Is_Rejected(string json)
        {
            Assert.Throws<InvalidOperationException>(() => ModManifestSpecificationLoader.LoadFromJson(json));
        }

        [Fact]
        public void Registry_Resolves_Dependencies_And_Load_Order()
        {
            var system = new ModSupportSystem();
            system.RegisterMod(Manifest("alpha", loadOrder: 10, allowOverrides: true));
            system.RegisterMod(Manifest("beta", loadOrder: 20, allowOverrides: true, deps: new[] { "alpha" }));
            system.RegisterMod(Manifest("gamma", loadOrder: 5, allowOverrides: true));

            var order = system.ResolveLoadOrder();
            Assert.Contains("alpha", order);
            Assert.Contains("beta", order);
            Assert.True(order.ToList().IndexOf("alpha") < order.ToList().IndexOf("beta"));

            var census = system.GetCensus();
            Assert.Equal(3, census.TotalMods);
            Assert.Equal(3, census.EnabledMods);
        }

        [Fact]
        public void Missing_And_Cyclic_Dependencies_Are_Statused()
        {
            var system = new ModSupportSystem();
            system.RegisterMod(Manifest("needs_ghost", deps: new[] { "ghost" }));
            system.RegisterMod(Manifest("cyc_a", deps: new[] { "cyc_b" }));
            system.RegisterMod(Manifest("cyc_b", deps: new[] { "cyc_a" }));

            Assert.Equal(ModStatus.MissingDependency, system.RegisteredMods["needs_ghost"].Status);
            Assert.Equal(ModStatus.CyclicDependency, system.RegisteredMods["cyc_a"].Status);
            Assert.Equal(ModStatus.CyclicDependency, system.RegisteredMods["cyc_b"].Status);
            Assert.Equal(1, system.GetCensus().MissingDependencyMods);
            Assert.Equal(2, system.GetCensus().CyclicMods);
        }

        [Fact]
        public void Invalid_Manifest_And_Incompatible_Range_Are_Statused()
        {
            var system = new ModSupportSystem();
            system.RegisterMod(Manifest("ok"));
            system.RegisterMod(Manifest("Bad-ID"));
            system.RegisterMod(Manifest("future", gameRange: ">=9.0.0"));

            Assert.Equal(ModStatus.InvalidManifest, system.RegisteredMods["Bad-ID"].Status);
            Assert.Equal(ModStatus.Incompatible, system.RegisteredMods["future"].Status);
            Assert.Equal(1, system.GetCensus().InvalidMods);
            Assert.Equal(1, system.GetCensus().IncompatibleMods);
        }

        [Fact]
        public void Override_Conflict_Requires_Both_Mods_To_Forbid_Overrides()
        {
            var system = new ModSupportSystem();
            system.RegisterMod(Manifest("m1", allowOverrides: false));
            system.RegisterMod(Manifest("m2", allowOverrides: false));
            system.RegisterMod(Manifest("m3", allowOverrides: true));

            var conflicts = system.DetectConflicts();
            Assert.Contains(conflicts, c => c.CatalogName == "items.json"
                && (c.ModIdA == "m1" || c.ModIdB == "m1") && (c.ModIdA == "m2" || c.ModIdB == "m2"));
            // m3 allows overrides, so it conflicts with neither.
            Assert.DoesNotContain(conflicts, c => c.ModIdA == "m3" || c.ModIdB == "m3");
        }

        [Fact]
        public void RestoreState_Rejects_Newer_Schema_And_Accepts_Legacy()
        {
            var system = new ModSupportSystem();
            system.RegisterMod(Manifest("alpha"));
            system.SetModEnabled("alpha", false);

            var state = system.CaptureState();
            var restored = new ModSupportSystem();
            restored.RegisterMod(Manifest("alpha"));
            restored.RestoreState(state);
            Assert.Equal(ModStatus.Disabled, restored.RegisteredMods["alpha"].Status);

            var newer = system.CaptureState();
            newer.SchemaVersion = 99;
            Assert.Throws<InvalidOperationException>(() => system.RestoreState(newer));

            var legacy = system.CaptureState();
            legacy.SchemaVersion = 0;
            system.RestoreState(legacy); // legacy payload is accepted and normalized
            Assert.Equal(1, system.CaptureState().SchemaVersion);
        }
    }
}
