// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Plan125Amphibious
{
    /// <summary>
    /// Plan 125 Phase 2 — amphibious_draisine_catalog.json characterization:
    /// schema, duplicate-ID rejection, ranges, kit→pump/route foreign keys,
    /// kit-vs-route-class coherence, and the naval-distinctness invariant.
    /// Engine lands in Phase 6.
    /// </summary>
    public sealed class Plan125AmphibiousDraisineCatalogTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private sealed class RealFileIo : IFileIO
        {
            public bool DirectoryExists(string path) => Directory.Exists(path);
            public bool FileExists(string path) => File.Exists(path);
            public string ReadAllText(string path) => File.ReadAllText(path);
            public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
            public string Combine(params string[] parts) => Path.Combine(parts);
        }

        private static AmphibiousDraisineCatalog LoadReal()
        {
            var catalog = AmphibiousDraisineCatalogLoader.Load(GetDataDir(), new RealFileIo());
            Assert.True(catalog.Kits.Count > 0, "amphibious_draisine_catalog.json must load with kit profiles");
            return catalog;
        }

        // TEST-AGGREGATION: source_rows=5 rule families; failures reported per row.
        [Fact]
        public void Real_catalog_passes_structural_validation()
        {
            var catalog = LoadReal();
            Assert.True(catalog.ValidateCatalog(out string error), $"catalog invalid: {error}");
        }

        [Fact]
        public void Kit_rows_have_meaningful_tradeoffs()
        {
            var catalog = LoadReal();
            var failures = new List<string>();
            foreach (var kit in catalog.kit_profiles)
            {
                if (kit.cargo_capacity_modifier_bp >= 10000)
                    failures.Add($"{kit.id}: no cargo penalty (kit must cost capacity)");
                if (kit.water_speed_modifier_bp >= 10000)
                    failures.Add($"{kit.id}: water speed not reduced (no meaningful water tradeoff)");
                if (kit.deployment_ticks <= 0) failures.Add($"{kit.id}: instant deployment");
            }
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Kit_route_bindings_are_coherent()
        {
            var catalog = LoadReal();
            var failures = new List<string>();
            foreach (var kit in catalog.kit_profiles)
            {
                foreach (var routeClassId in kit.route_class_ids)
                {
                    var routeClass = catalog.GetRouteClass(routeClassId);
                    if (routeClass == null) { failures.Add($"{kit.id}: route_class '{routeClassId}' unresolved"); continue; }
                    if (kit.flotation_rating < routeClass.min_flotation_rating)
                        failures.Add($"{kit.id}: flotation below '{routeClassId}' minimum");
                    if (kit.current_tolerance < routeClass.max_current_risk)
                        failures.Add($"{kit.id}: current tolerance below '{routeClassId}' risk");
                }
            }
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Naval_authority_stays_distinct()
        {
            // Hard invariant (plan §7.16 case 19): kits unlock only authored
            // shallow-water route classes; nothing here may grant general
            // naval/open-water capability or replace ExpeditionNavalSystem.
            var text = File.ReadAllText(Path.Combine(GetDataDir(), AmphibiousDraisineCatalogLoader.DefaultFileName));
            foreach (var forbidden in new[] { "open_water", "naval_vessel", "ocean_route", "deep_water_route" })
                Assert.False(text.Contains(forbidden, StringComparison.Ordinal),
                    $"amphibious_draisine_catalog.json must not carry '{forbidden}' (naval stays distinct)");
        }

        [Fact]
        public void Item_references_exist_in_items_json()
        {
            var dataDir = GetDataDir();
            var text = File.ReadAllText(Path.Combine(dataDir, "items.json"));
            var itemIds = new HashSet<string>(
                System.Text.RegularExpressions.Regex.Matches(text, "\"id\"\\s*:\\s*\"(item_[a-z0-9_]+)\"")
                    .Select(m => m.Groups[1].Value), StringComparer.Ordinal);
            Assert.True(itemIds.Count > 0, "items.json produced no ids");

            var catalog = LoadReal();
            var failures = new List<string>();
            foreach (var kit in catalog.kit_profiles)
                foreach (var itemId in kit.install_item_ids.Concat(kit.repair_item_ids))
                    if (!itemIds.Contains(itemId)) failures.Add($"kit {kit.id}: item '{itemId}' not in items.json");
            foreach (var pump in catalog.pump_profiles)
                foreach (var itemId in pump.repair_item_ids)
                    if (!itemIds.Contains(itemId)) failures.Add($"pump {pump.id}: item '{itemId}' not in items.json");
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Duplicate_ids_are_rejected()
        {
            var catalog = new AmphibiousDraisineCatalog
            {
                kit_profiles =
                {
                    new AmphibiousKitProfile { id = "dup_kit", compatible_vehicle_tags = { "rail_draisine" }, flotation_rating = 0.8f, current_tolerance = 0.5f, cargo_capacity_modifier_bp = 8000, water_speed_modifier_bp = 3000, deployment_ticks = 3, mass_kg = 200, pump_profile_id = "p", route_class_ids = { "r" } },
                    new AmphibiousKitProfile { id = "dup_kit", compatible_vehicle_tags = { "rail_draisine" }, flotation_rating = 0.8f, current_tolerance = 0.5f, cargo_capacity_modifier_bp = 8000, water_speed_modifier_bp = 3000, deployment_ticks = 3, mass_kg = 200, pump_profile_id = "p", route_class_ids = { "r" } }
                },
                pump_profiles = { new AmphibiousPumpProfile { id = "p", ingress_mitigation_bp = 5000, power_source = "manual" } },
                route_class_profiles = { new AmphibiousRouteClassProfile { id = "r", max_current_risk = 0.5f, min_flotation_rating = 0.5f } }
            };
            catalog.Index();
            Assert.False(catalog.ValidateCatalog(out string error));
            Assert.Contains("duplicate kit id 'dup_kit'", error);
        }

        [Fact]
        public void Incoherent_kit_route_binding_is_rejected()
        {
            var catalog = new AmphibiousDraisineCatalog
            {
                kit_profiles =
                {
                    new AmphibiousKitProfile { id = "weak_kit", compatible_vehicle_tags = { "rail_draisine" }, flotation_rating = 0.6f, current_tolerance = 0.4f, cargo_capacity_modifier_bp = 8000, water_speed_modifier_bp = 3000, deployment_ticks = 3, mass_kg = 200, pump_profile_id = "p", route_class_ids = { "r_hard" } }
                },
                pump_profiles = { new AmphibiousPumpProfile { id = "p", ingress_mitigation_bp = 5000, power_source = "manual" } },
                route_class_profiles = { new AmphibiousRouteClassProfile { id = "r_hard", max_current_risk = 0.9f, min_flotation_rating = 0.9f } }
            };
            catalog.Index();
            Assert.False(catalog.ValidateCatalog(out string error));
            Assert.Contains("weak_kit", error);
        }

        [Fact]
        public void Load_is_deterministic()
        {
            var dataDir = GetDataDir();
            var a = AmphibiousDraisineCatalogLoader.Load(dataDir, new RealFileIo());
            var b = AmphibiousDraisineCatalogLoader.Load(dataDir, new RealFileIo());
            Assert.Equal(a.kit_profiles.Count, b.kit_profiles.Count);
            foreach (var (x, y) in a.kit_profiles.Zip(b.kit_profiles, (x, y) => (x, y)))
                Assert.Equal((x.id, x.flotation_rating, x.current_tolerance, x.deployment_ticks),
                             (y.id, y.flotation_rating, y.current_tolerance, y.deployment_ticks));
        }

        [Fact]
        public void Missing_file_loads_empty_without_throwing()
        {
            var catalog = AmphibiousDraisineCatalogLoader.Load(Path.Combine(Path.GetTempPath(), "no_such_dir_125"), new RealFileIo());
            Assert.Empty(catalog.Kits);
        }
    }
}
