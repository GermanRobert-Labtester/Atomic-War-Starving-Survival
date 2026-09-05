using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 80 — kinetic storage energy math, unit conversion, boundary,
    /// surge, black-start, and failure-replay tests.
    /// </summary>
    public class KineticStorageSystemTests
    {
        private const string ClassId = "flywheel_test_500";
        private const string InstanceId = "flywheel_flywheel_test_500_room_a";

        private static KineticFlywheelCatalog MakeCatalog()
        {
            return new KineticFlywheelCatalog
            {
                flywheel_classes = new List<FlywheelClassDef>
                {
                    new FlywheelClassDef
                    {
                        flywheel_id = ClassId,
                        display_name = "Test Rotor",
                        rotor_mass_kg = 500f,
                        effective_radius_m = 0.45f,
                        moment_of_inertia_factor = 0.5f,
                        max_rpm = 12000f,
                        max_safe_rpm_ratio = 0.9f,
                        min_vacuum_torr = 1.0e-3f,
                        operational_vacuum_torr = 1.0e-5f,
                        max_bearing_temp_c = 120f,
                        safe_bearing_temp_c = 90f,
                        containment_rating = 0.7f,
                        motor_generator_efficiency = 0.88f,
                        max_charge_kw = 15f,
                        max_discharge_kw = 25f,
                        idle_drag_loss_percent_per_hour = 0.5f,
                        vacuum_leak_rate_per_day = 0.02f,
                        bearing_heat_per_charge_kw = 0.8f,
                        bearing_heat_per_discharge_kw = 1.0f,
                        bearing_cooling_rate_per_tick = 0.5f,
                        construction_required_items = new List<string> { "item_forged_rotor_shaft" },
                        maintenance_required_items = new List<string> { "item_bearing_grease" }
                    }
                },
                surge_events = new List<SurgeEventDef>
                {
                    new SurgeEventDef { surge_id = "surge_blast_door_motor", display_name = "Blast Door", peak_kw = 18f, duration_ticks = 1, event_class = "door_motor" },
                    new SurgeEventDef { surge_id = "surge_decon_pump_cycle", display_name = "Decon Pump", peak_kw = 8f, duration_ticks = 1, event_class = "decon_pump" }
                },
                black_start = new BlackStartDef { min_stored_energy_kwh = 0.5f, generator_restart_probability = 0.95f },
                containment_hazard = new ContainmentHazardDef()
            };
        }

        private static KineticStorageSystem Create(out List<string> consumed, int seed = 64, KineticFlywheelCatalog? catalog = null)
        {
            consumed = new List<string>();
            var sys = new KineticStorageSystem(catalog ?? MakeCatalog(), new SeededRng(seed));
            return sys;
        }

        private static Func<string, int, bool> ConsumeFrom(List<string> ledger)
        {
            return (itemId, amount) =>
            {
                for (int i = 0; i < amount; i++) ledger.Add(itemId);
                return true;
            };
        }

        private static FlywheelInstance InstallOnline(KineticStorageSystem sys, List<string>? ledger = null)
        {
            var r = sys.InstallFlywheel(ClassId, "room_a", 1, ConsumeFrom(ledger ?? new List<string>()));
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(ActionResult.StatusKind.Success, sys.BringOnline(InstanceId).Status);
            return sys.FindFlywheel(InstanceId)!;
        }

        private static float MaxEnergyJ(KineticStorageSystem sys)
        {
            var fc = sys.FindClass(ClassId)!;
            float i = KineticStorageSystem.ComputeMomentOfInertia(fc.rotor_mass_kg, fc.effective_radius_m, fc.moment_of_inertia_factor);
            return KineticStorageSystem.ComputeStoredEnergyJ(i, fc.max_rpm * fc.max_safe_rpm_ratio);
        }

        // ─── Energy math & unit discipline ───

        [Fact]
        public void EnergyFormula_MatchesPhysics()
        {
            float i = KineticStorageSystem.ComputeMomentOfInertia(500f, 0.45f, 0.5f);
            Assert.Equal(50.625f, i, 4);

            float rpm = 10800f;
            float omega = rpm * (float)(2.0 * Math.PI / 60.0);
            float expected = 0.5f * i * omega * omega;
            Assert.Equal(expected, KineticStorageSystem.ComputeStoredEnergyJ(i, rpm), 1);
        }

        [Fact]
        public void EnergyRpm_RoundTrip()
        {
            float i = KineticStorageSystem.ComputeMomentOfInertia(500f, 0.45f, 0.5f);
            float rpm = 9000f;
            float roundTrip = KineticStorageSystem.ComputeRpmFromEnergy(i, KineticStorageSystem.ComputeStoredEnergyJ(i, rpm));
            Assert.Equal(rpm, roundTrip, 2);
        }

        [Fact]
        public void UnitConversion_JouleKwh()
        {
            Assert.Equal(1f, KineticStorageSystem.JoulesToKwh(3_600_000f), 5);
            Assert.Equal(3_600_000f, KineticStorageSystem.KwhToJoules(1f), 1);
            Assert.Equal(7.5f, KineticStorageSystem.JoulesToKwh(KineticStorageSystem.KwhToJoules(7.5f)), 4);
        }

        // ─── Boundaries ───

        [Fact]
        public void FreshInstall_ZeroRpm_ZeroEnergy()
        {
            var sys = Create(out _);
            var f = InstallOnline(sys);
            Assert.Equal(0f, f.rotorRpm);
            Assert.Equal(0f, f.storedEnergyJ);
        }

        [Fact]
        public void Charge_ClampsAtExactFullEnergy()
        {
            var sys = Create(out _);
            InstallOnline(sys);
            float maxEnergy = MaxEnergyJ(sys);

            sys.Charge(InstanceId, 15f, 3600f);
            sys.Charge(InstanceId, 15f, 3600f);
            sys.Charge(InstanceId, 15f, 3600f);

            var f = sys.FindFlywheel(InstanceId)!;
            Assert.Equal(maxEnergy, f.storedEnergyJ, 1);
        }

        [Fact]
        public void Discharge_NeverDrivesEnergyNegative()
        {
            var sys = Create(out _);
            InstallOnline(sys);
            sys.Charge(InstanceId, 15f, 60f); // small charge

            sys.Discharge(InstanceId, 25f, 3600f); // request far more than stored

            var f = sys.FindFlywheel(InstanceId)!;
            Assert.Equal(0f, f.storedEnergyJ);
            Assert.Equal(0f, f.rotorRpm);
        }

        [Fact]
        public void Discharge_FromEmpty_ReturnsZero()
        {
            var sys = Create(out _);
            InstallOnline(sys);
            Assert.Equal(0f, sys.Discharge(InstanceId, 25f, 10f));
        }

        [Fact]
        public void Charge_BlockedAtExactThermalLimit()
        {
            var sys = Create(out _);
            var fc = sys.FindClass(ClassId)!;
            var f = InstallOnline(sys);
            f.bearingTemperatureC = fc.max_bearing_temp_c; // exactly at limit

            Assert.Equal(0f, sys.Charge(InstanceId, 15f, 60f));
        }

        [Fact]
        public void Charge_BlockedWhenVacuumTooPoor()
        {
            var sys = Create(out _);
            var fc = sys.FindClass(ClassId)!;
            var f = InstallOnline(sys);
            f.vacuumPressureTorr = fc.min_vacuum_torr * 2f; // degraded past the floor

            Assert.Equal(0f, sys.Charge(InstanceId, 15f, 60f));
        }

        // ─── Charge / discharge accounting ───

        [Fact]
        public void Charge_RespectsEfficiency_AndHeatsBearings()
        {
            var sys = Create(out _);
            var fc = sys.FindClass(ClassId)!;
            var f = InstallOnline(sys);
            float tempBefore = f.bearingTemperatureC;

            float stored = sys.Charge(InstanceId, 10f, 60f);

            // Ideal 10 kW * 60 s = 600 kJ; at 88% efficiency the rotor sees less.
            Assert.Equal(10f * 1000f * 60f * fc.motor_generator_efficiency, stored, 1);
            Assert.True(f.bearingTemperatureC > tempBefore, "charging must heat bearings");
        }

        [Fact]
        public void Discharge_DecreasesEnergy_AndFiresOnceAtEmpty()
        {
            var sys = Create(out _);
            var f = InstallOnline(sys);
            sys.Charge(InstanceId, 15f, 120f);
            float before = f.storedEnergyJ;
            int dischargedEvents = 0;
            sys.OnFlywheelDischarged += _ => dischargedEvents++;

            sys.Discharge(InstanceId, 25f, 3600f);

            Assert.True(f.storedEnergyJ < before);
            Assert.Equal(1, dischargedEvents); // fires exactly once at empty
        }

        // ─── Idle physics ───

        [Fact]
        public void DragLoss_DecaysStoredEnergy_PerHour()
        {
            var sys = Create(out _);
            var fc = sys.FindClass(ClassId)!;
            var f = InstallOnline(sys);
            sys.Charge(InstanceId, 15f, 600f);
            float before = f.storedEnergyJ;

            sys.Tick(3600f); // one hour

            Assert.Equal(before * (1f - fc.idle_drag_loss_percent_per_hour / 100f), f.storedEnergyJ, before * 0.0001f);
        }

        [Fact]
        public void BearingCooling_WhenIdle()
        {
            var sys = Create(out _);
            var f = InstallOnline(sys);
            f.bearingTemperatureC = 80f;
            sys.Tick(60f);
            Assert.True(f.bearingTemperatureC < 80f);
            Assert.True(f.bearingTemperatureC >= 20f);
        }

        [Fact]
        public void VacuumLeak_Degrades_OverTime()
        {
            var sys = Create(out _);
            var fc = sys.FindClass(ClassId)!;
            var f = InstallOnline(sys);
            float vacuumBefore = f.vacuumPressureTorr;

            sys.Tick(86400f); // one day

            Assert.True(f.vacuumPressureTorr > vacuumBefore, "vacuum must degrade without service");
        }

        // ─── Surge buffering ───

        [Fact]
        public void SurgeDeliversBurst_AndDrainsEnergy()
        {
            var sys = Create(out _);
            InstallOnline(sys);
            sys.Charge(InstanceId, 15f, 600f);
            float before = sys.FindFlywheel(InstanceId)!.storedEnergyJ;

            float delivered = sys.HandleSurge(InstanceId, "surge_blast_door_motor");

            Assert.True(delivered > 0f);
            Assert.True(delivered <= 18f + 0.001f); // never exceeds the surge peak
            Assert.True(sys.FindFlywheel(InstanceId)!.storedEnergyJ < before);
        }

        [Fact]
        public void Surge_FromEmptyFlywheel_DeliversNothing()
        {
            var sys = Create(out _);
            InstallOnline(sys);
            Assert.Equal(0f, sys.HandleSurge(InstanceId, "surge_blast_door_motor"));
        }

        // ─── Black start ───

        [Fact]
        public void BlackStart_ConsumesExactEnergy_AndIsDeterministic()
        {
            var sys1 = Create(out _, seed: 91);
            InstallOnline(sys1);
            sys1.Charge(InstanceId, 15f, 1200f);

            var sys2 = Create(out _, seed: 91);
            InstallOnline(sys2);
            sys2.Charge(InstanceId, 15f, 1200f);

            float before1 = sys1.FindFlywheel(InstanceId)!.storedEnergyJ;
            bool r1 = sys1.TryBlackStart(InstanceId);
            bool r2 = sys2.TryBlackStart(InstanceId);

            Assert.Equal(r1, r2); // same seed → same outcome
            Assert.Equal(
                before1 - KineticStorageSystem.KwhToJoules(0.5f),
                sys1.FindFlywheel(InstanceId)!.storedEnergyJ, 1);
        }

        [Fact]
        public void BlackStart_WithInsufficientEnergy_FailsAtomically()
        {
            var sys = Create(out _);
            InstallOnline(sys);
            sys.Charge(InstanceId, 15f, 10f); // far below 0.5 kWh

            float before = sys.FindFlywheel(InstanceId)!.storedEnergyJ;
            Assert.False(sys.TryBlackStart(InstanceId));
            Assert.Equal(before, sys.FindFlywheel(InstanceId)!.storedEnergyJ); // nothing consumed
        }

        // ─── Failure replay ───

        [Fact]
        public void OverspeedFailure_DeterministicPerSeed()
        {
            float overEnergy = MaxEnergyJ(new KineticStorageSystem(MakeCatalog(), new SeededRng(1)))
                * 4f; // 2x safe rpm ratio → energy 4x (E ∝ ω²)

            var sys1 = Create(out _, seed: 17);
            InstallOnline(sys1);
            sys1.FindFlywheel(InstanceId)!.storedEnergyJ = overEnergy;

            var sys2 = Create(out _, seed: 17);
            InstallOnline(sys2);
            sys2.FindFlywheel(InstanceId)!.storedEnergyJ = overEnergy;

            sys1.Tick(1f);
            sys2.Tick(1f);

            Assert.Equal(
                sys1.FindFlywheel(InstanceId)!.hasFailed,
                sys2.FindFlywheel(InstanceId)!.hasFailed);
        }

        [Fact]
        public void CatastrophicFailure_FiresExactlyOnce()
        {
            var sys = Create(out _, seed: 17);
            InstallOnline(sys);
            int failures = 0;
            sys.OnFlywheelFailure += _ => failures++;
            sys.FindFlywheel(InstanceId)!.storedEnergyJ = MaxEnergyJ(sys) * 4f;

            sys.Tick(1f);
            if (sys.FindFlywheel(InstanceId)!.hasFailed)
            {
                // A failed flywheel is skipped by future ticks — no repeat event.
                sys.Tick(3600f);
                sys.Tick(3600f);
                Assert.Equal(1, failures);
            }
            else
            {
                Assert.Equal(0, failures);
            }
        }

        // ─── Maintenance ───

        [Fact]
        public void Maintenance_RestoresVacuum_AndClearsBrake()
        {
            var sys = Create(out _);
            var ledger = new List<string>();
            var f = InstallOnline(sys, ledger);
            f.bearingTemperatureC = 119f;
            f.emergencyBrakeEngaged = true;

            var maintLedger = new List<string>();
            var r = sys.PerformMaintenance(InstanceId, 40, ConsumeFrom(maintLedger));

            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.False(f.emergencyBrakeEngaged);
            Assert.Equal(sys.FindClass(ClassId)!.operational_vacuum_torr, f.vacuumPressureTorr, 10);
            Assert.Single(maintLedger); // exactly one grease tube
        }

        // ─── Aggregates ───

        [Fact]
        public void Totals_AggregateAcrossFlywheels()
        {
            var sys = Create(out _);
            InstallOnline(sys);
            sys.Charge(InstanceId, 15f, 600f);

            Assert.True(sys.TotalStoredEnergyKwh() > 0f);
            Assert.True(sys.TotalDischargeCapacityKw() > 0f);
            Assert.True(sys.TotalDischargeCapacityKw() <= 25f + 0.001f);
        }
    }
}
