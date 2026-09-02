using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Recreation;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public sealed class Plans194_197_CampaignContinuityTests
    {
        [Fact]
        public void WinterFreeze_DynamicallyBlocksNavalNavigation()
        {
            var rng = new SeededRng(100);
            var needs = new NeedsSystem();
            var starting = new StartingLevelSystem();
            var deepFreezeState = new YearOfAshDeepFreezeState { indoorTemperatureCelsius = -22f };
            var deepFreeze = new YearOfAshDeepFreezeSystem(deepFreezeState);
            var thermalSys = new ShelterThermalSystem(rng, needs, starting, deepFreeze);

            var navalSys = new ExpeditionNavalSystem();
            var vessel = navalSys.CreateInstance("vessel_motorboat");
            var route = new MapRoute
            {
                From = "loc_holdfast",
                To = "loc_river_delta",
                DistanceKm = 35f,
                TravelDomain = "water"
            };

            // Day 0: Open water allows swift navigation
            var estOpen = navalSys.EstimateRoute(vessel, route, thermalSys.WaterwayFreezeState);
            Assert.False(estOpen.isClosedByIce);
            Assert.True(estOpen.effectiveSpeedKmH > 20f);

            // Advance winter by 5 days -> severe freeze accumulates
            for (int day = 1; day <= 5; day++)
            {
                thermalSys.TickDay(day);
            }

            Assert.Equal("Frozen", thermalSys.WaterwayFreezeState);

            // Day 5: Waterway is frozen solid, blocking naval navigation
            var estFrozen = navalSys.EstimateRoute(vessel, route, thermalSys.WaterwayFreezeState);
            Assert.True(estFrozen.isClosedByIce);
            Assert.Equal(0f, estFrozen.effectiveSpeedKmH);
        }

        [Fact]
        public void EquipmentDegradation_ImpairsThermalProtectionInFreezingRooms()
        {
            var rng = new SeededRng(101);
            var inv = new Inventory.Inventory();
            var crafting = new CraftingSystem(inv);
            var equipSys = new EquipmentConditionSystem(rng, inv, crafting);

            var needs = new NeedsSystem();
            var starting = new StartingLevelSystem();
            var deepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState { indoorTemperatureCelsius = -18f });
            var thermalSys = new ShelterThermalSystem(rng, needs, starting, deepFreeze);

            equipSys.RegisterItem("parka_01", "item_thermal_parka", "survivor_01", EquipmentFamily.Clothing, 100f);
            thermalSys.EquipThermalGear("survivor_01", "item_thermal_parka");

            float pristineInsulation = thermalSys.GetEffectiveSurvivorInsulation("survivor_01", equipSys);

            // Parka suffers heavy field wear down to 15% condition
            equipSys.ApplyWear("parka_01", new WearEvent { intensity = 50f, environmentModifier = 1.7f });
            float damagedInsulation = thermalSys.GetEffectiveSurvivorInsulation("survivor_01", equipSys);

            Assert.True(damagedInsulation < pristineInsulation);
            // Survivor attempts field jury-rigging with scrap
            inv.AddById("scrap_metal", 2);
            equipSys.JuryRig("parka_01", new List<string> { "scrap_metal" });
            float patchedInsulation = thermalSys.GetEffectiveSurvivorInsulation("survivor_01", equipSys);

            Assert.True(patchedInsulation > damagedInsulation);
        }

        [Fact]
        public void RecreationDowntime_ConsumesCraftingScrap_AndRestoresMorale()
        {
            var rng = new SeededRng(102);
            var inv = new Inventory.Inventory();
            var needs = new NeedsSystem();
            var downtimeSys = new SurvivorDowntimeSystem(rng, inv, needs);

            inv.AddById("scrap_wood", 3);
            var startRes = downtimeSys.StartSession("hobby_whittling", "room_workshop", new List<string> { "survivor_01" });
            Assert.Equal(ActionResult.StatusKind.Success, startRes.Status);

            downtimeSys.TickDay(1);

            Assert.True(inv.CountById("item_carved_figurine") >= 1);
            var prof = downtimeSys.GetOrCreateProfile("survivor_01");
            Assert.True(prof.stressRelievedTotal >= 15f);
        }
    }
}
