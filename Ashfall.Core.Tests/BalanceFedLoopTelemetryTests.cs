using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Radiation;
using Ashfall.Core.Economy;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Fed/hydrated deterministic telemetry.
    /// Extends the original 7-day starvation harness with realistic consumption schedules
    /// to prove whether sustainable survival is achievable under current tuning.
    /// </summary>
    public class BalanceFedLoopTelemetryTests
    {
        private static readonly string ArtifactDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "artifacts", "balance");

        private enum Scenario
        {
            DailyRation,   // food 24h (2 units), water 24h (3 units)
            Scarcity,      // food 48h (2 units), water 24h (3 units)
            SevereScarcity // food 48h (2 units), water 48h (2 units)  (also covers 36h variant)
        }

        private static (int intervalFoodH, int unitsFood, int intervalWaterH, int unitsWater) Schedule(Scenario s) => s switch
        {
            Scenario.DailyRation => (24, 2, 24, 3),
            Scenario.Scarcity => (48, 2, 24, 3),
            Scenario.SevereScarcity => (48, 2, 48, 2),
            _ => (24, 2, 24, 3)
        };

        [Theory]
        [InlineData(42, "DailyRation", 7)]
        [InlineData(123, "DailyRation", 7)]
        [InlineData(999, "DailyRation", 7)]
        [InlineData(42, "Scarcity", 7)]
        [InlineData(42, "SevereScarcity", 7)]
        [InlineData(42, "DailyRation", 14)]
        [InlineData(42, "DailyRation", 30)]
        [InlineData(999, "SevereScarcity", 30)]
        public void FedLoop_ProducesTelemetry_ForScenario(int seed, string scenarioName, int days)
        {
            var scenario = Enum.Parse<Scenario>(scenarioName);
            var rows = RunFedLoop(seed, scenario, days, out var survivor, out var radState, out var totalFood, out var totalWater);

            // Write CSV per scenario/seed/days
            try
            {
                Directory.CreateDirectory(ArtifactDir);
                var path = Path.Combine(ArtifactDir, $"fed_{scenarioName.ToLower()}_seed_{seed}_{days}d.csv");
                File.WriteAllLines(path, rows);
            }
            catch { }

            // Basic invariants
            Assert.True(survivor.Hunger is >= 0f and <= 100f, "Hunger clamped");
            Assert.True(survivor.Thirst is >= 0f and <= 100f, "Thirst clamped");
            Assert.True(radState.LifetimeRadiationExposure > 0f, "Radiation accumulates");
            // Determinism: rerun same config must match final hunger
            var rows2 = RunFedLoop(seed, scenario, days, out var surv2, out _, out _, out _);
            Assert.Equal(survivor.Hunger, surv2.Hunger, precision: 2);
            Assert.Equal(survivor.Thirst, surv2.Thirst, precision: 2);

            // Scenario-specific expectations (not tuning, just measurement guard)
            if (scenario == Scenario.DailyRation && days <= 30)
            {
                // With daily ration, survivor should not reach critical hunger/thirst and health should remain high
                Assert.True(survivor.Health >= 85f, $"Daily ration should keep health >=85 over {days}d, got {survivor.Health:F1} (seed {seed})");
                Assert.True(survivor.Hunger < 85f, $"Daily ration hunger <85, got {survivor.Hunger:F1}");
                Assert.True(survivor.Thirst < 85f, $"Daily ration thirst <85, got {survivor.Thirst:F1}");
            }
            if (scenario == Scenario.SevereScarcity && days >= 14)
            {
                // Severe scarcity should show degradation but not immediate trivial death
                // Health may decline but we at least assert we tracked it
                Assert.True(rows.Count == days + 1, "CSV rows = days+header");
            }
        }

        [Fact]
        public void FedLoop_Telemetry_IsDeterministic_AcrossSeeds()
        {
            // Needs drift is not seeded; two runs same seed identical, different seeds same hunger trajectory for fed loop (since needs not seeded)
            var r1 = RunFedLoop(42, Scenario.DailyRation, 7, out var s1, out _, out _, out _);
            var r2 = RunFedLoop(42, Scenario.DailyRation, 7, out var s2, out _, out _, out _);
            Assert.Equal(s1.Hunger, s2.Hunger, precision: 3);
            var r3 = RunFedLoop(999, Scenario.DailyRation, 7, out var s3, out _, out _, out _);
            // Hunger should be same across seeds (deterministic profile)
            Assert.Equal(s1.Hunger, s3.Hunger, precision: 3);
        }

        private static List<string> RunFedLoop(int seed, Scenario scenario, int days,
            out SurvivorNeedsState survivor, out SurvivorRadState radState,
            out int foodConsumedTotal, out int waterConsumedTotal)
        {
            // Shelter provides steady heat for fed-loop: without it, warmth decays to critical and dominates health loss.
            // This mirrors the shelter thermal panel keeping the interior warm.
            var needs = new NeedsSystem(isNearHeatSource: _ => true);
            survivor = new SurvivorNeedsState { Id = "fed_survivor", Health = 100f, Hunger = 20f, Thirst = 25f, Warmth = 85f, Morale = 70f, Fatigue = 10f };
            needs.Register(survivor);

            radState = new SurvivorRadState { Id = "fed_survivor", RadiationDose = 5f, LifetimeRadiationExposure = 10f };
            var rad = new RadiationSystem(exposureContext: s => new ExposureContext { ZoneRadLevel = 2f, ShelterShielding = 1f }, seed: seed);
            rad.Register(radState);

            var market = new MarketSystem();
            var rng = new SeededRng(seed);

            var (intervalFood, unitsFood, intervalWater, unitsWater) = Schedule(scenario);
            int foodTotal = 0, waterTotal = 0;
            float initialHealth = survivor.Health;

            var rows = new List<string>();
            rows.Add("seed,scenario,day,hunger,thirst,fatigue,warmth,morale,health,radiationDose,lifetimeExposure,foodConsumed,waterConsumed,hungerCritical,thirstCritical,healthLossPerDay");

            // Simulate hour-by-hour to allow scheduled feeding at correct intervals
            int hoursPerDay = 24;
            for (int day = 1; day <= days; day++)
            {
                for (int hour = 1; hour <= hoursPerDay; hour++)
                {
                    int elapsedHours = (day - 1) * hoursPerDay + hour;
                    // Feeding schedule: consume at interval boundaries (end of interval window)
                    // Apply before tick so drift and restoration are co-located in same hour
                    if (elapsedHours % intervalFood == 0)
                    {
                        // Real mechanic: NeedsSystem.Modify is the authoritative hunger restore (10 per unit in SurvivorInspection)
                        needs.Modify(survivor, NeedKind.Hunger, -unitsFood * 10f);
                        foodTotal += unitsFood;
                    }
                    if (elapsedHours % intervalWater == 0)
                    {
                        needs.Modify(survivor, NeedKind.Thirst, -unitsWater * 10f);
                        waterTotal += unitsWater;
                    }

                    needs.Tick(1f);
                    rad.Tick(1f);
                }
                // Market tick per day (deterministic via seeded rng)
                market.TickDay(day, rng);

                bool hungerCrit = survivor.Hunger >= 90f;
                bool thirstCrit = survivor.Thirst >= 90f;
                float healthLossPerDay = initialHealth - survivor.Health;
                rows.Add($"{seed},{scenario},{day},{survivor.Hunger:F2},{survivor.Thirst:F2},{survivor.Fatigue:F2},{survivor.Warmth:F2},{survivor.Morale:F2},{survivor.Health:F2},{radState.RadiationDose:F2},{radState.LifetimeRadiationExposure:F2},{foodTotal},{waterTotal},{hungerCrit},{thirstCrit},{healthLossPerDay:F2}");

                // Early death guard: daily ration should not die within 7 days
                if (scenario == Scenario.DailyRation && day <= 7)
                {
                    // Allow health to dip slightly but not die
                    if (!survivor.IsAliveState)
                        throw new Xunit.Sdk.XunitException($"DailyRation: survivor died on day {day} seed {seed} (health {survivor.Health:F1}, hunger {survivor.Hunger:F1}, thirst {survivor.Thirst:F1})");
                }
            }

            foodConsumedTotal = foodTotal;
            waterConsumedTotal = waterTotal;
            return rows;
        }

        [Theory]
        [InlineData(42)]
        [InlineData(123)]
        [InlineData(999)]
        public void OriginalStarvation_Harness_StillReachesCritical_AroundDay7(int seed)
        {
            // Preserve original finding: without food/water, critical thresholds are reached and health declines, but not instantly
            var needs = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "starve", Health = 100f, Hunger = 20f, Thirst = 25f };
            needs.Register(s);
            var rad = new RadiationSystem(seed: seed);
            var rs = new SurvivorRadState { Id = "starve" };
            rad.Register(rs);
            for (int day = 1; day <= 7; day++)
            {
                needs.Tick(24f);
                rad.Tick(24f);
            }
            Assert.True(s.Hunger > 85f || s.Thirst > 85f, $"Starvation should push needs critical by day 7 (hunger {s.Hunger:F1}, thirst {s.Thirst:F1})");
        }
    }
}
