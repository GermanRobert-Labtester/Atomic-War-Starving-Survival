using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan B68 — Geological Faultline Seismic Monitoring &amp; Shock Dampening.
    /// Verifies the monitoring expansion of the Plan 56 SeismicDynamicsSystem:
    /// P/S two-stage warning window, geophone coverage (earlier detection),
    /// dampener installation/service/wear/damping, derived warning stages,
    /// rockburst handoff, save round-trip, legacy-safe defaults and paired
    /// determinism. Damage ownership stays with thermal/excavation authorities.
    /// </summary>
    public sealed class SeismicMonitoringB68Tests
    {
        private static (SeismicDynamicsSystem seismic, ShelterThermalSystem thermal, ExcavationHazardSystem hazard, InventoryContainer inventory) CreateFixture(int seed = 42)
        {
            var rng = new SeededRng(seed);
            var inventory = new InventoryContainer();
            var needs = new NeedsSystem();
            var sl = new StartingLevelSystem();
            var df = new YearOfAshDeepFreezeSystem();
            var thermal = new ShelterThermalSystem(rng, needs, sl, df);
            var hazard = new ExcavationHazardSystem(inventory, rng);
            var seismic = new SeismicDynamicsSystem(rng, thermal, hazard, inventory);
            return (seismic, thermal, hazard, inventory);
        }

        private static void SeedItems(InventoryContainer inv)
        {
            inv.AddById("item_geophone_probe", 20);
            inv.AddById("item_seismic_damper_pad", 20);
            inv.AddById("item_vibration_dampening_mount", 20);
        }

        // -----------------------------------------------------------------
        // 1. Geophones
        // -----------------------------------------------------------------

        [Fact]
        public void InstallGeophone_ConsumesProbe_AddsCoverage()
        {
            var (seismic, _, _, inv) = CreateFixture();
            SeedItems(inv);

            var res = seismic.InstallGeophone("sector_excavation_alpha", inv);

            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Contains("sector_excavation_alpha", seismic.State.geophoneSectors);
            Assert.True(inv.CountById("item_geophone_probe") < 20, "probe should be consumed");
            Assert.True(seismic.HasGeophoneCoverage("fault_sub_strata_rift"));
        }

        [Fact]
        public void InstallGeophone_WithoutProbe_Blocked()
        {
            var (seismic, _, _, inv) = CreateFixture();
            var res = seismic.InstallGeophone("sector_excavation_alpha", inv);
            Assert.Equal(ActionResult.StatusKind.Blocked, res.Status);
            Assert.Empty(seismic.State.geophoneSectors);
        }

        [Fact]
        public void GeophoneCoverage_LowersDetectionThresholds()
        {
            // Drive a fault into the P window (0.45..0.65 with geophones) and
            // verify the primary-wave event fires while the main arrival does not.
            var (seismic, _, _, inv) = CreateFixture(seed: 7);
            SeedItems(inv);
            Assert.True(seismic.InstallGeophone("sector_excavation_alpha", inv).Status == ActionResult.StatusKind.Success);

            var pWarnings = new List<string>();
            var sWarnings = new List<string>();
            seismic.OnPrimaryWaveDetected += (f, r) => pWarnings.Add(f);
            seismic.OnEarlyWarning += (f, r) => sWarnings.Add(f);

            // sub_strata_rift: rate 2.5 * 1.15 = 2.875/day, threshold 100 →
            // ratio 0.5 around day 18. Tick until ratio is inside 0.45..0.65.
            for (int day = 1; day <= 12; day++) seismic.TickDay(day);
            Assert.Empty(pWarnings); // below 0.45 yet (ratio ~0.35)

            for (int day = 13; day <= 17; day++) seismic.TickDay(day);
            Assert.Contains("fault_sub_strata_rift", pWarnings);   // P window opened
            Assert.Empty(sWarnings);                                // main arrival not yet

            // Continue into the S window (0.65+).
            bool sFired = false;
            for (int day = 18; day <= 24 && !sFired; day++)
            {
                seismic.TickDay(day);
                sFired = sWarnings.Contains("fault_sub_strata_rift");
            }
            Assert.True(sFired, "main-arrival warning should follow the P window");
        }

        // -----------------------------------------------------------------
        // 2. Dampeners
        // -----------------------------------------------------------------

        [Fact]
        public void InstallDampener_ConsumesItems_SetsFullIntegrity()
        {
            var (seismic, _, _, inv) = CreateFixture();
            SeedItems(inv);

            var res = seismic.InstallDampener("sector_excavation_alpha", inv);

            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(100f, seismic.GetDampenerIntegrity("sector_excavation_alpha"));
            Assert.Equal(19, inv.CountById("item_seismic_damper_pad"));
            Assert.Equal(19, inv.CountById("item_vibration_dampening_mount"));
        }

        [Fact]
        public void Dampener_ReducesSlipSeverity_AndWears()
        {
            // Two identical runs: with and without a dampener. The damped run
            // must produce milder shoring damage in the affected sector.
            float severityWithout = RunSlipAndShoringDamage(withDampener: false);
            float severityWith = RunSlipAndShoringDamage(withDampener: true);

            Assert.True(severityWith < severityWithout,
                $"dampened shoring damage ({severityWith}) should be lower than undampened ({severityWithout})");
        }

        private float RunSlipAndShoringDamage(bool withDampener)
        {
            var (seismic, _, hazard, inv) = CreateFixture(seed: 99);
            SeedItems(inv);
            if (withDampener)
                Assert.Equal(ActionResult.StatusKind.Success,
                    seismic.InstallDampener("sector_excavation_alpha", inv).Status);

            // Force an immediate slip along sub_strata_rift via kinetic shock
            // (transfer 0.8*200 = 160 >= threshold 100).
            seismic.InjectKineticShock(200f, "sector_excavation_alpha");
            Assert.True(seismic.State.recentQuakes.Count > 0, "shock should force a slip");

            var sector = hazard.GetOrCreateSector("sector_excavation_alpha");
            return 1000f - sector.ShoringHealthPermille; // damage taken
        }

        [Fact]
        public void Dampener_WearsOnSlip_AndServiceRestores()
        {
            var (seismic, _, _, inv) = CreateFixture(seed: 5);
            SeedItems(inv);
            seismic.InstallDampener("sector_excavation_alpha", inv);
            Assert.Equal(100f, seismic.GetDampenerIntegrity("sector_excavation_alpha"));

            seismic.InjectKineticShock(200f, "sector_excavation_alpha");
            Assert.True(seismic.State.recentQuakes.Count > 0, "shock should force a slip");
            float after = seismic.GetDampenerIntegrity("sector_excavation_alpha");
            Assert.True(after < 100f, "slip should wear the dampener");
            Assert.True(after > 0f, "a single moderate slip must not destroy the dampener");

            var res = seismic.ServiceDampener("sector_excavation_alpha", inv);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(100f, seismic.GetDampenerIntegrity("sector_excavation_alpha"));
        }

        [Fact]
        public void ServiceDampener_WithoutDampener_Fails()
        {
            var (seismic, _, _, inv) = CreateFixture();
            SeedItems(inv);
            Assert.Equal(ActionResult.StatusKind.Failed, seismic.ServiceDampener("sector_excavation_alpha", inv).Status);
        }

        // -----------------------------------------------------------------
        // 3. Warning stages (derived)
        // -----------------------------------------------------------------

        [Fact]
        public void WarningStage_Progression_Deterministic()
        {
            var (seismic, _, _, _) = CreateFixture(seed: 3);

            Assert.Equal(SeismicWarningStage.Stable, seismic.GetFaultWarningStage("fault_sub_strata_rift", 1));

            // Imminent once ratio >= 0.80 (~day 29 at 2.875/day).
            for (int day = 1; day <= 28; day++) seismic.TickDay(day);
            var stage28 = seismic.GetFaultWarningStage("fault_sub_strata_rift", 28);
            Assert.True(stage28 == SeismicWarningStage.Swarm || stage28 == SeismicWarningStage.Imminent,
                "day 28 stage should be Swarm or Imminent, was " + stage28);
        }

        [Fact]
        public void WarningStage_Aftershock_Window()
        {
            var (seismic, _, _, _) = CreateFixture(seed: 11);
            seismic.InjectKineticShock(400f, "sector_excavation_alpha"); // force slip
            var stage = seismic.GetFaultWarningStage("fault_sub_strata_rift", seismic.State.currentDay);
            Assert.Equal(SeismicWarningStage.Aftershock, stage);
        }

        [Fact]
        public void ArrivalEstimate_BoundedAndMonotonic()
        {
            var (seismic, _, _, _) = CreateFixture(seed: 13);
            int d1 = seismic.EstimateArrivalDays("fault_sub_strata_rift");
            Assert.Equal(35, d1); // 100 / 2.875 = 34.8 → 35

            seismic.TickDay(1);
            int d2 = seismic.EstimateArrivalDays("fault_sub_strata_rift");
            Assert.True(d2 < d1, "estimate should shrink as tension accumulates");
        }

        // -----------------------------------------------------------------
        // 4. Rockburst handoff (delegation only)
        // -----------------------------------------------------------------

        [Fact]
        public void SevereSlip_EmitsRockburstRequest()
        {
            var (seismic, _, _, inv) = CreateFixture(seed: 21);
            SeedItems(inv);

            RockburstRequest? rb = null;
            seismic.OnRockburstRequested += r => rb = r;

            // Massive shock on the core → multiple faults slip; at least one
            // produces a severity >= 0.6 rockburst request.
            seismic.InjectKineticShock(500f, "bunker_core");

            Assert.NotNull(rb);
            Assert.True(rb!.severity >= 0.6f);
            Assert.NotEmpty(rb.sectors);
        }

        // -----------------------------------------------------------------
        // 5. Legacy-safe defaults + save round-trip
        // -----------------------------------------------------------------

        [Fact]
        public void LegacyState_NoCoverageNoDampeners_Plan56BehaviorUnchanged()
        {
            var (seismic, _, _, _) = CreateFixture(seed: 42);
            Assert.Empty(seismic.State.geophoneSectors);
            Assert.Empty(seismic.State.dampenerIntegrity);
            Assert.False(seismic.HasGeophoneCoverage("fault_sub_strata_rift"));
            // Without geophones, no P event in the 0.65..0.79 band.
            var pWarnings = new List<string>();
            seismic.OnPrimaryWaveDetected += (f, r) => pWarnings.Add(f);
            for (int day = 1; day <= 20; day++) seismic.TickDay(day);
            Assert.Empty(pWarnings); // ratio ~0.575 < 0.60 base threshold at day 20
        }

        [Fact]
        public void SaveRoundTrip_PreservesCoverageAndDampeners()
        {
            var (seismic, _, _, inv) = CreateFixture(seed: 77);
            SeedItems(inv);
            seismic.InstallGeophone("sector_excavation_alpha", inv);
            seismic.InstallDampener("bunker_core", inv);

            var saved = seismic.CaptureState();

            var (restored, _, _, _) = CreateFixture(seed: 77);
            restored.RestoreState(saved);

            Assert.Contains("sector_excavation_alpha", restored.State.geophoneSectors);
            Assert.Equal(100f, restored.GetDampenerIntegrity("bunker_core"));
            Assert.True(restored.HasGeophoneCoverage("fault_sub_strata_rift"));
        }

        // -----------------------------------------------------------------
        // 6. Paired determinism
        // -----------------------------------------------------------------

        [Fact]
        public void PairedRuns_SameSeed_ProduceIdenticalDampenedOutcome()
        {
            float a = RunSlipAndShoringDamage(withDampener: true);
            float b = RunSlipAndShoringDamage(withDampener: true);
            Assert.Equal(a, b);
        }
    }
}
