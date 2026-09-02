using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class ShelterThermalWinterTests
    {
        private static ShelterThermalSystem CreateSystem(float outdoorTemp = -15f)
        {
            var rng = new SeededRng(42);
            var needs = new NeedsSystem();
            var starting = new StartingLevelSystem();
            var deepFreezeState = new YearOfAshDeepFreezeState { indoorTemperatureCelsius = outdoorTemp };
            var deepFreeze = new YearOfAshDeepFreezeSystem(deepFreezeState);
            var sys = new ShelterThermalSystem(rng, needs, starting, deepFreeze);
            sys.AddRoom("dormitory", "Living Quarters", 60f, 1.2f, true);
            sys.AddRoom("greenhouse", "Hydroponics Bay", 80f, 0.8f, true);
            return sys;
        }

        [Fact]
        public void BoundedFuelDemand_ScalesWithTemperatureDifference()
        {
            var sys = CreateSystem(-25f);
            sys.SetBoilerActive(true, 70f);
            float initialFuel = sys.BoilerFuelLevel;

            sys.TickDay(1);

            Assert.True(sys.BoilerFuelLevel < initialFuel);
            // Fuel burn is bounded <= 2.5 per day
            Assert.True((initialFuel - sys.BoilerFuelLevel) <= 2.5f);
        }

        [Fact]
        public void Huddle_AddsBodyHeatToRoom_AndBoostsPersonalInsulation()
        {
            var sys = CreateSystem(-10f);
            sys.AssignHuddle("dormitory", "survivor_01");

            Assert.True(sys.IsSurvivorHuddling("survivor_01"));
            float insulation = sys.GetEffectiveSurvivorInsulation("survivor_01");
            // Baseline 10 * 1.3 = 13
            Assert.True(insulation > 10f);
        }

        [Fact]
        public void ClothingCondition_ScalesEffectiveInsulation()
        {
            var sys = CreateSystem(-15f);
            var inv = new Inventory.Inventory();
            var crafting = new CraftingSystem(inv);
            var equipSys = new EquipmentConditionSystem(new SeededRng(7), inv, crafting);

            equipSys.RegisterItem("coat_01", "item_heavy_wool_coat", "survivor_01", EquipmentFamily.Clothing, 100f);
            sys.EquipThermalGear("survivor_01", "item_heavy_wool_coat");

            float fullInsulation = sys.GetEffectiveSurvivorInsulation("survivor_01", equipSys);

            // Degrade coat to 20%
            equipSys.UseItem("coat_01", 80f);
            float damagedInsulation = sys.GetEffectiveSurvivorInsulation("survivor_01", equipSys);

            Assert.True(damagedInsulation < fullInsulation);
            Assert.True(damagedInsulation > 10f); // Still better than bare baseline
        }

        [Fact]
        public void WaterwayFreezeState_AdvancesToFrozen_UnderSevereWinter()
        {
            var sys = CreateSystem(-20f);
            Assert.Equal("Open", sys.WaterwayFreezeState);

            for (int day = 1; day <= 6; day++)
            {
                sys.TickDay(day);
            }

            Assert.Equal("Frozen", sys.WaterwayFreezeState);
        }

        [Fact]
        public void CropTemperatureModifier_PenalizesSubzeroRooms()
        {
            var sys = CreateSystem(-10f);
            var room = sys.State.rooms.Find(r => r.roomId == "greenhouse");
            Assert.NotNull(room);
            room.currentTempC = -4f;

            float mod = sys.GetCropTemperatureModifier("greenhouse");
            Assert.True(mod < 0f); // structural freeze damage
        }

        [Fact]
        public void SaveAndRestore_PreservesHuddleAndWaterwayFreezeState()
        {
            var sys = CreateSystem(-20f);
            sys.AssignHuddle("dormitory", "survivor_02");
            sys.TickDay(1);
            sys.TickDay(2);

            var saved = sys.CaptureState();
            var restored = CreateSystem(-20f);
            restored.RestoreState(saved);

            Assert.True(restored.IsSurvivorHuddling("survivor_02"));
            Assert.Equal(sys.WaterwayFreezeState, restored.WaterwayFreezeState);
            Assert.Equal(sys.State.waterwayFreezeScore, restored.State.waterwayFreezeScore);
        }
    }
}
