using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.Visual
{
    public class AssetFallbackDiagnosticsTests
    {
        private static string RepoRoot()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    if (File.Exists(Path.Combine(dir.FullName, "project.godot")))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate repository root (project.godot) from test run");
        }

        [Fact]
        public void FactionIconCatalog_FallbackPath_IsCanonicalAndRelative()
        {
            Assert.Equal("assets/ui/Icons/icon_unknown_faction.png", FactionIconCatalog.FallbackIconPath);
            Assert.False(FactionIconCatalog.FallbackIconPath.StartsWith("res://", StringComparison.OrdinalIgnoreCase),
                "Core fallback icon path must be a relative path and must not use the Godot res:// URI scheme.");
            Assert.False(FactionIconCatalog.FallbackIconPath.StartsWith("/", StringComparison.Ordinal),
                "Core fallback icon path must be a relative path without leading slash.");
        }

        [Fact]
        public void FactionIconCatalog_UnknownId_ResolvesToFallback()
        {
            string resolved = FactionIconCatalog.Resolve("faction_nonexistent_unknown");
            Assert.Equal(FactionIconCatalog.FallbackIconPath, resolved);
            Assert.False(FactionIconCatalog.HasExplicitMapping("faction_nonexistent_unknown"));
        }

        [Fact]
        public void FactionIconCatalog_AllMappedPaths_AreRelativeAndNeverUseResScheme()
        {
            foreach (string factionId in FactionIconCatalog.CoveredFactionIds)
            {
                string path = FactionIconCatalog.Resolve(factionId);
                Assert.False(path.StartsWith("res://", StringComparison.OrdinalIgnoreCase),
                    $"Core faction icon path for '{factionId}' must not use res:// scheme: {path}");
                Assert.False(path.StartsWith("/", StringComparison.Ordinal),
                    $"Core faction icon path for '{factionId}' must be relative without leading slash: {path}");
                Assert.StartsWith("assets/", path, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void HostAssetRegistry_FallbackPaths_UseResSchemeOnlyAtGodotResourceBoundary()
        {
            string root = RepoRoot();
            string assetRegistryPath = Path.Combine(root, "src", "Host", "AssetRegistry.cs");
            Assert.True(File.Exists(assetRegistryPath), $"AssetRegistry.cs not found at {assetRegistryPath}");

            string code = File.ReadAllText(assetRegistryPath);

            // Extract constants using regex
            var matchSurvivorRes = Regex.Match(code, @"public\s+const\s+string\s+FallbackSurvivorPath\s*=\s*""([^""]+)"";");
            var matchSurvivorRel = Regex.Match(code, @"public\s+const\s+string\s+FallbackSurvivorRelativePath\s*=\s*""([^""]+)"";");
            var matchIconRes = Regex.Match(code, @"public\s+const\s+string\s+FallbackIconPath\s*=\s*""([^""]+)"";");
            var matchIconRel = Regex.Match(code, @"public\s+const\s+string\s+FallbackIconRelativePath\s*=\s*""([^""]+)"";");

            Assert.True(matchSurvivorRes.Success, "FallbackSurvivorPath constant not found in AssetRegistry.cs");
            Assert.True(matchSurvivorRel.Success, "FallbackSurvivorRelativePath constant not found in AssetRegistry.cs");
            Assert.True(matchIconRes.Success, "FallbackIconPath constant not found in AssetRegistry.cs");
            Assert.True(matchIconRel.Success, "FallbackIconRelativePath constant not found in AssetRegistry.cs");

            string survivorRes = matchSurvivorRes.Groups[1].Value;
            string survivorRel = matchSurvivorRel.Groups[1].Value;
            string iconRes = matchIconRes.Groups[1].Value;
            string iconRel = matchIconRel.Groups[1].Value;

            // Resource paths MUST start with res://
            Assert.StartsWith("res://", survivorRes, StringComparison.Ordinal);
            Assert.StartsWith("res://", iconRes, StringComparison.Ordinal);

            // Relative paths MUST NOT start with res://
            Assert.False(survivorRel.StartsWith("res://", StringComparison.OrdinalIgnoreCase),
                $"Relative survivor path '{survivorRel}' must not contain res:// prefix.");
            Assert.False(iconRel.StartsWith("res://", StringComparison.OrdinalIgnoreCase),
                $"Relative icon path '{iconRel}' must not contain res:// prefix.");

            // Resource paths MUST equal "res://" + relative path
            Assert.Equal("res://" + survivorRel, survivorRes);
            Assert.Equal("res://" + iconRel, iconRes);
        }

        [Fact]
        public void CoreUI_AssetCatalogs_UseRelativePathsOnly()
        {
            // Core UI catalogs (FactionIconCatalog, UiAssetManifest, etc.) must use
            // relative paths and never assume or hardcode Godot's res:// resource scheme.
            Assert.False(FactionIconCatalog.FallbackIconPath.StartsWith("res://", StringComparison.OrdinalIgnoreCase));

            foreach (var bg in UiAssetManifest.RequiredPreviewTextures())
            {
                Assert.False(bg.StartsWith("res://", StringComparison.OrdinalIgnoreCase),
                    $"UiAssetManifest texture '{bg}' must be a relative path and not start with res://");
                Assert.False(bg.StartsWith("/", StringComparison.Ordinal),
                    $"UiAssetManifest texture '{bg}' must be a relative path without leading slash.");
            }
        }

        [Fact]
        public void CanonicalFallbackAssets_ExistOnDisk()
        {
            string root = RepoRoot();
            string[] fallbackAssets = new[]
            {
                "assets/sprites/Characters/placeholder_survivor.png",
                "assets/ui/Icons/icon_placeholder.png",
                "assets/ui/Icons/icon_unknown_faction.png"
            };

            foreach (var relPath in fallbackAssets)
            {
                string fullPath = Path.Combine(root, relPath);
                Assert.True(File.Exists(fullPath), $"Canonical fallback asset missing from disk: {fullPath}");
            }
        }
    }
}
