// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Plan141Expeditions
{
    /// <summary>
    /// Plan 141 Phase 1 — run-flat wheel contract: installation gating, hazard
    /// reduction without immunity, severe damage, rim/bead hits, rolling-
    /// resistance fuel cost, deterministic heat, maintenance, save/load.
    /// </summary>
    public sealed class Plan141RunFlatTireTests
    {
        private static RunFlatTireCatalog CreateCatalog()
        {
            var catalog = new RunFlatTireCatalog
            {
                runflat_profiles =
                {
                    new RunFlatProfileDef
                    {
                        id = "utility", display_name = "Utility",
                        compatible_vehicle_tags = { "road_truck", "expedition_rover" },
                        puncture_resistance_bp = 4000, sidewall_damage_resistance_bp = 1000,
                        rolling_resistance_modifier_bp = 500, heat_generation_rate_bp = 350,
                        safe_speed_profile_kph = 70, repairability_bp = 6000,
                        required_item_ids = { "kit_utility" }
                    },
                    new RunFlatProfileDef
                    {
                        id = "armored", display_name = "Armored",
                        compatible_vehicle_tags = { "armored_car" },
                        puncture_resistance_bp = 8800, sidewall_damage_resistance_bp = 6500,
                        rolling_resistance_modifier_bp = 1400, heat_generation_rate_bp = 750,
                        safe_speed_profile_kph = 55, repairability_bp = 2500,
                        required_item_ids = { "kit_armored" }
                    }
                }
            };
            catalog.Index();
            return catalog;
        }

        private static RunFlatTireEngine CreateEngine(ISeededRng? rng = null)
        {
            var engine = new RunFlatTireEngine(CreateCatalog());
            engine.Rng = rng;
            return engine;
        }

        [Fact]
        public void Compatible_installation_succeeds()
        {
            var engine = CreateEngine();
            var result = engine.Install("v1", "road_truck", "utility", workshopAvailable: true, partsAvailable: true, skill: 0.5);
            Assert.True(result.IsSuccess);
            Assert.NotNull(engine.FindWheelSet("v1"));
        }

        [Fact]
        public void Incompatible_vehicle_is_refused()
        {
            var engine = CreateEngine();
            var result = engine.Install("v1", "steam_locomotive", "utility", true, true, 0.5);
            Assert.True(result.IsFailure);
            Assert.Equal("vehicle_incompatible", result.FailureCode);
        }

        [Fact]
        public void Missing_parts_or_workshop_is_refused()
        {
            var engine = CreateEngine();
            Assert.Equal("upgrade_parts_missing",
                engine.Install("v1", "road_truck", "utility", true, false, 0.5).FailureCode);
            Assert.Equal("workshop_unavailable",
                engine.Install("v1", "road_truck", "utility", false, true, 0.5).FailureCode);
            Assert.Null(engine.FindWheelSet("v1"));
        }

        [Fact]
        public void Installation_skill_improves_integrity_and_balance()
        {
            var low = CreateEngine();
            low.Install("v1", "road_truck", "utility", true, true, 0.0);
            var high = CreateEngine();
            high.Install("v1", "road_truck", "utility", true, true, 1.0);

            Assert.True(high.FindWheelSet("v1")!.IntegrityBp >= low.FindWheelSet("v1")!.IntegrityBp);
            Assert.True(high.FindWheelSet("v1")!.ImbalanceBp <= low.FindWheelSet("v1")!.ImbalanceBp);
        }

        [Fact]
        public void Runflat_reduces_puncture_loss_versus_weaker_profile()
        {
            var utility = CreateEngine();
            utility.Install("v1", "road_truck", "utility", true, true, 0.5);
            utility.ApplyHazard("v1", "glass", 40, 20);
            int utilityLoss = 100 - utility.FindWheelSet("v1")!.IntegrityBp;

            var armored = CreateEngine();
            armored.Install("v1", "armored_car", "armored", true, true, 0.5);
            armored.ApplyHazard("v1", "glass", 40, 20);
            int armoredLoss = 100 - armored.FindWheelSet("v1")!.IntegrityBp;

            Assert.True(armoredLoss < utilityLoss);
        }

        [Fact]
        public void Severe_hazard_always_causes_some_damage()
        {
            var engine = CreateEngine();
            engine.Install("v1", "armored_car", "armored", true, true, 1.0);
            engine.ApplyHazard("v1", "severe", 30, 20);
            Assert.True(engine.FindWheelSet("v1")!.IntegrityBp < 100, "No run-flat is invulnerable.");
        }

        [Fact]
        public void Repeated_severe_hazards_can_destroy_a_wheel()
        {
            var engine = CreateEngine(new SeededRngStub(0));
            engine.Install("v1", "armored_car", "armored", true, true, 1.0);
            ActionResult last = ActionResult.Success("test");
            for (int i = 0; i < 20 && !string.Equals(last.FailureCode, "wheel_damaged", StringComparison.Ordinal); i++)
                last = engine.ApplyHazard("v1", "severe", 30, 20);

            Assert.Equal("wheel_damaged", last.FailureCode);
            Assert.Equal(0, engine.FindWheelSet("v1")!.IntegrityBp);
        }

        [Fact]
        public void Severe_impact_can_bend_rim_and_bead()
        {
            var engine = CreateEngine(new SeededRngStub(0));
            engine.Install("v1", "road_truck", "utility", true, true, 0.0);
            engine.ApplyHazard("v1", "spike", 30, 20);
            Assert.True(engine.FindWheelSet("v1")!.RimBeadBp < 100);
        }

        [Fact]
        public void Rolling_resistance_costs_fuel()
        {
            var engine = CreateEngine();
            engine.Install("v1", "armored_car", "armored", true, true, 0.5);
            Assert.True(engine.GetFuelPenaltyPct("v1") > 0);
            Assert.True(engine.GetRollingResistanceModifierBp("v1") > 0);
        }

        [Fact]
        public void Heat_accumulates_with_speed_and_cools_when_stopped()
        {
            var engine = CreateEngine();
            engine.Install("v1", "armored_car", "armored", true, true, 0.5);
            int start = engine.FindWheelSet("v1")!.HeatC;

            for (int i = 0; i < 10; i++) engine.TickHeat("v1", 90, 80, 20);
            int hot = engine.FindWheelSet("v1")!.HeatC;
            Assert.True(hot > start);

            for (int i = 0; i < 10; i++) engine.TickHeat("v1", 0, 0, 20);
            Assert.True(engine.FindWheelSet("v1")!.HeatC < hot);
        }

        [Fact]
        public void Hot_ambient_raises_heat_faster_than_mild_ambient()
        {
            var mild = CreateEngine();
            mild.Install("v1", "armored_car", "armored", true, true, 0.5);
            mild.TickHeat("v1", 60, 50, 20);

            var hot = CreateEngine();
            hot.Install("v1", "armored_car", "armored", true, true, 0.5);
            hot.TickHeat("v1", 60, 50, 40);

            Assert.True(hot.FindWheelSet("v1")!.HeatC > mild.FindWheelSet("v1")!.HeatC);
        }

        [Fact]
        public void Overheating_degrades_wheel_and_lowers_safe_speed()
        {
            var engine = CreateEngine();
            engine.Install("v1", "armored_car", "armored", true, true, 0.5);
            int cleanSpeed = engine.GetSafeSpeedKph("v1");

            for (int i = 0; i < 40; i++) engine.TickHeat("v1", 100, 100, 45);
            var wheel = engine.FindWheelSet("v1")!;

            Assert.True(wheel.HeatC >= RunFlatTireEngine.OverheatThresholdC);
            Assert.True(wheel.WearBp > 0);
            Assert.True(engine.GetSafeSpeedKph("v1") < cleanSpeed);
        }

        [Fact]
        public void Overloaded_vehicle_heats_faster()
        {
            var light = CreateEngine();
            light.Install("v1", "armored_car", "armored", true, true, 0.5);
            light.TickHeat("v1", 60, 0, 20);

            var heavy = CreateEngine();
            heavy.Install("v1", "armored_car", "armored", true, true, 0.5);
            heavy.TickHeat("v1", 60, 150, 20);

            Assert.True(heavy.FindWheelSet("v1")!.HeatC > light.FindWheelSet("v1")!.HeatC);
        }

        [Fact]
        public void Maintenance_restores_bounded_integrity()
        {
            var engine = CreateEngine(new SeededRngStub(0));
            engine.Install("v1", "road_truck", "utility", true, true, 0.5);
            engine.ApplyHazard("v1", "spike", 30, 20);
            int damaged = engine.FindWheelSet("v1")!.IntegrityBp;

            Assert.True(engine.Repair("v1", true, 0.5).IsSuccess);
            Assert.True(engine.FindWheelSet("v1")!.IntegrityBp >= damaged);
            Assert.True(engine.FindWheelSet("v1")!.IntegrityBp <= 100);
        }

        [Fact]
        public void Hazard_without_runflat_is_refused()
        {
            var engine = CreateEngine();
            var result = engine.ApplyHazard("no_such_vehicle", "glass", 40, 20);
            Assert.True(result.IsFailure);
            Assert.Equal("maintenance_required", result.FailureCode);
        }

        [Fact]
        public void Save_load_round_trip_preserves_wheel_state()
        {
            var engine = CreateEngine();
            engine.Install("v1", "road_truck", "utility", true, true, 0.5);
            engine.ApplyHazard("v1", "rubble", 50, 20);
            engine.TickHeat("v1", 80, 40, 30);

            var restored = CreateEngine();
            restored.RestoreState(engine.CaptureState());

            var before = engine.FindWheelSet("v1")!;
            var after = restored.FindWheelSet("v1")!;
            Assert.Equal(before.IntegrityBp, after.IntegrityBp);
            Assert.Equal(before.HeatC, after.HeatC);
            Assert.Equal(before.PunctureCount, after.PunctureCount);
            Assert.Equal(before.RimBeadBp, after.RimBeadBp);
        }

        [Fact]
        public void Old_save_baseline_has_no_fabricated_wheels()
        {
            var restored = CreateEngine();
            restored.RestoreState(new RunFlatTireState());
            Assert.Empty(restored.State.WheelSets);
            Assert.Equal(0, restored.GetSafeSpeedKph("v1"));
            Assert.Equal(0, restored.GetFuelPenaltyPct("v1"));
        }

        [Fact]
        public void Hazard_and_heat_are_deterministic_without_rng()
        {
            int RunOnce()
            {
                var engine = CreateEngine();
                engine.Install("v1", "road_truck", "utility", true, true, 0.0);
                for (int i = 0; i < 5; i++)
                {
                    engine.ApplyHazard("v1", "spike", 40, 20);
                    engine.TickHeat("v1", 70, 60, 25);
                }
                return engine.FindWheelSet("v1")!.IntegrityBp * 1000
                     + engine.FindWheelSet("v1")!.HeatC * 10
                     + engine.FindWheelSet("v1")!.RimBeadBp;
            }

            Assert.Equal(RunOnce(), RunOnce());
        }

        internal sealed class SeededRngStub : ISeededRng
        {
            private readonly int _value;
            public SeededRngStub(int value) { _value = value; }
            public int Seed => _value;
            public int Next(int minInclusive, int maxExclusive) => _value;
            public float NextFloat() => _value / 10000f;
            public double NextDouble() => _value / 10000.0;
            public ISeededRng Fork(int day = 0, int actionIndex = 0) => this;
        }
    }
}
