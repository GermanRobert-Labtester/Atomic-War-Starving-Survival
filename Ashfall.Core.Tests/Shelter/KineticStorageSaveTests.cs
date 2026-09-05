using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 80 — kinetic storage save/restore parity tests.
    /// Pins: restore at non-zero RPM preserves the exact rotor state and the
    /// future tick trajectory; restore is silent; recapture is normalized.
    /// </summary>
    public class KineticStorageSaveTests
    {
        private const string ClassId = "flywheel_save_500";
        private const string InstanceId = "flywheel_flywheel_save_500_room_a";

        private static KineticFlywheelCatalog MakeCatalog()
        {
            return new KineticFlywheelCatalog
            {
                flywheel_classes = new List<FlywheelClassDef>
                {
                    new FlywheelClassDef
                    {
                        flywheel_id = ClassId,
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
                surge_events = new List<SurgeEventDef>(),
                black_start = new BlackStartDef(),
                containment_hazard = new ContainmentHazardDef()
            };
        }

        private static KineticStorageSystem Create(int seed)
        {
            return new KineticStorageSystem(MakeCatalog(), new SeededRng(seed));
        }

        private static void InstallAndCharge(KineticStorageSystem sys)
        {
            sys.InstallFlywheel(ClassId, "room_a", 1, (itemId, amount) => true);
            sys.BringOnline(InstanceId);
            sys.Charge(InstanceId, 15f, 600f);
        }

        [Fact]
        public void RestoreAtNonZeroRpm_PreservesExactState()
        {
            var sys = Create(10);
            InstallAndCharge(sys);
            var captured = sys.CaptureState();

            var restored = Create(999); // different seed: state must come from the save, not the rng
            restored.RestoreState(captured);

            var a = sys.FindFlywheel(InstanceId)!;
            var b = restored.FindFlywheel(InstanceId)!;
            Assert.Equal(a.rotorRpm, b.rotorRpm, 4);
            Assert.Equal(a.storedEnergyJ, b.storedEnergyJ, 2);
            Assert.Equal(a.bearingTemperatureC, b.bearingTemperatureC, 4);
            Assert.Equal(a.vacuumPressureTorr, b.vacuumPressureTorr, 10);
            Assert.True(b.storedEnergyJ > 0f, "restore must not reset RPM/energy to zero");
        }

        [Fact]
        public void Restore_ContinuesIdenticalTrajectory()
        {
            // Control: no interruption.
            var control = Create(10);
            InstallAndCharge(control);
            for (int i = 0; i < 11; i++) control.Tick(60f);

            // Saved: capture mid-flight, restore, same 10 future ticks with the same seed.
            var saved = Create(10);
            InstallAndCharge(saved);
            saved.Tick(60f);
            var captured = saved.CaptureState();

            var restored = Create(321);
            restored.RestoreState(captured);
            for (int i = 0; i < 10; i++) restored.Tick(60f);

            var a = control.FindFlywheel(InstanceId)!;
            var b = restored.FindFlywheel(InstanceId)!;
            Assert.Equal(a.storedEnergyJ, b.storedEnergyJ, 1);
            Assert.Equal(a.rotorRpm, b.rotorRpm, 3);
            Assert.Equal(a.bearingTemperatureC, b.bearingTemperatureC, 3);
        }

        [Fact]
        public void Restore_IsSilent_NoEvents()
        {
            var sys = Create(10);
            InstallAndCharge(sys);
            var captured = sys.CaptureState();

            int installed = 0, overspeed = 0, discharged = 0, failed = 0, changed = 0;
            var restored = Create(77);
            restored.OnFlywheelInstalled += _ => installed++;
            restored.OnFlywheelOverspeed += _ => overspeed++;
            restored.OnFlywheelDischarged += _ => discharged++;
            restored.OnFlywheelFailure += _ => failed++;
            restored.OnStorageChanged += () => changed++;
            restored.RestoreState(captured);

            Assert.Equal(0, installed);
            Assert.Equal(0, overspeed);
            Assert.Equal(0, discharged);
            Assert.Equal(0, failed);
            Assert.Equal(0, changed);
        }

        [Fact]
        public void Restore_PreservesFailureState()
        {
            var sys = Create(17);
            InstallAndCharge(sys);
            sys.FindFlywheel(InstanceId)!.storedEnergyJ =
                KineticStorageSystem.ComputeStoredEnergyJ(
                    KineticStorageSystem.ComputeMomentOfInertia(500f, 0.45f, 0.5f), 24000f);
            sys.Tick(1f);
            var failedBefore = sys.FindFlywheel(InstanceId)!.hasFailed;
            var captured = sys.CaptureState();

            var restored = Create(42);
            restored.RestoreState(captured);

            Assert.Equal(failedBefore, restored.FindFlywheel(InstanceId)!.hasFailed);
            if (failedBefore)
            {
                Assert.True(restored.FindFlywheel(InstanceId)!.rotorHealth <= 0f);
            }
        }

        [Fact]
        public void Recapture_AfterRestore_IsNormalized()
        {
            var sys = Create(10);
            InstallAndCharge(sys);
            var first = sys.CaptureState();

            var restored = Create(55);
            restored.RestoreState(first);
            var second = restored.CaptureState();

            Assert.Equal(
                new SystemTextJsonSerializer().Serialize(first),
                new SystemTextJsonSerializer().Serialize(second));
        }
    }
}
