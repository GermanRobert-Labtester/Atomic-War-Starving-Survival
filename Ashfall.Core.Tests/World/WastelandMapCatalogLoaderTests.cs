using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class WastelandMapCatalogLoaderTests
    {
        private static string GetDataDir()
        {
            return Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void Load_FromDisk_LoadsAllNodesAndRoutes()
        {
            string dataDir = GetDataDir();
            var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);

            Assert.NotEmpty(nodes);
            Assert.Equal(6, nodes.Count);
            Assert.NotEmpty(routes);
            Assert.Equal(7, routes.Count);
        }

        [Fact]
        public void Load_ParsesDangerEnumsCorrectly()
        {
            string dataDir = GetDataDir();
            var (nodes, _) = WastelandMapCatalogLoader.Load(dataDir);

            var holdfast = nodes.FirstOrDefault(n => n.Id == "loc_holdfast");
            Assert.NotNull(holdfast);
            Assert.Equal(MapNodeDanger.None, holdfast.Danger);
            Assert.True(holdfast.StartingUnlocked);

            var depot = nodes.FirstOrDefault(n => n.Id == "loc_cut_abandoned_depot");
            Assert.NotNull(depot);
            Assert.Equal(MapNodeDanger.Low, depot.Danger);

            var arsenal = nodes.FirstOrDefault(n => n.Id == "loc_cut_arsenal_ruin");
            Assert.NotNull(arsenal);
            Assert.Equal(MapNodeDanger.Medium, arsenal.Danger);

            var radZone = nodes.FirstOrDefault(n => n.Id == "loc_cut_radiation_zone_alpha");
            Assert.NotNull(radZone);
            Assert.Equal(MapNodeDanger.High, radZone.Danger);

            var flotilla = nodes.FirstOrDefault(n => n.Id == "loc_black_flotilla_outpost");
            Assert.NotNull(flotilla);
            Assert.Equal(MapNodeDanger.Locked, flotilla.Danger);
        }

        [Fact]
        public void CreateSystem_InitializesWastelandMapSystemWithValidRoutes()
        {
            string dataDir = GetDataDir();
            var system = WastelandMapCatalogLoader.CreateSystem(dataDir);

            Assert.NotNull(system);
            Assert.Equal(6, system.Nodes.Count);
            Assert.Equal(7, system.Routes.Count);

            // Starting unlocked node is discovered automatically
            Assert.True(system.IsDiscovered("loc_holdfast"));
            Assert.True(system.IsDiscovered("loc_cut_merchant_caravanserai"));

            // Undiscovered by default
            Assert.False(system.IsDiscovered("loc_cut_abandoned_depot"));

            // Discover and plan route
            Assert.True(system.Discover("loc_cut_abandoned_depot"));
            var route = system.PlanRoute("loc_holdfast", "loc_cut_abandoned_depot");
            Assert.NotEmpty(route);
            Assert.Equal("loc_holdfast", route.First());
            Assert.Equal("loc_cut_abandoned_depot", route.Last());
        }

        [Fact]
        public void EmptyDataDir_FallbackSystemCreatedSafely()
        {
            var system = WastelandMapCatalogLoader.CreateSystem(string.Empty);
            Assert.NotNull(system);
            Assert.Single(system.Nodes);
            Assert.Equal("loc_holdfast", system.Nodes[0].Id);
        }
    }
}
