using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class FactionTerritoryCatalogTests
    {
        private static string GetDataPath()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;
            probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);
            probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);
            return Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void Catalog_LoadsSuccessfully_Contains19TerritoriesAnd5ContestedZones()
        {
            var fileIO = new FileSystemIO();
            var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);

            Assert.NotNull(catalog);
            Assert.Equal(19, catalog.TerritoryCount);
            Assert.Equal(5, catalog.ContestedZoneCount);
            Assert.Equal(19, catalog.Territories.Count);
            Assert.Equal(5, catalog.ContestedZones.Count);
        }

        [Fact]
        public void EveryTerritory_HasValidIdAndRecognizedClassification()
        {
            var fileIO = new FileSystemIO();
            var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);

            var validClasses = new[] { "territorial", "nomadic", "ideological", "mixed" };
            var validScales = new[] { "major", "medium", "minor", "none" };

            foreach (var t in catalog.Territories)
            {
                Assert.StartsWith("territory_", t.id);
                Assert.StartsWith("faction_", t.faction);
                Assert.False(string.IsNullOrWhiteSpace(t.display_name));
                Assert.Contains(t.classification, validClasses);
                Assert.Contains(t.territory_scale, validScales);
                Assert.InRange(t.control_strength, 0, 100);
                Assert.InRange(t.trade_tax, 0.0f, 1.0f);
                Assert.InRange(t.travel_safety, 0.0f, 1.0f);
                Assert.False(string.IsNullOrWhiteSpace(t.description));
            }
        }

        [Fact]
        public void EveryTerritory_ControlledNodes_AreNonEmpty_AndUnique()
        {
            var fileIO = new FileSystemIO();
            var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);

            var seenIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var t in catalog.Territories)
            {
                Assert.True(seenIds.Add(t.id), $"Duplicate territory id found: {t.id}");
                Assert.NotEmpty(t.controlled_nodes);
                Assert.NotEmpty(t.control_points);
            }
        }

        [Fact]
        public void EveryTerritory_ContestedWith_AreValidFactionsAndNotSelf()
        {
            var fileIO = new FileSystemIO();
            var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);

            foreach (var t in catalog.Territories)
            {
                Assert.NotEmpty(t.contested_with);
                foreach (var rival in t.contested_with)
                {
                    Assert.StartsWith("faction_", rival);
                    Assert.NotEqual(t.faction, rival);
                }
            }
        }

        [Fact]
        public void EveryContestedZone_HasAtLeastTwoClaimantFactions()
        {
            var fileIO = new FileSystemIO();
            var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);

            var seenZoneIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var zone in catalog.ContestedZones)
            {
                Assert.True(seenZoneIds.Add(zone.id), $"Duplicate contested zone id: {zone.id}");
                Assert.StartsWith("zone_contested_", zone.id);
                Assert.False(string.IsNullOrWhiteSpace(zone.name));
                Assert.False(string.IsNullOrWhiteSpace(zone.strategic_value));
                Assert.StartsWith("loc_", zone.focal_node_id);
                Assert.StartsWith("loc_", zone.focal_location_id);
                Assert.True(zone.claimant_factions.Count >= 2, $"Zone {zone.id} must have at least 2 claimants");
                Assert.InRange(zone.hazard_rating, 1, 5);
                Assert.InRange(zone.dispute_intensity, 0, 100);
            }
        }

        [Fact]
        public void LookupsByIdAndFaction_WorkCorrectly()
        {
            var fileIO = new FileSystemIO();
            var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);

            Assert.True(catalog.TryGetTerritory("territory_the_office", out var officeTerritory));
            Assert.Equal("faction_the_office", officeTerritory.faction);

            Assert.True(catalog.TryGetTerritoryByFaction("faction_hydro_barons", out var hydroTerritory));
            Assert.Equal("territory_hydro_barons", hydroTerritory.id);

            Assert.True(catalog.TryGetContestedZone("zone_contested_water_rights", out var waterZone));
            Assert.Contains("faction_hydro_barons", waterZone.claimant_factions);

            Assert.False(catalog.TryGetTerritory("nonexistent_territory", out _));
            Assert.False(catalog.TryGetTerritoryByFaction("nonexistent_faction", out _));
            Assert.False(catalog.TryGetContestedZone("nonexistent_zone", out _));
        }

        [Fact]
        public void GetTerritoriesForNode_ReturnsMatchingTerritories()
        {
            var fileIO = new FileSystemIO();
            var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);

            var depotTerritories = catalog.GetTerritoriesForNode("loc_cut_abandoned_depot").ToList();
            Assert.NotEmpty(depotTerritories);
            Assert.Contains(depotTerritories, t => t.faction == "faction_deserter_coalition");
            Assert.Contains(depotTerritories, t => t.faction == "faction_undertow");
            Assert.Contains(depotTerritories, t => t.faction == "faction_iron_raiders");
        }
    }
}
