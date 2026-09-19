// SPDX-License-Identifier: MIT
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class RegionalTreatyCatalogLoaderTests
    {
        [Fact]
        public void Load_ShippedCatalog_HasMechanicalTreaties()
        {
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dataDir))
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data directory not found.");

            var list = RegionalTreatyCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotEmpty(list);
            Assert.Contains(list, t => t.treaty_id == "road_iron_charter");
            var road = list.Find(t => t.treaty_id == "road_iron_charter");
            Assert.NotNull(road);
            Assert.Equal("hydro_barons", road!.faction_id);
            Assert.True(road.ratification_cost_scrap > 0);
            Assert.NotEmpty(road.effects);
        }

        [Fact]
        public void Load_FeedsRegionalTreatySystem_ProposeSucceeds()
        {
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dataDir))
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data directory not found.");

            var list = RegionalTreatyCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var sys = new RegionalTreatySystem();
            sys.LoadCatalog(list);
            var result = sys.Propose("road_iron_charter");
            Assert.True(result.IsSuccess, result.FailureCode);
        }
    }
}
