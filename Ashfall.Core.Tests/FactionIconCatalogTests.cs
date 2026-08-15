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
            // lore ns such as scavenger_camp / iron_garrison / cult_of_the_glow
            // must NOT collapse silently to a systems id.
            Assert.False(FactionIconCatalog.HasExplicitMapping("scavenger_camp"));
            Assert.False(FactionIconCatalog.HasExplicitMapping("iron_garrison"));
            Assert.False(FactionIconCatalog.HasExplicitMapping("cult_of_the_glow"));
            Assert.False(FactionIconCatalog.HasExplicitMapping("militia"));
        }

        [Fact]
        public void CoveredFactionIds_IsReadOnlyAndSealed()
        {
            Assert.NotNull(FactionIconCatalog.CoveredFactionIds);
            Assert.Equal(16, FactionIconCatalog.CoveredFactionIds.Count);
        }
    }
}
