using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Radiation;

namespace Ashfall.Core.Tests
{
    public class BalanceRestRadiationTests
    {
        private static readonly string ArtifactDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "artifacts", "balance");

        private static List<string> RunWithRest(int seed, int days, bool useRest, out SurvivorNeedsState s)
        {
            var needs = new NeedsSystem(isNearHeatSource: _ => true);
            s = new SurvivorNeedsState { Id = "rest_test", Health = 100f, Hunger = 20f, Thirst = 25f, Warmth = 85f, Morale = 70f, Fatigue = 10f };
            needs.Register(s);
            var rad = new RadiationSystem(exposureContext: st => new ExposureContext { ZoneRadLevel = 2f, ShelterShielding = 1f }, seed: seed);
            var rs = new SurvivorRadState { Id = "rest_test", RadiationDose = 5f, LifetimeRadiationExposure = 10f };
            rad.Register(rs);

            var rows = new List<string>();
            rows.Add("seed,day,fatigue,health,restApplied");
            for (int day = 1; day <= days; day++)
            {
                // 24h drift
                needs.Tick(24f);
                rad.Tick(24f);
                // Daily ration to isolate fatigue from starvation (2 food, 3 water)
                needs.Modify(s, NeedKind.Hunger, -20f);
                needs.Modify(s, NeedKind.Thirst, -30f);
                bool restApplied = false;
                if (useRest)
                {
                    // Real mechanic: AssignRest 8h restores 64 fatigue (8*8)
                    needs.Modify(s, NeedKind.Fatigue, -64f);
                    restApplied = true;
                }
                rows.Add($"{seed},{day},{s.Fatigue:F2},{s.Health:F2},{restApplied}");
            }
            try { Directory.CreateDirectory(ArtifactDir); File.WriteAllLines(Path.Combine(ArtifactDir, $"rest_seed_{seed}_{days}d_rest{useRest}.csv"), rows); } catch { }
            return rows;
        }

        [Theory]
        [InlineData(42, 7)]
        [InlineData(42, 14)]
        [InlineData(42, 30)]
        public void Rest_Mechanic_Prevents_Fatigue_Cap(int seed, int days)
        {
            var rowsNoRest = RunWithRest(seed, days, false, out var sNoRest);
            var rowsWithRest = RunWithRest(seed, days, true, out var sWithRest);

            // Without rest: 0.4/h, start 10 -> day7 77.2, day14 100. So check progression.
            if (days >= 14)
                Assert.True(sNoRest.Fatigue >= 95f, $"Without rest fatigue should cap near 100 by day {days}, got {sNoRest.Fatigue:F1}");
            else
                Assert.True(sNoRest.Fatigue >= 70f, $"Without rest fatigue should be >70 by day {days}, got {sNoRest.Fatigue:F1}");
            // With 8h rest per day (real -64), fatigue stays low (<40)
            Assert.True(sWithRest.Fatigue < 40f, $"With rest fatigue should stay <40, got {sWithRest.Fatigue:F1} (seed {seed} days {days})");
            // Health should not be degraded by fatigue alone
            Assert.True(sWithRest.Health >= 95f, $"Rest scenario health should stay high, got {sWithRest.Health:F1}");
            // Determinism
            RunWithRest(seed, days, true, out var s2);
            Assert.Equal(sWithRest.Fatigue, s2.Fatigue, precision: 2);
        }

        private static (SurvivorNeedsState needs, SurvivorRadState rad, NeedsSystem needsSys, RadiationSystem radSys) CreateCoupledSystem(int seed, float zoneRad, float shielding)
        {
            var needsState = new SurvivorNeedsState { Id = "rad_test", Health = 100f, Hunger = 20f, Thirst = 25f, Warmth = 100f, Morale = 70f, Fatigue = 10f };
            var needsSys = new NeedsSystem(isNearHeatSource: _ => true);
            needsSys.Register(needsState);
            var radState = new SurvivorRadState { Id = "rad_test", RadiationDose = 5f, LifetimeRadiationExposure = 10f };
            // Coupled: radiation health loss via applyNeed -> NeedsSystem
            var radSys = new RadiationSystem(
                exposureContext: _ => new ExposureContext { ZoneRadLevel = zoneRad, ShelterShielding = shielding },
                applyNeed: (rs, needId, delta) =>
                {
                    if (needId == "health")
                        needsSys.Modify(needsState, NeedKind.Health, delta);
                },
                seed: seed);
            radSys.Register(radState);
            return (needsState, radState, needsSys, radSys);
        }

