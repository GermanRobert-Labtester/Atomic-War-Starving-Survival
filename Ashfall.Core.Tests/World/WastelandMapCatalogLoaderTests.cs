// SPDX-License-Identifier: MIT
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
            Assert.True(nodes.Count >= 6);
            Assert.NotEmpty(routes);
            Assert.True(routes.Count >= 7);
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
            Assert.True(system.Nodes.Count >= 6);
            Assert.True(system.Routes.Count >= 7);

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

        [Fact]
        public void AllMapNodes_ExistInLocationsCatalog()
        {
            string dataDir = GetDataDir();
            var (nodes, _) = WastelandMapCatalogLoader.Load(dataDir);
            Assert.NotEmpty(nodes);

            string locationsPath = Path.Combine(dataDir, "locations.json");
            Assert.True(File.Exists(locationsPath), "locations.json must exist");

            string json = File.ReadAllText(locationsPath);
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var locationIds = new System.Collections.Generic.HashSet<string>(System.StringComparer.Ordinal);
            if (doc.RootElement.TryGetProperty("locations", out var locs) && locs.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var loc in locs.EnumerateArray())
                {
                    if (loc.TryGetProperty("id", out var idProp))
                    {
                        var id = idProp.GetString();
                        if (!string.IsNullOrEmpty(id))
                            locationIds.Add(id);
                    }
                }
            }

            var missingNodes = new System.Collections.Generic.List<string>();
            foreach (var node in nodes)
            {
                if (!locationIds.Contains(node.Id))
                {
                    missingNodes.Add(node.Id);
                }
            }

            Assert.True(missingNodes.Count == 0, $"The following {missingNodes.Count} map nodes are missing from locations.json: {string.Join(", ", missingNodes)}");
        }
    }
}
