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
    /// Deterministic 7-day balance telemetry harness.
    /// Exercises real SurvivorsHost-like needs/radiation + market ticks and writes CSV to artifacts/balance/.
    /// Does not tune values; measures drift.
    /// </summary>
    public class BalanceTelemetryHarnessTests
    {
        private static readonly string ArtifactDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "artifacts", "balance");

        [Theory]
        [InlineData(42)]
        [InlineData(123)]
        [InlineData(999)]
        public void SevenDayHarness_ProducesTelemetry_And_NoImmediateDeath(int seed)
        {
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "test_survivor", Health = 100f, Hunger = 20f, Thirst = 25f, Warmth = 85f, Morale = 70f };
            needs.Register(survivor);

            var rad = new RadiationSystem(exposureContext: s => new ExposureContext { ZoneRadLevel = 2f, ShelterShielding = 1f }, seed: seed);
            var radState = new SurvivorRadState { Id = "test_survivor", RadiationDose = 5f, LifetimeRadiationExposure = 10f };
            rad.Register(radState);

            var market = new MarketSystem();
            var rng = new SeededRng(seed);
            // Load goods catalog if present (optional)
            try
            {
                var io = new FileSystemIO();
                var ser = new SystemTextJsonSerializer();
                var dataDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(dataDir))
                {
                    var goods = GoodsCatalogLoader.Load(dataDir, io, ser);
                    // goods loaded but not strictly needed for market ticks
                }
            }
            catch { /* ignore */ }

            var rows = new List<string>();
            rows.Add("seed,day,hunger,thirst,fatigue,warmth,morale,health,radiationDose,lifetimeExposure,hungerCritical,thirstCritical,healthLossPerDay");

            for (int day = 1; day <= 7; day++)
            {
                // Tick 24h
                needs.Tick(24f);
                rad.Tick(24f);
                market.TickDay(day, rng);

                bool hungerCrit = survivor.Hunger >= 90f;
                bool thirstCrit = survivor.Thirst >= 90f;
                float health = survivor.Health;
                rows.Add($"{seed},{day},{survivor.Hunger:F2},{survivor.Thirst:F2},{survivor.Fatigue:F2},{survivor.Warmth:F2},{survivor.Morale:F2},{health:F2},{radState.RadiationDose:F2},{radState.LifetimeRadiationExposure:F2},{hungerCrit},{thirstCrit},{(100f-health):F2}");
                // Early death is a finding, not a harness failure, but we assert no death before day 3 for baseline survivor
                if (day <= 3) Assert.True(survivor.IsAliveState, $"Survivor died unexpectedly on day {day} seed {seed}");
            }

            // Write CSV
            try
            {
                Directory.CreateDirectory(ArtifactDir);
                var path = Path.Combine(ArtifactDir, $"balance_seed_{seed}.csv");
                File.WriteAllLines(path, rows);
            }
            catch { /* ignore I/O in CI */ }

            // Basic invariants
            Assert.True(survivor.Hunger > 20f, "Hunger must increase over 7 days");
            Assert.True(survivor.Thirst > 25f, "Thirst must increase");
            Assert.True(radState.LifetimeRadiationExposure > 10f, "Radiation must accumulate");
            Assert.True(survivor.Hunger <= 100f && survivor.Thirst <= 100f, "Needs clamped 0..100");
        }

        [Fact]
        public void SevenDayHarness_CsvIsDeterministic()
        {
            // Two runs with same seed must produce identical hunger trajectory
            var traj1 = CaptureTrajectory(42);
            var traj2 = CaptureTrajectory(42);
            Assert.Equal(traj1.Count, traj2.Count);
            for (int i = 0; i < traj1.Count; i++)
                Assert.Equal(traj1[i], traj2[i]);

            var trajDiff = CaptureTrajectory(999);
            // Different seed may diverge in market, but needs drift is deterministic per profile (not seed), so hunger same across seeds
            Assert.Equal(traj1[1], trajDiff[1]); // day1 hunger same regardless of seed (needs not seeded)
        }

        private static List<float> CaptureTrajectory(int seed)
        {
            var needs = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "x", Health = 100f };
            needs.Register(s);
            var rad = new RadiationSystem(seed: seed);
            var rs = new SurvivorRadState { Id = "x" };
            rad.Register(rs);
            var list = new List<float>();
            for (int d = 1; d <= 7; d++) { needs.Tick(24f); rad.Tick(24f); list.Add(s.Hunger); }
            return list;
        }
    }
}
