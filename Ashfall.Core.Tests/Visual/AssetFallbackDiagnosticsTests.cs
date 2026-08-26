using System;
using System.IO;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.Visual
{
    public class AssetFallbackDiagnosticsTests
    {
        [Fact]
        public void FactionIconCatalog_FallbackPath_IsCanonical()
        {
            Assert.Equal("assets/ui/Icons/icon_unknown_faction.png", FactionIconCatalog.FallbackIconPath);
        }

        [Fact]
        public void FactionIconCatalog_UnknownId_ResolvesToFallback()
        {
            string resolved = FactionIconCatalog.Resolve("faction_nonexistent_unknown");
            Assert.Equal(FactionIconCatalog.FallbackIconPath, resolved);
            Assert.False(FactionIconCatalog.HasExplicitMapping("faction_nonexistent_unknown"));
        }

        [Fact]
        public void CanonicalFallbackAssets_ExistOnDisk()
        {
            string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));
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
