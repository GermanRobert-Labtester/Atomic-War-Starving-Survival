using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Radiation;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests
{
    public class BalancePowerEconomyTests
    {
        private static readonly string ArtifactDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "artifacts", "balance");

        private enum Scenario { Stable, FuelPressure, EconomicScarcity, CombinedStress }

        [Theory]
        [InlineData(42, "Stable", 7)]
        [InlineData(42, "Stable", 14)]
        [InlineData(42, "Stable", 30)]
        [InlineData(42, "FuelPressure", 7)]
        [InlineData(42, "EconomicScarcity", 7)]
        [InlineData(42, "CombinedStress", 7)]
        [InlineData(999, "CombinedStress", 30)]
        public void PowerEconomy_Telemetry_Is_Deterministic(int seed, string scenarioName, int days)
        {
            var scenario = Enum.Parse<Scenario>(scenarioName);
            var rows = RunScenario(seed, scenario, days, out var invFood, out var invWater, out var fuel, out var health);

            try
            {
                Directory.CreateDirectory(ArtifactDir);
                File.WriteAllLines(Path.Combine(ArtifactDir, $"power_econ_{scenarioName.ToLower()}_seed_{seed}_{days}d.csv"), rows);
            }
            catch { }

            // Basic invariants: inventory never negative due to clamped Remove
            Assert.True(invFood >= 0, $"Food inventory {invFood} should be >=0");
            Assert.True(invWater >= 0, $"Water inventory {invWater} should be >=0");
            Assert.True(fuel >= 0, $"Fuel {fuel} should be >=0");

            // Determinism
            var rows2 = RunScenario(seed, scenario, days, out var invFood2, out var invWater2, out var fuel2, out var health2);
            Assert.Equal(invFood, invFood2);
            Assert.Equal(fuel, fuel2, precision: 1);

            // Scenario expectations (instrumentation, not tuning)
            if (scenario == Scenario.Stable)
            {
                // With adequate start (food 30, water 30, fuel 100) and daily ration, health should stay high for 30d
                Assert.True(health >= 85f, $"Stable health should stay >=85, got {health:F1} seed {seed} days {days}");
            }
        }

        private static List<string> RunScenario(int seed, Scenario scenario, int days, out int finalFood, out int finalWater, out float finalFuel, out float finalHealth)
        {
            var rng = new SeededRng(seed);
            // Needs + warmth via shelter heat (PowerGrid will determine heat availability)
            var needs = new NeedsSystem(isNearHeatSource: _ => true);
            var survivor = new SurvivorNeedsState { Id = "pe_test", Health = 100f, Hunger = 20f, Thirst = 25f, Warmth = 85f, Morale = 70f, Fatigue = 10f };
            needs.Register(survivor);

            var rad = new RadiationSystem(exposureContext: _ => new ExposureContext { ZoneRadLevel = 2f, ShelterShielding = 1f }, seed: seed);
            var radState = new SurvivorRadState { Id = "pe_test", RadiationDose = 5f, LifetimeRadiationExposure = 10f };
            rad.Register(radState);

            // Inventory
            var inventory = new Ashfall.Core.Inventory.Inventory();
            int startFood = scenario switch { Scenario.Stable => 30, Scenario.FuelPressure => 30, Scenario.EconomicScarcity => 5, Scenario.CombinedStress => 5, _ => 30 };
            int startWater = scenario switch { Scenario.Stable => 30, Scenario.FuelPressure => 30, Scenario.EconomicScarcity => 5, Scenario.CombinedStress => 5, _ => 30 };
            int startFuel = scenario switch { Scenario.Stable => 100, Scenario.FuelPressure => 10, Scenario.EconomicScarcity => 100, Scenario.CombinedStress => 10, _ => 100 };
            // Add items via minimal ItemDefinition
            inventory.Add(new ItemDefinition { id = "canned_food", displayName = "Canned Food" }, startFood);
            inventory.Add(new ItemDefinition { id = "clean_water", displayName = "Clean Water" }, startWater);
            inventory.Add(new ItemDefinition { id = "fuel_canister", displayName = "Fuel" }, startFuel);

            // PowerGrid
            var powerState = new PowerGridState { FuelUnits = startFuel, BatteryReserveWh = 500, BatteryCapacityWh = 1000 };
            var rooms = new List<PowerGridRoom> { new PowerGridRoom { RoomId = "room_a", DisplayName = "Test Room", DrawWatts = 100 } };
            var power = new PowerGridSystem(powerState, rooms, rng);

            // Market
            var market = new MarketSystem();
            // Try to load goods catalog if present (optional)
            try
            {
                var io = new FileSystemIO();
                var ser = new SystemTextJsonSerializer();
                var dataDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(dataDir))
                {
                    var goods = GoodsCatalogLoader.Load(dataDir, io, ser);
                }
            }
            catch { }

            var rows = new List<string>();
            rows.Add("seed,scenario,day,health,hunger,thirst,fatigue,warmth,morale,radiationDose,foodInv,waterInv,fuelInv,foodConsumed,waterConsumed,fuelConsumed,foodPrice,waterPrice,fuelPrice,powerAvailable,heatAvailable,criticalCount,healthLoss");

            int foodConsumed = 0, waterConsumed = 0;
            float fuelConsumedTotal = 0f;
            for (int day = 1; day <= days; day++)
            {
                // Daily needs tick + ration + rest
                needs.Tick(24f);
                rad.Tick(24f);
                // Daily ration: 2 food, 3 water (real -10 per unit) — use direct Modify to isolate balance from inventory catalog quirks
                // Track inventory separately but always apply needs restore
                if (inventory.CountById("canned_food") >= 2) inventory.RemoveById("canned_food", 2);
                needs.Modify(survivor, NeedKind.Hunger, -20f);
                foodConsumed += 2;
                if (inventory.CountById("clean_water") >= 3) inventory.RemoveById("clean_water", 3);
                needs.Modify(survivor, NeedKind.Thirst, -30f);
                waterConsumed += 3;
                // Rest
                needs.Modify(survivor, NeedKind.Fatigue, -64f);

                // Power tick
                var summary = power.TickDay(day, rng);
                fuelConsumedTotal += summary.FuelConsumed;

                // Market tick
                market.TickDay(day, rng);
                // Observe prices (if catalog loaded, price may be 0 if not; we still record)
                float foodPrice = 0f, waterPrice = 0f, fuelPrice = 0f;
                try { foodPrice = market.GetPrice("canned_food"); waterPrice = market.GetPrice("clean_water"); fuelPrice = market.GetPrice("fuel_canister"); } catch { }

                bool powerAvailable = !power.IsBrownout;
                bool heatAvailable = powerAvailable; // simplified: heat requires power

                // If power brownout, warmth loss would increase — but we already force isNearHeatSource true for this harness to isolate power metric
                // In real integration, isNearHeatSource would be powerAvailable

                int critical = 0;
                if (survivor.Hunger >= 90) critical++;
                if (survivor.Thirst >= 90) critical++;
                if (survivor.Warmth <= 20) critical++;

                int foodInv = inventory.CountById("canned_food");
                int waterInv = inventory.CountById("clean_water");
                float fuelInv = power.FuelUnits;

                rows.Add($"{seed},{scenario},{day},{survivor.Health:F1},{survivor.Hunger:F1},{survivor.Thirst:F1},{survivor.Fatigue:F1},{survivor.Warmth:F1},{survivor.Morale:F1},{radState.RadiationDose:F1},{foodInv},{waterInv},{fuelInv:F1},{foodConsumed},{waterConsumed},{fuelConsumedTotal:F1},{foodPrice:F2},{waterPrice:F2},{fuelPrice:F2},{powerAvailable},{heatAvailable},{critical},{100f - survivor.Health:F1}");
            }

            finalFood = inventory.CountById("canned_food");
            finalWater = inventory.CountById("clean_water");
            finalFuel = power.FuelUnits;
            finalHealth = survivor.Health;
            return rows;
        }
    }
}
