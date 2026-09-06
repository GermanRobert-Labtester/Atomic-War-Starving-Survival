// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    public class CaravanPatrolIntegrationTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;

        public CaravanPatrolIntegrationTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _fileIO = new FileSystemIO();
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
        }

        [Fact]
        public void ConvoyPatrols_CanBeSelected_DuringCaravanTravel()
        {
            var inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 1000f };
            var travelSys = new TravelEncounterSystem(_catalog, inv);
            var caravanSys = new TravelingCaravanSystem
            {
                TravelEncounters = travelSys
            };

            var route = new List<string> { "loc_toll_house", "pass_mount_karkov" };
            caravanSys.SpawnCaravan("caravan_iron_line", "Iron Line Haulers", "faction_railway_guild", route, "foundry");
            var caravan = caravanSys.State.activeCaravans[0];

            // In the_toll, railway convoy is eligible
            var railwayConvoy = _catalog.GetEncounter("enc_patrol_railway_convoy")!;
            Assert.NotNull(railwayConvoy);
            Assert.True(travelSys.IsEncounterEligible(railwayConvoy, "the_toll", 2.0f, "all", 1, caravan.currentNodeId));

            // Hydro escort in dead_suburbs
            var hydroEscort = _catalog.GetEncounter("enc_patrol_hydro_escort")!;
            Assert.NotNull(hydroEscort);
            Assert.True(travelSys.IsEncounterEligible(hydroEscort, "dead_suburbs", 2.0f, "all", 1));

            // Supply corps convoy in high_scarp
            var supplyCorps = _catalog.GetEncounter("enc_patrol_supply_corps_convoy")!;
            Assert.NotNull(supplyCorps);
            Assert.True(travelSys.IsEncounterEligible(supplyCorps, "high_scarp", 1.5f, "all", 1));

            // Selection context with Caravan mode rolls encounters
            bool rolledPatrol = false;
            for (int seed = 1; seed <= 50; seed++)
            {
                var enc = caravanSys.CheckRouteEncounter(caravan, "the_toll", 2, "all", 1, new SeededRng(seed));
                if (enc != null && enc.Id.StartsWith("enc_patrol_"))
                {
                    rolledPatrol = true;
                    break;
                }
            }
            Assert.True(rolledPatrol, "Caravan route travel should roll patrol encounters across seeds in valid territory.");
        }

        [Fact]
        public void CaravanAndTravel_ShareCooldownTable()
        {
            var inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 1000f };
            var travelSys = new TravelEncounterSystem(_catalog, inv);
            var caravanSys = new TravelingCaravanSystem
            {
                TravelEncounters = travelSys
            };

            var route = new List<string> { "loc_toll_house", "pass_mount_karkov" };
            caravanSys.SpawnCaravan("caravan_test", "Test Caravan", "faction_railway_guild", route);

            var enc = _catalog.GetEncounter("enc_patrol_railway_convoy")!;
            Assert.NotNull(enc);

            // 1. Caravan resolves encounter on Day 10
            bool ok = caravanSys.ResolveRouteEncounterChoice(enc.Id, enc.Choices[0].ChoiceId, 10, out var res);
            Assert.True(ok);
            Assert.NotNull(res);

            // 2. Both Caravan and Travel should find it on cooldown on Day 11 (5-day cooldown -> day 15)
            Assert.False(travelSys.IsEncounterEligible(enc, "the_toll", 2.0f, "all", 11));

            var caravanContext = TravelEncounterSelectionContext.From(
                "the_toll", 2, "Balanced", "all", 11, WeatherKind.Clear,
                mode: TravelMode.Caravan, locationId: "loc_toll_house");
            Assert.False(travelSys.IsEncounterEligible(enc, caravanContext));

            // 3. Cooldown expires on Day 15
            Assert.True(travelSys.IsEncounterEligible(enc, "the_toll", 2.0f, "all", 15));
            var caravanContextDay15 = TravelEncounterSelectionContext.From(
                "the_toll", 2, "Balanced", "all", 15, WeatherKind.Clear,
                mode: TravelMode.Caravan, locationId: "loc_toll_house");
            Assert.True(travelSys.IsEncounterEligible(enc, caravanContextDay15));
        }

        [Fact]
        public void CaravanAndTravel_ShareChoiceResolutionSemantics()
        {
            var inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 1000f };
            inv.TryProduce("fuel", 10);
            var warSys = new FactionWarSystem();
            var travelSys = new TravelEncounterSystem(_catalog, inv, warSys);
            var caravanSys = new TravelingCaravanSystem
            {
                TravelEncounters = travelSys
            };

            var enc = _catalog.GetEncounter("enc_patrol_railway_convoy")!;
            // Choice 1: Trade fuel for parts (costs 1 fuel, +1 standing, +1 morale)
            var tradeChoice = enc.Choices.Find(c => c.ChoiceId == "choice_guild_trade_parts")!;
            Assert.NotNull(tradeChoice);

            int startFuel = inv.CountById("fuel");
            int startStanding = warSys.GetStanding("faction_railway_guild");

            bool resolved = caravanSys.ResolveRouteEncounterChoice(enc.Id, tradeChoice.ChoiceId, 5, out var result);
            Assert.True(resolved);
            Assert.NotNull(result);

            // Verified choice effects:
            Assert.Equal(1, result.MoraleDelta);
            Assert.Equal(0, result.GuiltDelta);
            Assert.Equal(1, result.FactionStandingDelta);
            Assert.Equal(startFuel - 1, inv.CountById("fuel"));
            Assert.Equal(startStanding + 1, warSys.GetStanding("faction_railway_guild"));
        }

        [Fact]
        public void CaravanPatrol_EventInvocation_OnRouteEncounter()
        {
            var inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 1000f };
            var travelSys = new TravelEncounterSystem(_catalog, inv);
            var caravanSys = new TravelingCaravanSystem
            {
                TravelEncounters = travelSys
            };

            var route = new List<string> { "loc_toll_house", "pass_mount_karkov" };
            caravanSys.SpawnCaravan("caravan_event_test", "Event Caravan", "faction_railway_guild", route);
            var caravan = caravanSys.State.activeCaravans[0];

            CaravanEntry? eventCaravan = null;
            TravelEncounterDefinition? eventEncounter = null;

            caravanSys.OnCaravanPatrolEncountered += (c, e) =>
            {
                eventCaravan = c;
                eventEncounter = e;
            };

            var rng = new SeededRng(999);
            var enc = caravanSys.CheckRouteEncounter(caravan, "the_toll", 2, "all", 1, rng);
            if (enc != null)
            {
                Assert.Same(caravan, eventCaravan);
                Assert.Same(enc, eventEncounter);
            }
        }
    }
}
