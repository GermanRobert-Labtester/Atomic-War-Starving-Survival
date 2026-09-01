using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class HoldfastCatalogTests
    : CatalogTestBase{
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void LocationIdsUniqueSnakeCase()
        {
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            Assert.True(catalog.Locations.Count >= 11, "Cut spine plus Salt/Cluster/Shelf cards");
            var set = new System.Collections.Generic.HashSet<string>();
            for (int i = 0; i < catalog.Locations.Count; i++)
            {
                var e = catalog.Locations[i];
                Assert.False(string.IsNullOrEmpty(e.id));
                Assert.True(set.Add(e.id), "duplicate " + e.id);
                Assert.Equal(e.id, e.id.ToLowerInvariant());
            }
            Assert.NotNull(catalog.GetLocation("loc_ice_road_gate"));
            Assert.NotNull(catalog.GetLocation("loc_cut_kilometre_19"));
            Assert.NotNull(catalog.GetLocation("loc_cut_waystation_a"));
        }

        [Fact]
        public void TenMainQuestsRegistered()
        {
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            Assert.True(catalog.Quests.Count >= 10);
            Assert.NotNull(catalog.GetQuest("quest_holdfast_the_sheet"));
            Assert.NotNull(catalog.GetQuest("quest_holdfast_the_hatch"));
        }

        [Fact]
        public void RecastsAreAlwaysOn()
        {
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            var plant = catalog.GetLocation("location_abandoned_desalination");
            Assert.NotNull(plant);
            Assert.True(plant.recast_always);
            Assert.Contains("Occupied", plant.inspect);
        }
    }

    public class IceRoadHeadlessDemoTests
    {
        [Fact]
        public void HeadlessDemoPassesWithCatalogs()
        {
            string start = Directory.GetCurrentDirectory();
            Assert.True(
                CatalogLocator.TryFindDataDirectory(start, out string data)
                || CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out data));
            var report = IceRoadHeadlessDemo.Run(data);
            Assert.True(report.Passed, report.Summary);
            Assert.True(report.LocationCount >= 11);
            Assert.True(report.QuestCount >= 10);
        }

        [Fact]
        public void Loader_PopulatesItemsAndFactions()
        {
            // Guards against the loader dropping the items/factions loads
            // (items were empty until items/factions loading was wired in).
            var data = TestDataDir();
            // Guards against the loader dropping the items/factions loads
            // (items were empty until items/factions loading was wired in).
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(data);
            Assert.NotNull(catalog.Items);
            Assert.True(catalog.Items.IsValid, "Holdfast items must load");
            // 40 ice-road items + 15 debt principal items (creditor trade).
            Assert.Equal(55, catalog.Items.Count);
            Assert.NotNull(catalog.GetItem("item_triplicate_carbon"));
            Assert.NotNull(catalog.GetItem("item_fume_rag"));
            Assert.NotNull(catalog.GetItem("item_canned_food"));
            Assert.NotNull(catalog.GetItem("item_diesel_fuel"));

            Assert.NotNull(catalog.Factions);
            Assert.True(catalog.Factions.Count > 0, "Holdfast factions must load");
            Assert.NotNull(catalog.GetFaction("faction_the_office"));
            Assert.NotNull(catalog.GetFaction("faction_the_cutters"));
        }

        private static string TestDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }
    }
}
