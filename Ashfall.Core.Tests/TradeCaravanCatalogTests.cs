using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class TradeCaravanCatalogTests
    : CatalogTestBase{
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void TradeCaravans_LoadsAll18CanonicalRoutes()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "wasteland_trade_caravan_routes.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new TradeCaravanCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(18, catalog.AllRoutes.Count);

            // Test first route (The Iron Line Express)
            var r1 = catalog.GetById("route_01_the_iron_line_garrison_express");
            Assert.NotNull(r1);
            Assert.Equal("The Iron Line Express (Main Railway Spur)", r1.route_name);
            Assert.Equal(2.5f, r1.travel_days);
            Assert.Equal(2, r1.hazard_index);
            Assert.Contains("7.62x54mm reloaded ammunition", r1.primary_cargo_manifest);
            Assert.Contains("Armored draisine tractor", r1.caravan_master_log);

            // Test hub connection query (Republic)
            var republicRoutes = catalog.GetRoutesFromHub("settlement_20_the_valley_sunken_atrium_republic");
            Assert.True(republicRoutes.Count >= 4); // Grain Haul, Dam Battery Relay, Subway Metro, Winter Eden, Brewery

            // Test safe low hazard routes
            var safeRoutes = catalog.GetLowHazardSafeRoutes(2);
            Assert.True(safeRoutes.Count >= 7);

            // Test final route (The Great Brewery and Yeast Caravan)
            var r18 = catalog.GetById("route_18_the_great_brewery_and_yeast_caravan");
            Assert.NotNull(r18);
            Assert.Equal(1, r18.hazard_index);
            Assert.Contains("vital vitamin B yeast cakes and morale beer", r18.caravan_master_log);

            // Test tag search
            var canal = catalog.GetByTag("canal");
            Assert.True(canal.Count >= 2);
        }

        [Fact]
        public void TradeCaravans_AllEntriesHaveValidFieldsAndWaypoints()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "wasteland_trade_caravan_routes.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new TradeCaravanCatalog();
            catalog.Load(json, serializer);

            foreach (var r in catalog.AllRoutes)
            {
                Assert.False(string.IsNullOrWhiteSpace(r.route_id), "Missing route_id");
                Assert.False(string.IsNullOrWhiteSpace(r.route_name), $"Missing route_name on {r.route_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.origin_hub), $"Missing origin_hub on {r.route_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.destination_hub), $"Missing destination_hub on {r.route_id}");
                Assert.True(r.travel_days > 0f, $"Invalid travel days on {r.route_id}");
                Assert.InRange(r.hazard_index, 1, 5);
                Assert.False(string.IsNullOrWhiteSpace(r.primary_cargo_manifest), $"Missing cargo on {r.route_id}");
                Assert.NotNull(r.key_waypoints);
                Assert.True(r.key_waypoints.Length >= 3, $"Waypoints too short on {r.route_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.caravan_master_log), $"Missing caravan log on {r.route_id}");
                Assert.True(r.caravan_master_log.Length > 30, $"Caravan log too brief on {r.route_id}");
                Assert.NotNull(r.tags);
                Assert.True(r.tags.Length > 0, $"Tags empty on {r.route_id}");
            }
        }
    }
}