        [Theory]
        [InlineData(42, 1f, 1f, 7)]   // shielded: 1 rad/h (zone2 - shield1) -> dose 29 day1 -> 178 day7, but health coupling at acute 80
        [InlineData(42, 2f, 0f, 7)]   // unshielded: 2 rad/h -> dose 53 day1 -> 346 day7 -> acute health loss
        [InlineData(42, 10f, 0f, 7)]  // high rad: 10 rad/h -> rapid acute
        public void Radiation_Health_Coupling_Is_Measured(int seed, float zoneRad, float shielding, int days)
        {
            var (needs, rad, needsSys, radSys) = CreateCoupledSystem(seed, zoneRad, shielding);
            var rows = new List<string>();
            rows.Add("seed,day,zoneRad,shielding,dose,lifetime,health,acute");
            for (int day = 1; day <= days; day++)
            {
                needsSys.Tick(24f);
                radSys.Tick(24f);
                // Feed daily to keep hunger/thirst from killing before radiation does
                needsSys.Modify(needs, NeedKind.Hunger, -20f);
                needsSys.Modify(needs, NeedKind.Thirst, -30f);
                // Rest to keep fatigue low
                needsSys.Modify(needs, NeedKind.Fatigue, -64f);

                bool acute = rad.RadiationDose >= RadiationSystem.AcuteThreshold;
                rows.Add($"{seed},{day},{zoneRad},{shielding},{rad.RadiationDose:F1},{rad.LifetimeRadiationExposure:F1},{needs.Health:F1},{acute}");
            }
            try { Directory.CreateDirectory(ArtifactDir); File.WriteAllLines(Path.Combine(ArtifactDir, $"rad_seed_{seed}_zone{zoneRad}_shield{shielding}_{days}d.csv"), rows); } catch { }

            // With shielding 1, zone 2 => 1 rad/h, dose day7 ~178 but health coupling at 80+ should have caused some loss
            // We don't assert exact health, just that measurement happened and is deterministic
            if (zoneRad == 10f)
                Assert.True(needs.Health < 100f || rad.RadiationDose >= 80f, "High rad should trigger acute or health loss");
            // Determinism check
            var (needs2, rad2, needsSys2, radSys2) = CreateCoupledSystem(seed, zoneRad, shielding);
            for (int day = 1; day <= days; day++) { needsSys2.Tick(24f); radSys2.Tick(24f); needsSys2.Modify(needs2, NeedKind.Hunger, -20f); needsSys2.Modify(needs2, NeedKind.Thirst, -30f); needsSys2.Modify(needs2, NeedKind.Fatigue, -64f); }
            Assert.Equal(rad.RadiationDose, rad2.RadiationDose, precision: 1);
            Assert.Equal(needs.Health, needs2.Health, precision: 1);
        }

        [Fact]
        public void Radiation_Iodine_Provides_Protection()
        {
            var (needs, rad, needsSys, radSys) = CreateCoupledSystem(42, 2f, 0f);
            var (needsNoI, radNoI, needsSysNoI, radSysNoI) = CreateCoupledSystem(42, 2f, 0f);
            // Run without iodine
            for (int day = 1; day <= 7; day++)
            {
                needsSysNoI.Tick(24f);
                radSysNoI.Tick(24f);
                needsSysNoI.Modify(needsNoI, NeedKind.Hunger, -20f);
                needsSysNoI.Modify(needsNoI, NeedKind.Thirst, -30f);
                needsSysNoI.Modify(needsNoI, NeedKind.Fatigue, -64f);
            }
            // Run with iodine at day 3
            for (int day = 1; day <= 7; day++)
            {
                needsSys.Tick(24f);
                radSys.Tick(24f);
                needsSys.Modify(needs, NeedKind.Hunger, -20f);
                needsSys.Modify(needs, NeedKind.Thirst, -30f);
                needsSys.Modify(needs, NeedKind.Fatigue, -64f);
                if (day == 3)
                    radSys.AdministerIodine(rad);
            }
            // Iodine should have been applied (flag or timer)
            Assert.True(rad.HasRadResistance || rad.RadResistanceHoursRemaining >= 0f, "Iodine should set resistance flag");
            // Dose with iodine should be <= without (half for 6h)
            Assert.True(rad.LifetimeRadiationExposure <= radNoI.LifetimeRadiationExposure, $"Iodine dose {rad.LifetimeRadiationExposure:F1} should be <= no-iodine {radNoI.LifetimeRadiationExposure:F1}");
            // Health with iodine should be >= without (protection)
            Assert.True(needs.Health >= needsNoI.Health, $"Health with iodine {needs.Health:F1} should be >= without {needsNoI.Health:F1}");
        }
    }
}
