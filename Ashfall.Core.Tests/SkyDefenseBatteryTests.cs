using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Institutions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Ashfall.Core.SkyDefense;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Task 7 — SkyDefenseBatterySystem behaviour gates: telemetry
    /// intake, magazine logistics, deterministic firing, heat/hydraulics,
    /// service, and armor-pipeline integration for residual strikes.
    /// </summary>
    public class SkyDefenseBatteryTests
    {
        private const int ImpactDay = 30;

        private sealed class TrackingAvailability : IInstitutionAvailability
        {
            public readonly HashSet<string> Claims = new(StringComparer.Ordinal);
            public bool IsAvailable(string survivorId) => !Claims.Contains(survivorId);
            public bool TryClaim(string survivorId, string institutionId, string roleId) => Claims.Add(survivorId);
            public void Release(string survivorId, string institutionId, string roleId) => Claims.Remove(survivorId);
        }

        private sealed class FixedSkills : ISurvivorSkillsPort
        {
            public readonly HashSet<string> Skilled = new(StringComparer.Ordinal);
            public bool HasSkill(string survivorId, string skillId) => Skilled.Contains(survivorId);
        }

        private sealed class Fixture
        {
            public Inventory.Inventory Inventory = new();
            public TrackingAvailability Availability = new();
            public FixedSkills Skills = new();
            public SkyLayerArmorSystem Armor = new();
            public OrbitalHarrowTelemetrySystem Telemetry;
            public SkyDefenseBatterySystem Defense = null!;
            public List<(string Track, bool Success, float Residual)> Resolutions = new();
            public List<string> MaintenanceDue = new();

            public static Fixture Create(int masterSeed = 42)
            {
                var f = new Fixture();
                f.Telemetry = new OrbitalHarrowTelemetrySystem(f.Armor, new SeededRng(masterSeed));
                f.Defense = new SkyDefenseBatterySystem(
                    masterSeed,
                    inventory: f.Inventory,
                    telemetry: f.Telemetry,
                    availability: f.Availability,
                    skills: f.Skills);
                f.Defense.LoadOrdnanceCatalog(LoadOrdnance());
                f.Defense.OnInterceptResolved += (track, ammo, success, residual) => f.Resolutions.Add((track, success, residual));
                f.Defense.OnMaintenanceDue += t => f.MaintenanceDue.Add(t);
                f.Inventory.TryProduce("ammo_76mm_he_flak", 20);
                f.Inventory.TryProduce("ammo_76mm_proximity_fuse", 20);
                f.Inventory.TryProduce("machine_oil", 10);
                return f;
            }

            public static List<SkyDefenseOrdnanceDefinition> LoadOrdnance()
            {
                string dataDir = CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string found)
                    ? found
                    : throw new InvalidOperationException("data dir not found");
                return SkyDefenseOrdnanceCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            }

            /// <summary>Schedules an inbound impact through the authoritative telemetry.</summary>
            public void ScheduleInbound(int impactDay = 30, int gridX = 4, float energyMj = 12f) =>
                Telemetry.ScheduleImpact(impactDay, gridX, energyMj);

            public OrbitalTrackState? Track => Defense.Tracks.FirstOrDefault();
        }

        // ------------------------------------------------------------------
        // TELEMETRY INTAKE
        // ------------------------------------------------------------------

        [Fact]
        public void OrbitalWarning_CreatesSingleTrack_NoDuplicates()
        {
            var f = Fixture.Create();
            f.ScheduleInbound();
            Assert.Single(f.Defense.Tracks);
            Assert.Equal("custom_impact", f.Track!.track_id);
            Assert.Equal(30, f.Track.impact_day);

            f.ScheduleInbound(); // repeated same warning
            Assert.Single(f.Defense.Tracks);
        }

        // ------------------------------------------------------------------
        // MAGAZINE LOGISTICS
        // ------------------------------------------------------------------

        [Fact]
        public void MagazineLoad_TransfersAtomically_AndNeverDoubleCounts()
        {
            var f = Fixture.Create();
            var turret = f.Defense.EnsureDefaultTurret();
            Assert.Equal(20, f.Inventory.CountById("ammo_76mm_he_flak"));

            var result = f.Defense.TryLoadMagazine(turret.turret_id, "ammo_76mm_he_flak");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal("ammo_76mm_he_flak", turret.loaded_ammo_id);
            Assert.Equal(6, turret.magazine_count);                     // authored magazine_units
            Assert.Equal(14, f.Inventory.CountById("ammo_76mm_he_flak")); // removed from inventory

            // reload after two rounds: unloads the remainder back, tops up
            turret.magazine_count = 4;
            f.Defense.TryLoadMagazine(turret.turret_id, "ammo_76mm_he_flak");
            Assert.Equal(6, turret.magazine_count);
        }

        [Fact]
        public void MagazineLoad_UnknownOrdnance_OrInsufficientStock_FailsCleanly()
        {
            var f = Fixture.Create();
            var turret = f.Defense.EnsureDefaultTurret();
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Defense.TryLoadMagazine(turret.turret_id, "ammo_nope").Status);

            f.Inventory.TryConsume("ammo_76mm_proximity_fuse", 20);
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Defense.TryLoadMagazine(turret.turret_id, "ammo_76mm_proximity_fuse").Status);
            Assert.Equal(0, turret.magazine_count);
        }

        // ------------------------------------------------------------------
        // FIRING
        // ------------------------------------------------------------------

        [Fact]
        public void Volley_ConsumesMagazineRound_IncrementsHeatAndServiceCounter()
        {
            var f = Fixture.Create();
            var turret = f.Defense.EnsureDefaultTurret();
            f.ScheduleInbound();
            f.Defense.TryLoadMagazine(turret.turret_id, "ammo_76mm_he_flak");
            int heatBefore = turret.barrel_heat;
            int invBefore = f.Inventory.CountById("ammo_76mm_he_flak");

            var result = f.Defense.TryFireVolley(turret.turret_id, "custom_impact");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(5, turret.magazine_count);
            Assert.Equal(heatBefore + 12, turret.barrel_heat);       // authored heat_per_volley
            Assert.Equal(1, turret.volleys_since_service);
            Assert.Equal(1, turret.azimuth >= 0 && turret.azimuth < 360 ? 1 : 0); // normalized
            Assert.Equal(invBefore, f.Inventory.CountById("ammo_76mm_he_flak"));  // magazine owns the round now
        }

        [Fact]
        public void Volley_WithoutAmmo_OrUnknownTrack_FailsWithoutConsumption()
        {
            var f = Fixture.Create();
            var turret = f.Defense.EnsureDefaultTurret();

            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Defense.TryFireVolley(turret.turret_id, "custom_impact").Status);

            f.ScheduleInbound();
            f.Defense.TryLoadMagazine(turret.turret_id, "ammo_76mm_he_flak");
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Defense.TryFireVolley(turret.turret_id, "track_does_not_exist").Status);
            Assert.Equal(6, turret.magazine_count); // untouched
        }

        [Fact]
        public void InterceptChance_ClampsToAuthoredBounds_AndProximityFuseHelps()
        {
            var f = Fixture.Create();
            var wrecked = f.Defense.EnsureDefaultTurret();
            wrecked.radar_calibration = 0;
            wrecked.hydraulic_condition = 0;

            var flak = f.Defense.GetOrdnance("ammo_76mm_he_flak")!;
            var proximity = f.Defense.GetOrdnance("ammo_76mm_proximity_fuse")!;
            var chaff = f.Defense.GetOrdnance("ammo_chaff_burst")!;
            var track = new OrbitalTrackState { track_id = "t", severity = "Severe" };

            int floor = f.Defense.PreviewInterceptChance(wrecked, track, chaff);
            Assert.InRange(floor, SkyDefenseBatterySystem.MinInterceptChance, SkyDefenseBatterySystem.MaxInterceptChance);
            Assert.Equal(SkyDefenseBatterySystem.MinInterceptChance, floor);

            var pristine = f.Defense.EnsureDefaultTurret();
            pristine.radar_calibration = 100;
            pristine.hydraulic_condition = 100;
            var easy = new OrbitalTrackState { track_id = "t2", severity = "Minor" };
            int ceiling = f.Defense.PreviewInterceptChance(pristine, easy, proximity);
            Assert.InRange(ceiling, SkyDefenseBatterySystem.MinInterceptChance, SkyDefenseBatterySystem.MaxInterceptChance);
            Assert.Equal(SkyDefenseBatterySystem.MaxInterceptChance, ceiling);

            // same turret/track: proximity fuse strictly outperforms plain flak
            int withFlak = f.Defense.PreviewInterceptChance(pristine, easy, flak);
            Assert.True(ceiling > withFlak, "proximity fuse must apply its authored modifier");
        }

        [Fact]
        public void Firing_SameSeedSameState_IsDeterministic()
        {
            string TraceSequence(int seed)
            {
                var f = Fixture.Create(seed);
                var turret = f.Defense.EnsureDefaultTurret();
                f.ScheduleInbound();
                f.Defense.TryLoadMagazine(turret.turret_id, "ammo_76mm_he_flak");
                var trace = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    var r = f.Defense.TryFireVolley(turret.turret_id, "custom_impact");
                    trace.Add($"{r.Deltas["roll"]}:{r.Deltas["intercepted"]}");
                }
                return string.Join("|", trace);
            }

            Assert.Equal(TraceSequence(42), TraceSequence(42));
            // different seed may differ — sanity only, no assertion on direction
        }

        // ------------------------------------------------------------------
        // ARMOR PIPELINE INTEGRATION
        // ------------------------------------------------------------------

        [Fact]
        public void Interception_ModifiesStrike_RetainedThroughArmorPipeline()
        {
            // find seeds that deterministically produce one success and one failure
            int successSeed = -1, failureSeed = -1;
            for (int seed = 42; seed < 80 && (successSeed < 0 || failureSeed < 0); seed++)
            {
                var probe = Fixture.Create(seed);
                var pTurret = probe.Defense.EnsureDefaultTurret();
                probe.ScheduleInbound();
                probe.Defense.TryLoadMagazine(pTurret.turret_id, "ammo_76mm_he_flak");
                var r = probe.Defense.TryFireVolley(pTurret.turret_id, "custom_impact");
                if (r.Deltas["intercepted"] > 0 && successSeed < 0) successSeed = seed;
                if (r.Deltas["intercepted"] == 0 && failureSeed < 0) failureSeed = seed;
            }
            Assert.True(successSeed > 0, "no interception-success seed found in scan range");
            Assert.True(failureSeed > 0, "no interception-failure seed found in scan range");

            float EnergyAfter(int seed)
            {
                var f = Fixture.Create(seed);
                float initial = 12f;
                f.ScheduleInbound(energyMj: initial);
                var turret = f.Defense.EnsureDefaultTurret();
                f.Defense.TryLoadMagazine(turret.turret_id, "ammo_76mm_he_flak");
                f.Defense.TryFireVolley(turret.turret_id, "custom_impact");
                return f.Telemetry.State.impactEnergyMj;
            }

            float successEnergy = EnergyAfter(successSeed);
            float failureEnergy = EnergyAfter(failureSeed);

            // failed interception leaves the strike untouched for the armor
            Assert.Equal(12f, failureEnergy, 2);
            // successful interception reduces energy to the authored shrapnel residual
            Assert.True(successEnergy < 12f, $"success energy {successEnergy} should be reduced");
            Assert.Equal(12f * 0.3f, successEnergy, 2); // flak residual_shrapnel_severity
        }

        // ------------------------------------------------------------------
        // MAINTENANCE + WEAR
        // ------------------------------------------------------------------

        [Fact]
        public void Service_IsAtomic_ResetsCounter_RepairsHydraulics()
        {
            var f = Fixture.Create();
            var turret = f.Defense.EnsureDefaultTurret();
            f.ScheduleInbound();
            f.Defense.TryLoadMagazine(turret.turret_id, "ammo_76mm_he_flak");

            for (int i = 0; i < 4; i++)
                f.Defense.TryFireVolley(turret.turret_id, "custom_impact");
            turret.hydraulic_condition = 55;
            Assert.Equal(4, turret.volleys_since_service);

            // no oil → atomic failure
            f.Inventory.TryConsume("machine_oil", 10);
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Defense.TryServiceHydraulics(turret.turret_id).Status);
            Assert.Equal(4, turret.volleys_since_service);

            f.Inventory.TryProduce("machine_oil", 5);
            var ok = f.Defense.TryServiceHydraulics(turret.turret_id);
            Assert.Equal(ActionResult.StatusKind.Success, ok.Status);
            Assert.Equal(0, turret.volleys_since_service);
            Assert.Equal(95, turret.hydraulic_condition); // 55 + 40
            Assert.Equal(4, f.Inventory.CountById("machine_oil"));
        }

        [Fact]
        public void DailyTick_CoolsHeat_DriftsRadar()
        {
            var f = Fixture.Create();
            var turret = f.Defense.EnsureDefaultTurret();
            turret.barrel_heat = 80;
            turret.radar_calibration = 80;
            f.Defense.TickDay(1);
            Assert.Equal(50, turret.barrel_heat);
            Assert.Equal(78, turret.radar_calibration);
        }

        [Fact]
        public void CrewAssignment_ClaimsAvailability_ThroughAuthority()
        {
            var f = Fixture.Create();
            var turret = f.Defense.EnsureDefaultTurret();
            Assert.Equal(ActionResult.StatusKind.Success,
                f.Defense.TryAssignCrew(turret.turret_id, "survivor_gunner").Status);
            Assert.False(f.Availability.IsAvailable("survivor_gunner"));

            f.Defense.TryRemoveCrew(turret.turret_id, "survivor_gunner");
            Assert.True(f.Availability.IsAvailable("survivor_gunner"));

            // unavailable survivor rejected
            Assert.True(f.Availability.TryClaim("survivor_busy", "institution_other", "x"));
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Defense.TryAssignCrew(turret.turret_id, "survivor_busy").Status);
        }

        // ------------------------------------------------------------------
        // SAVE / RESTORE
        // ------------------------------------------------------------------

        [Fact]
        public void SaveLoad_PreservesBatteryState_AndContinuationMatches()
        {
            var f = Fixture.Create();
            var turret = f.Defense.EnsureDefaultTurret();
            f.ScheduleInbound();
            f.Defense.TryLoadMagazine(turret.turret_id, "ammo_76mm_he_flak");
            f.Defense.TryFireVolley(turret.turret_id, "custom_impact");

            var saved = f.Defense.CaptureState();
            var fresh = Fixture.Create();
            fresh.Defense.RestoreState(saved);
            var restoredTurret = fresh.Defense.EnsureDefaultTurret();

            Assert.Equal(turret.magazine_count, restoredTurret.magazine_count);
            Assert.Equal(turret.barrel_heat, restoredTurret.barrel_heat);
            Assert.Equal(turret.volleys_since_service, restoredTurret.volleys_since_service);
            Assert.Single(fresh.Defense.Tracks);

            // post-restore next volley matches uninterrupted run
            var a = f.Defense.TryFireVolley(turret.turret_id, "custom_impact");
            var b = fresh.Defense.TryFireVolley(restoredTurret.turret_id, "custom_impact");
            Assert.Equal(a.Deltas["roll"], b.Deltas["roll"]);
            Assert.Equal(a.Deltas["intercepted"], b.Deltas["intercepted"]);
        }

        [Fact]
        public void OldSave_MissingBatterySection_DefaultsSafely()
        {
            var f = Fixture.Create();
            f.Defense.RestoreState(null);
            Assert.Empty(f.Defense.Turrets);
            Assert.Empty(f.Defense.Tracks);
            f.Defense.EnsureDefaultTurret(); // re-arms cleanly
            Assert.Single(f.Defense.Turrets);
        }
    }
}
