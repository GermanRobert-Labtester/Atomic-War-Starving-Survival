// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Plan122Sofc
{
    /// <summary>
    /// Plan 122 Phase 3 — SOFC operating contract: startup (no instant on),
    /// rated cap, efficiency, fuel-quality degradation, thermal derating,
    /// seal/stack wear, CHP waste heat, low acoustic signature, bounded
    /// skill modifier, safe shutdown, deterministic faults, maintenance,
    /// and the 180-day baseload soak + generator-dominance characterization.
    /// </summary>
    public sealed class Plan122SofcElectrochemistryEngineTests
    {
        private static SofcPowerCatalog CreateCatalog()
        {
            var catalog = new SofcPowerCatalog
            {
                stack_profiles =
                {
                    new SofcStackProfile
                    {
                        id = "test_stack", display_name = "Test Stack", fuel_profile_id = "fuel_clean",
                        grade_profile_id = "grade_std", waste_heat_profile_id = "heat_std",
                        maintenance_profile_id = "maint_std",
                        rated_power_kw = 20f, fuel_efficiency = 0.6f, waste_heat_units_per_tick = 8f,
                        startup_ticks = 4, cooldown_ticks = 3,
                        thermal_band = new SofcThermalBand { minimum = 0.5f, optimal_min = 0.7f, optimal_max = 0.9f, maximum = 1f },
                        degradation_per_operating_tick_bp = 15,
                        acoustic_signature_class = SofcPowerCatalog.SignatureVeryLow
                    }
                },
                fuel_profiles =
                {
                    new SofcFuelProfile { id = "fuel_dirty", display_name = "Dirty", quality_class = SofcPowerCatalog.QualityDirty, output_modifier_bp = 8000, degradation_multiplier_bp = 22000, fault_risk_bp = 350, feedstock_item_ids = { "item_biofuel_low_grade" } },
                    new SofcFuelProfile { id = "fuel_treated", display_name = "Treated", quality_class = SofcPowerCatalog.QualityTreated, output_modifier_bp = 9500, degradation_multiplier_bp = 12000, fault_risk_bp = 100, feedstock_item_ids = { "item_biofuel_generator_grade" } },
                    new SofcFuelProfile { id = "fuel_clean", display_name = "Clean", quality_class = SofcPowerCatalog.QualityClean, output_modifier_bp = 10000, degradation_multiplier_bp = 10000, fault_risk_bp = 20, feedstock_item_ids = { "item_biofuel_high_grade" } }
                },
                grade_profiles =
                {
                    new SofcGradeProfile { id = "grade_std", display_name = "Standard", stack_health_bonus_bp = 0, seal_integrity_bonus_bp = 0 },
                    new SofcGradeProfile { id = "grade_reinforced", display_name = "Reinforced", stack_health_bonus_bp = 400, seal_integrity_bonus_bp = 250 }
                },
                waste_heat_profiles =
                {
                    new SofcWasteHeatProfile { id = "heat_std", display_name = "Plenum", heat_kw_per_unit = 1.5f, max_allocatable_kw = 12f }
                },
                maintenance_profiles =
                {
                    new SofcMaintenanceProfile { id = "maint_std", display_name = "Standard", inspection_interval_days = 14, repair_skill_minimum = 45 }
                }
            };
            catalog.Index();
            return catalog;
        }

        private static SofcElectrochemistryEngine CreateOnlineEngine(ISeededRng? rng = null, float requested = 10f)
        {
            var engine = new SofcElectrochemistryEngine(CreateCatalog()) { Rng = rng };
            Assert.True(engine.Install("test_stack", partsAvailable: true).IsSuccess);
            Assert.True(engine.StartPreheat(fuelAvailable: true).IsSuccess);
            // Advance startup to Online.
            for (int i = 0; i < 10; i++)
            {
                var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = requested });
                if (r.Mode is SofcOperatingMode.Online or SofcOperatingMode.Derated) break;
            }
            Assert.True(engine.State.Mode is SofcOperatingMode.Online or SofcOperatingMode.Derated,
                $"engine did not reach Online during startup (mode={engine.State.Mode})");
            return engine;
        }

        private static void AdvanceStartup(SofcElectrochemistryEngine engine, int maxTicks = 12)
        {
            for (int i = 0; i < maxTicks; i++)
            {
                var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 10f });
                if (r.Mode is SofcOperatingMode.Online or SofcOperatingMode.Derated) return;
            }
            Assert.Fail("startup never reached Online");
        }

        internal sealed class SeededRngStub : ISeededRng
        {
            public SeededRngStub(int seed, int alwaysValue = 9999)
            {
                Seed = seed; _alwaysValue = alwaysValue;
            }
            private readonly int _alwaysValue;
            public int Seed { get; }
            public int Next(int minInclusive, int maxExclusive)
                => Math.Clamp(_alwaysValue, minInclusive, maxExclusive - 1);
            public float NextFloat() => _alwaysValue / 10000f;
            public double NextDouble() => _alwaysValue / 10000.0;
        }

        [Fact]
        public void No_fuel_means_no_output_and_no_fuel_request()
        {
            var engine = CreateOnlineEngine();
            engine.State.StackHealthBp = 10000;
            var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = false, RequestedOutputKw = 10f });
            Assert.Equal(SofcFailureCodes.FuelUnavailable, r.FailureCode);
            Assert.Equal(0f, r.FuelUnitsRequested);
        }

        [Fact]
        public void Startup_is_graded_never_instant()
        {
            var engine = new SofcElectrochemistryEngine(CreateCatalog());
            Assert.True(engine.Install("test_stack", partsAvailable: true).IsSuccess);
            Assert.True(engine.StartPreheat(fuelAvailable: true).IsSuccess);
            Assert.Equal(SofcOperatingMode.Preheating, engine.State.Mode);

            var r1 = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true });
            Assert.NotEqual(SofcOperatingMode.Online, r1.Mode); // tick 1: still heating

            int onlineTick = -1;
            for (int i = 2; i <= 12; i++)
            {
                var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 5f });
                if (r.Mode is SofcOperatingMode.Online or SofcOperatingMode.Derated) { onlineTick = i; break; }
            }
            Assert.True(onlineTick >= 4, $"reached Online too early at tick {onlineTick} (startup_ticks=4)");
        }

        [Fact]
        public void Output_is_capped_at_rated_power()
        {
            var engine = CreateOnlineEngine();
            engine.State.StackHealthBp = 10000;
            engine.State.SealIntegrityBp = 10000;
            var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 500f });
            Assert.True(r.AvailableOutputKw <= 20f + 0.0001f, $"available {r.AvailableOutputKw} exceeds rated 20");
            Assert.True(r.DispatchedOutputKw <= 20f + 0.0001f);
        }

        [Fact]
        public void Fuel_draw_follows_efficiency()
        {
            var engine = CreateOnlineEngine();
            engine.State.StackHealthBp = 10000;
            engine.State.SealIntegrityBp = 10000;
            var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 12f });
            // dispatched / effective_efficiency where effective = 0.6 * 1.0 (pristine stack)
            float expected = r.DispatchedOutputKw / 0.6f;
            Assert.True(Math.Abs(r.FuelUnitsRequested - expected) < 0.5f,
                $"fuel draw {r.FuelUnitsRequested} not near dispatched/efficiency {expected}");
        }

        [Fact]
        public void Dirty_fuel_degrades_faster_than_clean()
        {
            var dirty = CreateOnlineEngine(new SeededRngStub(42, 9999));
            dirty.State.StackHealthBp = 10000;
            dirty.State.SealIntegrityBp = 10000;
            var clean = CreateOnlineEngine(new SeededRngStub(42, 9999));
            clean.State.StackHealthBp = 10000;
            clean.State.SealIntegrityBp = 10000;

            for (int i = 0; i < 30; i++)
            {
                dirty.AdvanceTick(new SofcTickInput { FuelAvailable = true, FuelQualityClass = SofcPowerCatalog.QualityDirty, RequestedOutputKw = 10f });
                clean.AdvanceTick(new SofcTickInput { FuelAvailable = true, FuelQualityClass = SofcPowerCatalog.QualityClean, RequestedOutputKw = 10f });
            }
            Assert.True(dirty.State.StackHealthBp < clean.State.StackHealthBp,
                $"dirty-fuel health {dirty.State.StackHealthBp} should be below clean {clean.State.StackHealthBp}");
            Assert.True(dirty.State.SealIntegrityBp < clean.State.SealIntegrityBp,
                "dirty fuel must scour seals faster");
        }

        [Fact]
        public void Cold_stack_produces_nothing()
        {
            var engine = new SofcElectrochemistryEngine(CreateCatalog());
            Assert.True(engine.Install("test_stack", partsAvailable: true).IsSuccess);
            // Never preheated: thermal level 0. Direct online-mode tick is
            // impossible from Offline; the Offline tick reports no output.
            var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 10f });
            Assert.Equal(0f, r.AvailableOutputKw);
            Assert.Equal(SofcOperatingMode.Offline, r.Mode);
        }

        [Fact]
        public void Thermal_cycles_cost_seal_integrity()
        {
            var engine = CreateOnlineEngine();
            int sealBefore = engine.State.SealIntegrityBp;
            // Shutdown → cooling → restart → online (one thermal cycle).
            Assert.True(engine.Shutdown().IsSuccess);
            for (int i = 0; i < 40 && engine.State.Mode != SofcOperatingMode.Offline; i++)
                engine.AdvanceTick(new SofcTickInput { FuelAvailable = true });
            Assert.True(engine.StartPreheat(fuelAvailable: true).IsSuccess);
            AdvanceStartup(engine);
            Assert.True(engine.State.ThermalCycles >= 1, "thermal cycle not counted");
            Assert.True(engine.State.SealIntegrityBp < sealBefore, "seal integrity must drop per thermal cycle");
        }

        [Fact]
        public void Waste_heat_is_reported_and_capped()
        {
            var engine = CreateOnlineEngine();
            engine.State.StackHealthBp = 10000;
            engine.State.SealIntegrityBp = 10000;
            var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 20f });
            Assert.True(r.WasteHeatUnits > 0f, "online plant must produce waste heat");
            Assert.True(r.WasteHeatKw <= 12f + 0.0001f, $"waste heat {r.WasteHeatKw} exceeds allocatable cap 12");
        }

        [Fact]
        public void Acoustic_signature_is_low_not_undetectable()
        {
            var engine = CreateOnlineEngine();
            var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 10f });
            Assert.Equal(SofcPowerCatalog.SignatureVeryLow, r.AcousticSignatureClass);
            Assert.NotEqual("undetectable", r.AcousticSignatureClass);
            Assert.NotEqual("none", r.AcousticSignatureClass);
        }

        [Fact]
        public void Skill_reduces_degradation_within_bounds()
        {
            var skilled = CreateOnlineEngine(new SeededRngStub(7, 9999));
            skilled.State.StackHealthBp = 10000;
            var unskilled = CreateOnlineEngine(new SeededRngStub(7, 9999));
            unskilled.State.StackHealthBp = 10000;

            for (int i = 0; i < 50; i++)
            {
                skilled.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 10f, EngineeringSkillLevel = 100f });
                unskilled.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 10f, EngineeringSkillLevel = 0f });
            }
            Assert.True(skilled.State.StackHealthBp > unskilled.State.StackHealthBp,
                "skilled operator must wear the stack slower");
            Assert.True(skilled.State.StackHealthBp < 10000, "skill must not eliminate degradation");
        }

        [Fact]
        public void Shutdown_cools_before_offline_and_produces_nothing()
        {
            var engine = CreateOnlineEngine();
            Assert.True(engine.Shutdown().IsSuccess);
            Assert.Equal(SofcOperatingMode.CoolingDown, engine.State.Mode);
            var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 10f });
            Assert.Equal(0f, r.AvailableOutputKw);
            int guard = 0;
            while (engine.State.Mode != SofcOperatingMode.Offline && guard++ < 40)
                engine.AdvanceTick(new SofcTickInput { FuelAvailable = true });
            Assert.Equal(SofcOperatingMode.Offline, engine.State.Mode);
        }

        [Fact]
        public void Faulted_requires_maintenance_before_restart()
        {
            var engine = CreateOnlineEngine();
            engine.State.Mode = SofcOperatingMode.Faulted;
            engine.State.FaultCode = SofcFailureCodes.StackFaulted;
            var startBlocked = engine.StartPreheat(fuelAvailable: true);
            Assert.True(startBlocked.IsFailure);
            Assert.Equal(SofcFailureCodes.MaintenanceRequired, startBlocked.FailureCode);

            var maintained = engine.PerformMaintenance(partsAvailable: true, engineeringSkillLevel: 50f);
            Assert.True(maintained.IsSuccess);
            Assert.Equal(SofcOperatingMode.Offline, engine.State.Mode);
            Assert.True(engine.StartPreheat(fuelAvailable: true).IsSuccess);
        }

        [Fact]
        public void Deterministic_fault_behavior_same_seed_same_outcome()
        {
            // Reach Online with a non-faulting stub, then swap in the
            // always-zero roll so the first dirty-fuel tick faults.
            var a = CreateOnlineEngine(new SeededRngStub(99, 9999));
            a.Rng = new SeededRngStub(99, 0);
            var b = CreateOnlineEngine(new SeededRngStub(99, 9999));
            b.Rng = new SeededRngStub(99, 0);
            SofcGenerationResult? ra = null, rb = null;
            for (int i = 0; i < 6; i++)
            {
                ra = a.AdvanceTick(new SofcTickInput { FuelAvailable = true, FuelQualityClass = SofcPowerCatalog.QualityDirty, RequestedOutputKw = 10f });
                rb = b.AdvanceTick(new SofcTickInput { FuelAvailable = true, FuelQualityClass = SofcPowerCatalog.QualityDirty, RequestedOutputKw = 10f });
                if (ra.Mode == SofcOperatingMode.Faulted) break;
            }
            Assert.NotNull(ra);
            Assert.NotNull(rb);
            Assert.Equal(SofcOperatingMode.Faulted, ra!.Mode);
            Assert.Equal(ra.Mode, rb!.Mode);
            Assert.Equal(ra.FailureCode, rb.FailureCode);
        }

        [Fact]
        public void Degraded_stack_runs_derated()
        {
            var engine = CreateOnlineEngine();
            engine.State.StackHealthBp = Plan122SofcElectrochemistryEngineTests_Derated;
            var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 10f });
            Assert.True(r.Mode == SofcOperatingMode.Derated || r.StackHealthClass == SofcStackHealth.Worn
                || r.StackHealthClass == SofcStackHealth.Critical,
                $"expected derated/worn state, got mode={r.Mode} class={r.StackHealthClass}");
        }

        private const int Plan122SofcElectrochemistryEngineTests_Derated = 3500;

        [Fact]
        public void Soak_180_days_remains_bounded_and_stable()
        {
            var engine = CreateOnlineEngine(new SeededRngStub(2026, 5000));
            engine.State.StackHealthBp = 10000;
            engine.State.SealIntegrityBp = 10000;
            var rng = new System.Random(2026); // test-only load profile variation
            for (int day = 0; day < 180 * 4; day++)
            {
                var r = engine.AdvanceTick(new SofcTickInput
                {
                    FuelAvailable = true,
                    FuelQualityClass = day % 40 < 4 ? SofcPowerCatalog.QualityDirty : SofcPowerCatalog.QualityClean,
                    RequestedOutputKw = 10f + (float)(rng!.NextDouble() * 4.0)
                });
                Assert.True(engine.State.StackHealthBp >= 0 && engine.State.StackHealthBp <= 10000, "health out of bounds");
                Assert.True(engine.State.SealIntegrityBp >= 0 && engine.State.SealIntegrityBp <= 10000, "seal out of bounds");
                Assert.True(engine.State.ThermalLevel >= 0f && engine.State.ThermalLevel <= 1f, "thermal level out of bounds");
                Assert.True(r.AvailableOutputKw >= 0f && r.AvailableOutputKw <= 20f, "output out of bounds");
                Assert.False(float.IsNaN(engine.State.ThermalLevel) || float.IsInfinity(engine.State.ThermalLevel), "thermal NaN/Inf");
            }
            Assert.True(engine.State.StackHealthBp < 10000, "180-day soak must degrade the stack");
        }

        [Fact]
        public void Generator_dominance_characterization()
        {
            // Characterization (plan §11.1 / test 20): SOFC baseload economy vs
            // the legacy reciprocating baseline. The legacy grid baseline is
            // 0.30 kW-per-fuel-unit effective efficiency (documented constant;
            // authoritative generator profiles live in the power grid owner).
            var engine = CreateOnlineEngine();
            engine.State.StackHealthBp = 10000;
            engine.State.SealIntegrityBp = 10000;
            var r = engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 10f });
            float legacyBaseline = 0.30f;
            Assert.True(r.FuelUnitsRequested / Math.Max(0.01f, r.DispatchedOutputKw) < 1f / legacyBaseline,
                $"SOFC fuel intensity must beat the legacy generator baseline");
            // Diesel remains strategically relevant: SOFC cannot surge above
            // its (clamped, degraded) rating and takes 4+ ticks to start —
            // covered by Startup_is_graded_never_instant and the cap test.
        }

        [Fact]
        public void Same_seed_same_state_same_sequence_identical()
        {
            var a = CreateOnlineEngine(new SeededRngStub(555, 4200));
            a.State.StackHealthBp = 10000;
            var b = CreateOnlineEngine(new SeededRngStub(555, 4200));
            b.State.StackHealthBp = 10000;

            for (int i = 0; i < 40; i++)
            {
                var ra = a.AdvanceTick(new SofcTickInput { FuelAvailable = true, FuelQualityClass = i % 10 == 0 ? SofcPowerCatalog.QualityDirty : SofcPowerCatalog.QualityClean, RequestedOutputKw = 10f + i % 5 });
                var rb = b.AdvanceTick(new SofcTickInput { FuelAvailable = true, FuelQualityClass = i % 10 == 0 ? SofcPowerCatalog.QualityDirty : SofcPowerCatalog.QualityClean, RequestedOutputKw = 10f + i % 5 });
                Assert.Equal(ra.DispatchedOutputKw, rb.DispatchedOutputKw);
                Assert.Equal(a.State.StackHealthBp, b.State.StackHealthBp);
                Assert.Equal(a.State.Mode, b.State.Mode);
            }
        }
    }
}
