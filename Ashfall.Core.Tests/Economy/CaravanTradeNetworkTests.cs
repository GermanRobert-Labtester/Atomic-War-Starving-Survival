// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class CaravanTradeNetworkTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            candidate = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void CatalogLoader_LoadsAllTenRoutes()
        {
            string dataDir = GetDataDir();
            var routes = CaravanTradeRouteCatalogLoader.Load(dataDir);
            Assert.NotNull(routes);
            Assert.Equal(10, routes.Count);

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var r in routes)
            {
                Assert.False(string.IsNullOrEmpty(r.route_id));
                Assert.True(seenIds.Add(r.route_id), $"Duplicate route_id: {r.route_id}");
                Assert.True(r.travel_days > 0, "Travel days must be positive");
                Assert.True(r.base_risk_permille >= 0 && r.base_risk_permille <= 1000, "Risk in valid range");
                Assert.NotEmpty(r.import_demands);
                Assert.NotEmpty(r.export_surpluses);
            }
        }

        [Fact]
        public void DynamicPricing_AppliesSurplusDiscountAndDemandPremium()
        {
            var route = new CaravanRouteDefinition
            {
                route_id = "test_route",
                faction_id = "faction_the_compact",
                export_surpluses = new List<string> { "ammo_556" },
                import_demands = new List<string> { "anesthetic_ether" }
            };

            var inv = new Inventory.Inventory();
            var rng = new SeededRng(42);
            var system = new CaravanTradeNetworkSystem(new[] { route }, inv, rng);

            var manifest = system.ScheduleCaravan("test_route", currentDay: 1);

            // ammo_556 canonical value is 12 -> 30% discount = 12 * 0.7 = 8.4
            float buyPrice = system.CalculateItemBuyPrice(manifest, "ammo_556");
            Assert.Equal(8.4f, buyPrice);

            // anesthetic_ether canonical value is 25 -> 50% demand premium = 25 * 1.5 = 37.5
            float sellPrice = system.CalculateItemSellPrice(manifest, "anesthetic_ether");
            Assert.Equal(37.5f, sellPrice);
        }

        [Fact]
        public void FavoredStatus_UnlocksAtFiveTrades_AndGrantsFifteenPercentDiscount()
        {
            var route = new CaravanRouteDefinition
            {
                route_id = "test_route",
                faction_id = "faction_the_compact",
                travel_days = 1,
                export_surpluses = new List<string> { "clean_water" }, // base 4 -> discount 0.7 = 2.8
                import_demands = new List<string> { "scrap_metal" }     // base 3 -> premium 1.5 = 4.5
            };

            var inv = new Inventory.Inventory();
            var rng = new SeededRng(99);
            var system = new CaravanTradeNetworkSystem(new[] { route }, inv, rng);

            var manifest = system.ScheduleCaravan("test_route", 1);
            manifest.status = CaravanStatus.Arrived;
            manifest.stocks["clean_water"] = 50;

            inv.TryProduce("scrap_metal", 100);

            Assert.False(system.HasFavoredStatus("faction_the_compact"));

            // Perform 4 trades
            for (int i = 0; i < 4; i++)
            {
                var res = system.ExecuteBarter(
                    manifest.manifest_id,
                    new Dictionary<string, int> { { "scrap_metal", 2 } },
                    new Dictionary<string, int> { { "clean_water", 1 } });
                Assert.True(res.Success);
                Assert.False(res.UnlockedFavoredStatus);
            }

            Assert.Equal(4, system.GetProfitableTradeCount("faction_the_compact"));
            Assert.False(system.HasFavoredStatus("faction_the_compact"));

            // 5th trade unlocks favored status
            var fifthRes = system.ExecuteBarter(
                manifest.manifest_id,
                new Dictionary<string, int> { { "scrap_metal", 2 } },
                new Dictionary<string, int> { { "clean_water", 1 } });
            Assert.True(fifthRes.Success);
            Assert.True(fifthRes.UnlockedFavoredStatus);
            Assert.True(system.HasFavoredStatus("faction_the_compact"));

            // Check favored status price: 2.8 * (1 - 0.15) = 2.38
            float favoredBuyPrice = system.CalculateItemBuyPrice(manifest, "clean_water");
            Assert.Equal(2.38f, favoredBuyPrice);
        }

        [Fact]
        public void BarterTransaction_IsAtomicOnFailure()
        {
            var route = new CaravanRouteDefinition
            {
                route_id = "test_route",
                faction_id = "faction_the_scale",
                travel_days = 1,
                export_surpluses = new List<string> { "anesthetic_ether" } // base 25 -> 0.7 = 17.5
            };

            var inv = new Inventory.Inventory();
            inv.TryProduce("scrap_wood", 2); // 2 * 2 = 4 value, not enough for 17.5

            var rng = new SeededRng(101);
            var system = new CaravanTradeNetworkSystem(new[] { route }, inv, rng);

            var manifest = system.ScheduleCaravan("test_route", 1);
            manifest.status = CaravanStatus.Arrived;
            manifest.stocks["anesthetic_ether"] = 5;

            var res = system.ExecuteBarter(
                manifest.manifest_id,
                new Dictionary<string, int> { { "scrap_wood", 2 } },
                new Dictionary<string, int> { { "anesthetic_ether", 1 } });

            Assert.False(res.Success);
            Assert.Equal("insufficient_barter_value", res.FailureReason);
            // Verify zero mutation
            Assert.Equal(2, inv.CountById("scrap_wood"));
            Assert.Equal(0, inv.CountById("anesthetic_ether"));
            Assert.Equal(5, manifest.stocks["anesthetic_ether"]);
        }

        [Fact]
        public void Lifecycle_TransitArrivalDeparture_TickExecution()
        {
            var route = new CaravanRouteDefinition
            {
                route_id = "fast_route",
                faction_id = "faction_black_flotilla",
                travel_days = 2,
                export_surpluses = new List<string> { "ammo_9x19" }
            };

            var inv = new Inventory.Inventory();
            var rng = new SeededRng(77);
            var system = new CaravanTradeNetworkSystem(new[] { route }, inv, rng);

            var manifest = system.ScheduleCaravan("fast_route", currentDay: 1);
            Assert.Equal(CaravanStatus.Scheduled, manifest.status);

            // Day 1 tick -> InTransit
            system.TickDay(1);
            Assert.Equal(CaravanStatus.InTransit, manifest.status);

            // Day 2 tick -> Arrived (2 days travel reached)
            system.TickDay(2);
            Assert.Equal(CaravanStatus.Arrived, manifest.status);
            Assert.Equal(2, manifest.actual_arrival_day);
            Assert.Equal(5, manifest.departure_day); // 2 + 3 stay days

            // Advance to day 5 -> Departed
            system.TickDay(3);
            system.TickDay(4);
            system.TickDay(5);
            Assert.Equal(CaravanStatus.Departed, manifest.status);
        }

        [Fact]
        public void Persistence_StateSurvivesRoundTripWithoutHazardReroll()
        {
            var route = new CaravanRouteDefinition
            {
                route_id = "save_route",
                faction_id = "faction_the_compact",
                travel_days = 4,
                export_surpluses = new List<string> { "ammo_556" }
            };

            var inv = new Inventory.Inventory();
            var rng = new SeededRng(55);
            var systemA = new CaravanTradeNetworkSystem(new[] { route }, inv, rng);

            var manifestA = systemA.ScheduleCaravan("save_route", 1);
            systemA.TickDay(1);
            systemA.TickDay(2); // Midpoint reached, hazard resolved

            Assert.True(manifestA.hazard_resolved);
            var originalOutcome = manifestA.hazard_outcome;
            int progress = manifestA.transit_progress_days;

            // Capture state
            var save = systemA.CaptureState();
            Assert.NotNull(save);
            Assert.Single(save.caravans);

            // Restore in fresh system
            var systemB = new CaravanTradeNetworkSystem(new[] { route }, new Inventory.Inventory(), new SeededRng(999));
            systemB.RestoreState(save);

            var manifestB = systemB.FindManifest(manifestA.manifest_id);
            Assert.NotNull(manifestB);
            Assert.Equal(originalOutcome, manifestB.hazard_outcome);
            Assert.True(manifestB.hazard_resolved);
            Assert.Equal(progress, manifestB.transit_progress_days);
        }
    }
}
