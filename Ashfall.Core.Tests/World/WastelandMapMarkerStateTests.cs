using System.IO;
using System.Linq;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class WastelandMapMarkerStateTests
    {
        private static string GetDataDir()
        {
            return Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void InitialState_StartingNodesAreDiscovered()
        {
            string dataDir = GetDataDir();
            var system = WastelandMapCatalogLoader.CreateSystem(dataDir);

            // Starting unlocked nodes are discovered
            Assert.True(system.IsDiscovered("loc_holdfast"));
            Assert.True(system.IsDiscovered("loc_cut_merchant_caravanserai"));

            // Other nodes are not yet discovered
            Assert.False(system.IsDiscovered("loc_cut_abandoned_depot"));
            Assert.False(system.IsDiscovered("loc_cut_radiation_zone_alpha"));
            Assert.False(system.IsDiscovered("loc_cut_arsenal_ruin"));
            Assert.False(system.IsDiscovered("loc_black_flotilla_outpost"));
        }

        [Fact]
        public void RouteConnectivity_IdentifiesAdjacentAvailableNodes()
        {
            string dataDir = GetDataDir();
            var system = WastelandMapCatalogLoader.CreateSystem(dataDir);

            // Abandoned Depot connects to Holdfast (discovered) -> Available
            bool depotReachable = system.Routes.Any(r =>
                (r.To == "loc_cut_abandoned_depot" && system.IsDiscovered(r.From)) ||
                (r.From == "loc_cut_abandoned_depot" && system.IsDiscovered(r.To)));
            Assert.True(depotReachable);

            // Arsenal Ruin connects to Abandoned Depot (undiscovered) -> Unavailable (deep fog of war)
            bool arsenalReachable = system.Routes.Any(r =>
                (r.To == "loc_cut_arsenal_ruin" && system.IsDiscovered(r.From)) ||
                (r.From == "loc_cut_arsenal_ruin" && system.IsDiscovered(r.To)));
            Assert.False(arsenalReachable);

            // Discovering Abandoned Depot unlocks Arsenal Ruin to Available status
            system.Discover("loc_cut_abandoned_depot");
            bool arsenalNowReachable = system.Routes.Any(r =>
                (r.To == "loc_cut_arsenal_ruin" && system.IsDiscovered(r.From)) ||
                (r.From == "loc_cut_arsenal_ruin" && system.IsDiscovered(r.To)));
            Assert.True(arsenalNowReachable);
        }

        [Fact]
        public void LockedNode_RetainsLockedDangerRating()
        {
            string dataDir = GetDataDir();
            var system = WastelandMapCatalogLoader.CreateSystem(dataDir);

            var flotillaNode = system.GetNode("loc_black_flotilla_outpost");
            Assert.NotNull(flotillaNode);
            Assert.Equal(MapNodeDanger.Locked, flotillaNode.Danger);
        }
    }
}
