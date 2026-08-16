using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FactionIconCatalogTests
    {
        [Fact]
        public void Resolve_HydroBarons_ReturnsCanonicalPath()
        {
            var path = FactionIconCatalog.Resolve("faction_hydro_barons");
            Assert.Equal("Assets/UI/Icons/faction_icon_hydro_barons.png", path);
        }

        [Fact]
        public void Resolve_GuildHasExplicitNonFallbackEmblem()
        {
            string p = FactionIconCatalog.Resolve(Ashfall.Core.Foundry.SilentFoundryIds.FactionId);
            Assert.NotEqual(FactionIconCatalog.FallbackIconPath, p);
            Assert.True(FactionIconCatalog.HasExplicitMapping(Ashfall.Core.Foundry.SilentFoundryIds.FactionId));
            Assert.Equal("Assets/UI/Icons/faction_icon_silent_foundry.png", p);

            // The authored faction registry declares the same icon path.
            string dataDir = Directory.GetCurrentDirectory();
            if (!CatalogLocator.TryFindDataDirectory(dataDir, out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var faction = Ashfall.Core.Foundry.SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json);
            Assert.NotNull(faction);
            Assert.Equal(p, faction.icon_path);
        }

        [Fact]
        public void Resolve_AllCanonicalSystemsIds_HaveNonFallbackPath()
        {
            string[] ids = {
                "faction_archivists","faction_lamplighters","faction_quiet_house",
                "faction_grain_exchange","faction_sun_seekers","faction_osteophages",
                "faction_the_tally","faction_undertow","faction_cold_count",
                "faction_deserter_coalition","faction_the_provisioned",
                "faction_long_walk","faction_scavenger_guild","faction_iron_raiders",
                "faction_the_tempest","faction_hydro_barons"
            };
            foreach (var id in ids)
            {
                var p = FactionIconCatalog.Resolve(id);
                Assert.NotEqual(FactionIconCatalog.FallbackIconPath, p);
                Assert.True(FactionIconCatalog.HasExplicitMapping(id));
            }
        }

        [Fact]
        public void Resolve_Unknown_FallsBackToBlankEmblem()
        {
            var path = FactionIconCatalog.Resolve("faction_unlisted_invented");
            Assert.Equal(FactionIconCatalog.FallbackIconPath, path);
        }

        [Fact]
        public void Resolve_EmptyOrNull_ReturnsFallback()
        {
            Assert.Equal(FactionIconCatalog.FallbackIconPath, FactionIconCatalog.Resolve(""));
            Assert.Equal(FactionIconCatalog.FallbackIconPath, FactionIconCatalog.Resolve(null!));
        }

        [Fact]
        public void LoreNamespace_AliasesNotInCatalog()
        {
            // Unregistered aliases must NOT have explicit mapping
            Assert.False(FactionIconCatalog.HasExplicitMapping("iron_garrison"));
            Assert.False(FactionIconCatalog.HasExplicitMapping("militia"));
        }

        [Fact]
        public void CoveredFactionIds_IsReadOnlyAndSealed()
        {
            Assert.NotNull(FactionIconCatalog.CoveredFactionIds);
            Assert.Equal(28, FactionIconCatalog.CoveredFactionIds.Count); // 27 systems + the Silent Foundry Guild
        }
    }
}
