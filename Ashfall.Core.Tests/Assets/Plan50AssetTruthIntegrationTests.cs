// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Assets;
using Xunit;

namespace Ashfall.Core.Tests.Assets
{
    public sealed class Plan50AssetTruthIntegrationTests
    {
        [Fact]
        public void AssetManifestCatalog_ParsesJsonAndIndexesEntries()
        {
            const string json = @"{
                ""schema_version"": 1,
                ""total_assets"": 3,
                ""assets"": [
                    {
                        ""id"": ""pistol_cz75_9x19"",
                        ""family"": ""item"",
                        ""kind"": ""item_icon"",
                        ""path"": ""assets/sprites/Items/pistol_cz75_9x19.png"",
                        ""source"": ""Flux 2 Pro"",
                        ""is_ai_generated"": true,
                        ""import_preset"": ""2d_pixel_art"",
                        ""is_fallback"": false
                    },
                    {
                        ""id"": ""elena_vasquez"",
                        ""family"": ""portrait"",
                        ""kind"": ""character_portrait"",
                        ""path"": ""assets/sprites/Portraits/elena_vasquez.png"",
                        ""source"": ""Flux 2 Pro"",
                        ""is_ai_generated"": true,
                        ""import_preset"": ""2d_pixel_art"",
                        ""is_fallback"": false
                    },
                    {
                        ""id"": ""fallback_survivor"",
                        ""family"": ""portrait"",
                        ""kind"": ""character_portrait"",
                        ""path"": ""assets/sprites/Characters/placeholder_survivor.png"",
                        ""source"": ""scratch"",
                        ""is_ai_generated"": false,
                        ""import_preset"": ""2d_pixel_art"",
                        ""is_fallback"": true
                    }
                ]
            }";

            var catalog = AssetManifestCatalog.ParseJson(json);

            Assert.Equal(1, catalog.SchemaVersion);
            Assert.Equal(3, catalog.Count);

            Assert.True(catalog.TryGetEntry("pistol_cz75_9x19", out var itemEntry));
            Assert.NotNull(itemEntry);
            Assert.Equal("item", itemEntry!.Family);
            Assert.Equal("item_icon", itemEntry.Kind);
            Assert.True(itemEntry.IsAiGenerated);
            Assert.False(itemEntry.IsFallback);

            Assert.True(catalog.TryGetEntry("fallback_survivor", out var fbEntry));
            Assert.NotNull(fbEntry);
            Assert.True(fbEntry!.IsFallback);

            var portraits = catalog.GetByFamily("portrait");
            Assert.Equal(2, portraits.Count);
        }

        [Fact]
        public void AssetManifestResolver_StrictPrecedenceOrder_ManifestWinsOverConventionAndFallback()
        {
            var catalog = new AssetManifestCatalog();
            catalog.RegisterEntry(new AssetManifestEntry(
                "knife_combat",
                "item",
                "item_icon",
                "assets/sprites/Items/custom_knife.png",
                isFallback: false));

            var resolver = new AssetManifestResolver(catalog);

            // Mock filesystem where both explicit and convention exist
            var mockFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "assets/sprites/Items/custom_knife.png",
                "assets/art/knife_combat.jpg"
            };

            var result = resolver.Resolve("knife_combat", "item", path => mockFiles.Contains(path));

