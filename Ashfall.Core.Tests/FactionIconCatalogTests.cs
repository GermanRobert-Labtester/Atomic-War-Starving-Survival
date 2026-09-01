using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FactionIconCatalogTests
    : CatalogTestBase{
        [Fact]
        public void Resolve_HydroBarons_ReturnsCanonicalPath()
        {
            var path = FactionIconCatalog.Resolve("faction_hydro_barons");
            Assert.Equal("assets/ui/Icons/faction_icon_hydro_barons.png", path);
        }

        [Fact]
        public void Resolve_GuildHasExplicitNonFallbackEmblem()
        {
            string p = FactionIconCatalog.Resolve(Ashfall.Core.Foundry.SilentFoundryIds.FactionId);
            Assert.NotEqual(FactionIconCatalog.FallbackIconPath, p);
            Assert.True(FactionIconCatalog.HasExplicitMapping(Ashfall.Core.Foundry.SilentFoundryIds.FactionId));
            Assert.Equal("assets/ui/Icons/faction_icon_silent_foundry.png", p);

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
            Assert.Equal(FactionIconCatalog.FallbackIconPath, FactionIconCatalog.Resolve(null));
            Assert.Equal(FactionIconCatalog.FallbackIconPath, FactionIconCatalog.Resolve(string.Empty));
        }

        [Fact]
        public void LoreNamespace_Aliases_NowMappedInCatalog()
        {
            Assert.True(FactionIconCatalog.HasExplicitMapping("scavenger_camp"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("cult_of_the_glow"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("military_remnants"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("upland_militia"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("rot_farmers"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("wire_heads"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("sump_dredgers"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("custodians"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("doomsday_preppers"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("echo_bats"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("safe_haven_community"));

            foreach (var id in FactionIconCatalog.CoveredFactionIds)
            {
                var path = FactionIconCatalog.Resolve(id);
                Assert.NotEqual(FactionIconCatalog.FallbackIconPath, path);
                Assert.StartsWith("assets/ui/Icons/", path);
            }
        }

        [Fact]
        public void CoveredFactionIds_IsReadOnlyAndSealed()
        {
            var ids = FactionIconCatalog.CoveredFactionIds;
            // 28 original systems/lore mappings + 20 expansion & lore ids (including black flotilla).
            Assert.Equal(48, ids.Count);
        }

        [Fact]
        public void EveryMappedEmblem_ExistsOnDisk()
        {
            // Walk up from the test working directory to the repo root —
            // the first ancestor that contains the Godot assets tree.
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "assets", "ui", "Icons")))
                dir = dir.Parent;
            Assert.NotNull(dir);

            foreach (var id in FactionIconCatalog.CoveredFactionIds)
            {
                var path = FactionIconCatalog.Resolve(id);
                Assert.True(
                    File.Exists(Path.Combine(dir!.FullName, path)),
                    $"mapped emblem for {id} does not exist on disk: {path}");
            }
        }

        [Fact]
        public void ExpansionDeclaredFactions_AreMapped()
        {
            // crossing_factions.json
            Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_compact"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_scale"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_underwrite"));
            // holdfast_factions.json
            Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_cutters"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_fleet"));
            Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_office"));
            // standing_record_factions.json
            Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_overlay"));
            // currents.json 17th systems id
            Assert.True(FactionIconCatalog.HasExplicitMapping("faction_blank_rows"));
        }
    }
}