            Assert.Equal(AssetResolveStatus.LoadedExplicit, result.Status);
            Assert.Equal("assets/sprites/Items/custom_knife.png", result.ResolvedPath);
            Assert.False(result.IsFallback);
            Assert.True(result.IsSuccess(strictMode: true));
        }

        [Fact]
        public void AssetManifestResolver_ConventionCandidateResolvesWhenNotInManifest()
        {
            var catalog = new AssetManifestCatalog();
            var resolver = new AssetManifestResolver(catalog);

            var mockFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "assets/art/unmapped_rifle.jpg"
            };

            var result = resolver.Resolve("unmapped_rifle", "item", path => mockFiles.Contains(path));

            Assert.Equal(AssetResolveStatus.LoadedByConvention, result.Status);
            Assert.Equal("assets/art/unmapped_rifle.jpg", result.ResolvedPath);
            Assert.False(result.IsFallback);
            Assert.True(result.IsSuccess(strictMode: true));
        }

        [Fact]
        public void AssetManifestResolver_FamilyFallbackUsedWhenAssetMissing()
        {
            var catalog = new AssetManifestCatalog();
            var resolver = new AssetManifestResolver(catalog);

            // Empty mock filesystem: asset does not exist on disk
            var result = resolver.Resolve("non_existent_survivor", "portrait", path => false);

            Assert.Equal(AssetResolveStatus.FallbackUsed, result.Status);
            Assert.Equal("assets/sprites/Characters/placeholder_survivor.png", result.ResolvedPath);
            Assert.True(result.IsFallback);

            // Strict mode must reject fallbacks as failures
            Assert.False(result.IsSuccess(strictMode: true));
            // Non-strict mode tolerates fallbacks
            Assert.True(result.IsSuccess(strictMode: false));
        }

        [Fact]
        public void AssetManifestResolver_FiresSeamsOnResolutionAndFallback()
        {
            var catalog = new AssetManifestCatalog();
            var resolver = new AssetManifestResolver(catalog);

            string? resolvedId = null;
            AssetResolveResult? resolvedResult = null;
            string? interceptedId = null;
            string? interceptedPath = null;

            resolver.OnAssetResolvedSeam = (id, res) =>
            {
                resolvedId = id;
                resolvedResult = res;
            };

            resolver.OnFallbackInterceptedSeam = (id, fbPath) =>
            {
                interceptedId = id;
                interceptedPath = fbPath;
            };

            var result = resolver.Resolve("missing_item_123", "item", path => false);

            Assert.Equal("missing_item_123", resolvedId);
            Assert.NotNull(resolvedResult);
            Assert.Equal(AssetResolveStatus.FallbackUsed, resolvedResult!.Value.Status);

            Assert.Equal("missing_item_123", interceptedId);
            Assert.Equal("assets/ui/Icons/icon_placeholder.png", interceptedPath);
        }

        [Fact]
        public void AssetCoverageReport_CalculatesCoverageAndFamilyStats()
        {
            var catalog = new AssetManifestCatalog();
            catalog.RegisterEntry(new AssetManifestEntry("item_1", "item", "icon", "assets/sprites/Items/item_1.png", isFallback: false));
            catalog.RegisterEntry(new AssetManifestEntry("survivor_1", "portrait", "portrait", "assets/sprites/Portraits/survivor_1.png", isFallback: false));

            var resolver = new AssetManifestResolver(catalog);

            var existingFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "assets/sprites/Items/item_1.png",
                "assets/sprites/Portraits/survivor_1.png",
                "assets/art/item_2.jpg" // convention
            };

            var requests = new List<(string id, string family)>
            {
                ("item_1", "item"),         // Explicit
                ("survivor_1", "portrait"), // Explicit
                ("item_2", "item"),         // Convention
                ("missing_survivor", "portrait") // Fallback
            };

            var report = resolver.AuditCoverage(requests, path => existingFiles.Contains(path));

            Assert.Equal(4, report.TotalRequested);
            Assert.Equal(2, report.ExplicitLoadedCount);
            Assert.Equal(1, report.ConventionLoadedCount);
            Assert.Equal(1, report.FallbackCount);
            Assert.Equal(0, report.MissingCount);
            Assert.Equal(75.0, report.CoveragePercentage); // 3 of 4 loaded (75%)
            Assert.False(report.IsCleanStrict); // 1 fallback prevents strict clean

            Assert.True(report.ByFamily.TryGetValue("item", out var itemStats));
            Assert.Equal(2, itemStats!.TotalRequested);
            Assert.Equal(1, itemStats.ExplicitLoaded);
            Assert.Equal(1, itemStats.ConventionLoaded);
            Assert.Equal(0, itemStats.FallbackUsed);
            Assert.Equal(100.0, itemStats.CoveragePercentage);

            Assert.True(report.ByFamily.TryGetValue("portrait", out var portraitStats));
            Assert.Equal(2, portraitStats!.TotalRequested);
            Assert.Equal(1, portraitStats.ExplicitLoaded);
            Assert.Equal(0, portraitStats.ConventionLoaded);
            Assert.Equal(1, portraitStats.FallbackUsed);
            Assert.Equal(50.0, portraitStats.CoveragePercentage);
        }

        [Fact]
        public void CanonicalAssetRegistry_ParsesFromDiskAndSatisfiesIntegrity()
        {
            string catalogPath = Path.Combine(AppContext.BaseDirectory, "Data", "asset_registry.json");
            if (!File.Exists(catalogPath))
            {
                // Fallback for development test execution path
                catalogPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "asset_registry.json"));
            }

            Assert.True(File.Exists(catalogPath), $"Expected asset_registry.json at {catalogPath}");

            string json = File.ReadAllText(catalogPath);
            var catalog = AssetManifestCatalog.ParseJson(json);

            Assert.True(catalog.SchemaVersion >= 1);
            Assert.True(catalog.Count >= 300, $"Expected at least 300 assets in canonical registry, got {catalog.Count}");

            Assert.True(catalog.TryGetEntry("fallback_survivor", out var fbSurvivor));
            Assert.NotNull(fbSurvivor);
            Assert.True(fbSurvivor!.IsFallback);

            Assert.True(catalog.TryGetEntry("fallback_icon", out var fbIcon));
            Assert.NotNull(fbIcon);
            Assert.True(fbIcon!.IsFallback);
        }
    }
}
