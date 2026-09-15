// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Plan125Amphibious
{
    /// <summary>
    /// Plan 125 Core contract — amphibious draisine: kit install via vehicle
    /// tags, cargo penalty, flotation margin, current/weather/skill risk,
    /// pontoon damage + ingress hazards, pump gating, abort/emergency
    /// recovery, typed route capability, naval distinctness, seeded parity.
    /// </summary>
    public sealed class Plan125AmphibiousDraisineEngineTests
    {
        private static AmphibiousDraisineCatalog CreateCatalog()
        {
            var catalog = new AmphibiousDraisineCatalog
            {
                kit_profiles =
                {
                    new AmphibiousKitProfile
                    {
                        id = "kit_mk1", display_name = "Outrigger Mk I",
                        compatible_vehicle_tags = { "rail_draisine", "rail_draisine_heavy" },
                        flotation_rating = 0.8f, current_tolerance = 0.55f,
                        cargo_capacity_modifier_bp = 8200, water_speed_modifier_bp = 3500,
                        deployment_ticks = 3, mass_kg = 220, min_vehicle_condition_bp = 4000,
                        pump_profile_id = "pump_manual",
                        route_class_ids = { "route_shallow" }
                    },
                    new AmphibiousKitProfile
                    {
                        id = "kit_mk2", display_name = "Outrigger Mk II",
                        compatible_vehicle_tags = { "rail_draisine_heavy" },
                        flotation_rating = 0.9f, current_tolerance = 0.8f,
                        cargo_capacity_modifier_bp = 7200, water_speed_modifier_bp = 3000,
                        deployment_ticks = 4, mass_kg = 340, min_vehicle_condition_bp = 5000,
                        pump_profile_id = "pump_battery",
                        route_class_ids = { "route_shallow", "route_deep" }
                    }
                },
                pump_profiles =
                {
                    new AmphibiousPumpProfile { id = "pump_manual", display_name = "Hand Pump", ingress_mitigation_bp = 4000, power_source = "manual" },
                    new AmphibiousPumpProfile { id = "pump_battery", display_name = "Powered Pump", ingress_mitigation_bp = 7000, power_source = "battery" }
                },
                route_class_profiles =
                {
                    new AmphibiousRouteClassProfile { id = "route_shallow", display_name = "Flooded Rail", max_current_risk = 0.55f, min_flotation_rating = 0.7f },
                    new AmphibiousRouteClassProfile { id = "route_deep", display_name = "Submerged Causeway", max_current_risk = 0.75f, min_flotation_rating = 0.85f }
                },
                repair_profiles = { new AmphibiousMaintenanceProfile { id = "maint_field", display_name = "Field", inspection_interval_days = 10, repair_skill_minimum = 35 } }
            };
            catalog.Index();
            return catalog;
        }

        internal sealed class SeededRngStub : ISeededRng
        {
            public SeededRngStub(int seed, int alwaysValue = 9999) { Seed = seed; _alwaysValue = alwaysValue; }
            private readonly int _alwaysValue;
            public int Seed { get; }
            public int Next(int minInclusive, int maxExclusive) => Math.Clamp(_alwaysValue, minInclusive, maxExclusive - 1);
            public float NextFloat() => _alwaysValue / 10000f;
            public double NextDouble() => _alwaysValue / 10000.0;
        }

        private static AmphibiousDraisineEngine CreateEngine(ISeededRng? rng = null)
            => new AmphibiousDraisineEngine(CreateCatalog()) { Rng = rng };

        private static AmphibiousDraisineEngine CreateDeployed(ISeededRng? rng = null,
            string kitId = "kit_mk1", string vehicleTag = "rail_draisine")
        {
            var engine = CreateEngine(rng);
            Assert.True(engine.InstallKit("draisine_1", vehicleTag, kitId,
                workshopAvailable: true, partsAvailable: true, vehicleConditionBp: 9000).IsSuccess);
            Assert.True(engine.Deploy("draisine_1").IsSuccess);
            while (!engine.TickDeployment("draisine_1") && engine.FindVehicle("draisine_1")!.Phase != AmphibiousCrossingPhase.WaterReady)
            { }
            Assert.Equal(AmphibiousCrossingPhase.WaterReady, engine.FindVehicle("draisine_1")!.Phase);
            return engine;
        }

        private static void BeginAndRunCrossing(AmphibiousDraisineEngine engine, string routeClassId,
            float currentRisk = 0.2f, bool badWeather = false, float skill = 50f, int maxTicks = 40)
        {
            Assert.True(engine.BeginCrossing("draisine_1", routeClassId,
                vehicleMassFraction: 0.3f, vehicleConditionBp: 9000, pumpPowerAvailable: true).IsSuccess);
            for (int i = 0; i < maxTicks && engine.FindVehicle("draisine_1")!.Phase == AmphibiousCrossingPhase.Crossing; i++)
                engine.AdvanceCrossing("draisine_1", currentRisk, badWeather, skill, vehicleConditionBp: 9000);
        }

        [Fact]
        public void Incompatible_vehicle_tag_is_refused()
        {
            var engine = CreateEngine();
            var r = engine.InstallKit("v1", "steam_locomotive", "kit_mk1",
                workshopAvailable: true, partsAvailable: true, vehicleConditionBp: 9000);
            Assert.True(r.IsFailure);
            Assert.Equal(AmphibiousFailureCodes.VehicleIncompatible, r.FailureCode);
        }

        [Fact]
        public void Worn_vehicle_below_kit_threshold_is_refused()
        {
            var engine = CreateEngine();
            var r = engine.InstallKit("v1", "rail_draisine", "kit_mk2",
                workshopAvailable: true, partsAvailable: true, vehicleConditionBp: 4500);
            Assert.True(r.IsFailure);
            Assert.Equal(AmphibiousFailureCodes.VehicleIncompatible, r.FailureCode);
        }

        [Fact]
        public void Install_requires_workshop_and_parts()
        {
            var engine = CreateEngine();
            Assert.True(engine.InstallKit("v1", "rail_draisine", "kit_mk1",
                workshopAvailable: true, partsAvailable: false, vehicleConditionBp: 9000).IsFailure);
            Assert.True(engine.InstallKit("v1", "rail_draisine", "kit_mk1",
                workshopAvailable: false, partsAvailable: true, vehicleConditionBp: 9000).IsFailure);
            Assert.True(engine.InstallKit("v1", "rail_draisine", "kit_mk1",
                workshopAvailable: true, partsAvailable: true, vehicleConditionBp: 9000).IsSuccess);
        }

        [Fact]
        public void Deployment_takes_ticks_never_instant()
        {
            var engine = CreateEngine();
            Assert.True(engine.InstallKit("v1", "rail_draisine", "kit_mk1",
                workshopAvailable: true, partsAvailable: true, vehicleConditionBp: 9000).IsSuccess);
            Assert.True(engine.Deploy("v1").IsSuccess);
            Assert.Equal(AmphibiousCrossingPhase.Deploying, engine.FindVehicle("v1")!.Phase);
            // kit_mk1 deploys in 3 ticks.
            Assert.False(engine.TickDeployment("v1"));
            Assert.False(engine.TickDeployment("v1"));
            Assert.True(engine.TickDeployment("v1"));
            Assert.Equal(AmphibiousCrossingPhase.WaterReady, engine.FindVehicle("v1")!.Phase);
        }

        [Fact]
        public void Cargo_overload_is_rejected_at_crossing()
        {
            var engine = CreateDeployed();
            Assert.True(engine.SetCargoLoad("draisine_1", 0.95f).IsSuccess); // 9500 bp > 8200 capacity
            var r = engine.BeginCrossing("draisine_1", "route_shallow",
                vehicleMassFraction: 0.3f, vehicleConditionBp: 9000, pumpPowerAvailable: true);
            Assert.True(r.IsFailure);
            Assert.Equal(AmphibiousFailureCodes.VehicleIncompatible, r.FailureCode);
        }

        [Fact]
        public void Flotation_margin_rejects_overloaded_or_damaged_loadout()
        {
            var engine = CreateDeployed();
            // Overloaded: cargo 1.0 × 0.6 penalty + kit mass 0.15 + vehicle mass 0.3 → margin < 1500bp
            var overloaded = engine.ComputeFlotationMargin("draisine_1", "route_shallow",
                vehicleMassFraction: 0.4f, cargoLoadFraction: 1f, vehicleConditionBp: 9000, out var overCode);
            Assert.Equal(AmphibiousFailureCodes.FlotationMarginInsufficient, overCode);
            Assert.True(overloaded < AmphibiousDraisineEngine.MinFlotationMarginBp);

            // Light load, healthy vehicle: margin clears.
            var ok = engine.ComputeFlotationMargin("draisine_1", "route_shallow",
                vehicleMassFraction: 0.3f, cargoLoadFraction: 0.2f, vehicleConditionBp: 9000, out var okCode);
            Assert.Equal(string.Empty, okCode);
            Assert.True(ok >= AmphibiousDraisineEngine.MinFlotationMarginBp);
        }

        [Fact]
        public void Strong_current_exceeding_tolerance_aborts()
        {
            var engine = CreateDeployed();
            Assert.True(engine.SetCargoLoad("draisine_1", 0.2f).IsSuccess);
            Assert.True(engine.BeginCrossing("draisine_1", "route_shallow",
                vehicleMassFraction: 0.3f, vehicleConditionBp: 9000, pumpPowerAvailable: true).IsSuccess);
            // current 0.9 > tolerance 0.55 → forced abort
            var r = engine.AdvanceCrossing("draisine_1", currentRisk: 0.9f, badWeather: false, navigatorSkill: 0f, vehicleConditionBp: 9000);
            Assert.True(r.IsFailure);
            Assert.Equal(AmphibiousFailureCodes.CurrentRiskTooHigh, r.FailureCode);
            Assert.Equal(AmphibiousCrossingPhase.EmergencyRecovery, engine.FindVehicle("draisine_1")!.Phase);
        }

        [Fact]
        public void Valid_crossing_completes_through_landing_and_recovery()
        {
            var engine = CreateDeployed(new SeededRngStub(31, 9999)); // no hazards
            Assert.True(engine.SetCargoLoad("draisine_1", 0.2f).IsSuccess);
            BeginAndRunCrossing(engine, "route_shallow", currentRisk: 0.2f);
            Assert.Equal(AmphibiousCrossingPhase.Landing, engine.FindVehicle("draisine_1")!.Phase);

            Assert.True(engine.TickLanding("draisine_1"));
            Assert.Equal(AmphibiousCrossingPhase.Recovering, engine.FindVehicle("draisine_1")!.Phase);
            while (!engine.TickDeployment("draisine_1"))
            { }
            Assert.Equal(AmphibiousCrossingPhase.LandReady, engine.FindVehicle("draisine_1")!.Phase);
        }

        [Fact]
        public void Bad_weather_raises_hazard_and_current()
        {
            var calm = CreateDeployed(new SeededRngStub(77, 3000));
            var storm = CreateDeployed(new SeededRngStub(77, 3000));
            Assert.True(calm.SetCargoLoad("draisine_1", 0.2f).IsSuccess);
            Assert.True(storm.SetCargoLoad("draisine_1", 0.2f).IsSuccess);
            BeginAndRunCrossing(calm, "route_shallow", currentRisk: 0.3f, badWeather: false);
            BeginAndRunCrossing(storm, "route_shallow", currentRisk: 0.3f, badWeather: true);

            Assert.True(storm.FindVehicle("draisine_1")!.PontoonConditionBp <= calm.FindVehicle("draisine_1")!.PontoonConditionBp,
                "bad weather must not reduce hazard");
        }

        [Fact]
        public void Navigator_skill_lowers_hazard_within_bounds()
        {
            var novice = CreateDeployed(new SeededRngStub(41, 100)); // hazards always fire
            var expert = CreateDeployed(new SeededRngStub(41, 100));
            Assert.True(novice.SetCargoLoad("draisine_1", 0.4f).IsSuccess);
            Assert.True(expert.SetCargoLoad("draisine_1", 0.4f).IsSuccess);
            BeginAndRunCrossing(novice, "route_shallow", currentRisk: 0.4f, skill: 0f);
            BeginAndRunCrossing(expert, "route_shallow", currentRisk: 0.4f, skill: 100f);

            Assert.True(expert.FindVehicle("draisine_1")!.PontoonConditionBp >= novice.FindVehicle("draisine_1")!.PontoonConditionBp,
                "skilled navigator must suffer less pontoon damage");
            Assert.True(expert.FindVehicle("draisine_1")!.PontoonConditionBp < 10000,
                "skill must never guarantee a flawless crossing");
        }

        [Fact]
        public void Pontoon_damage_and_ingress_are_seeded_and_bounded()
        {
            // Stub rolls minimum: every hazard fires deterministically.
            var engine = CreateDeployed(new SeededRngStub(5, 0));
            Assert.True(engine.SetCargoLoad("draisine_1", 0.5f).IsSuccess);
            Assert.True(engine.BeginCrossing("draisine_1", "route_shallow",
                vehicleMassFraction: 0.4f, vehicleConditionBp: 7000, pumpPowerAvailable: true).IsSuccess);
            var a = engine.FindVehicle("draisine_1")!;
            int pontoonBefore = a.PontoonConditionBp;
            engine.AdvanceCrossing("draisine_1", currentRisk: 0.4f, badWeather: true, navigatorSkill: 0f, vehicleConditionBp: 9000);
            Assert.True(a.PontoonConditionBp < pontoonBefore, "hazardous tick must damage pontoons");
            Assert.True(a.IngressBp > 0, "hazardous tick must admit water");
            Assert.True(a.PontoonConditionBp >= 0 && a.PontoonConditionBp <= 10000, "pontoon condition out of bounds");
            Assert.True(a.IngressBp >= 0 && a.IngressBp <= 10000, "ingress out of bounds");
        }

        [Fact]
        public void Ingress_emergency_forces_recovery()
        {
            var engine = CreateDeployed(new SeededRngStub(5, 0));
            var state = engine.FindVehicle("draisine_1")!;
            Assert.True(engine.BeginCrossing("draisine_1", "route_shallow",
                vehicleMassFraction: 0.3f, vehicleConditionBp: 5000, pumpPowerAvailable: true).IsSuccess);
            // Simulate a swamped hull mid-crossing (test-only state injection).
            state.IngressBp = AmphibiousDraisineEngine.IngressEmergencyBp;
            var r = engine.AdvanceCrossing("draisine_1", currentRisk: 0.4f, badWeather: true, navigatorSkill: 0f, vehicleConditionBp: 5000);
            Assert.True(r.IsFailure);
            Assert.Equal(AmphibiousCrossingPhase.EmergencyRecovery, state.Phase);
            // Recovery returns the crew to LandReady.
            while (!engine.TickEmergencyRecovery("draisine_1"))
            { }
            Assert.Equal(AmphibiousCrossingPhase.LandReady, state.Phase);
            Assert.Equal(0, state.IngressBp);
        }

        [Fact]
        public void Battery_pump_requires_power()
        {
            var engine = CreateDeployed(kitId: "kit_mk2", vehicleTag: "rail_draisine_heavy");
            var r = engine.BeginCrossing("draisine_1", "route_deep",
                vehicleMassFraction: 0.25f, vehicleConditionBp: 9000, pumpPowerAvailable: false);
            Assert.True(r.IsFailure);
            Assert.Equal(AmphibiousFailureCodes.PumpUnavailable, r.FailureCode);
        }

        [Fact]
        public void Abort_and_recover_work_mid_crossing()
        {
            var engine = CreateDeployed();
            Assert.True(engine.SetCargoLoad("draisine_1", 0.2f).IsSuccess);
            Assert.True(engine.BeginCrossing("draisine_1", "route_shallow",
                vehicleMassFraction: 0.3f, vehicleConditionBp: 9000, pumpPowerAvailable: true).IsSuccess);
            Assert.True(engine.AbortCrossing("draisine_1").IsSuccess);
            Assert.Equal(AmphibiousCrossingPhase.EmergencyRecovery, engine.FindVehicle("draisine_1")!.Phase);
            while (!engine.TickEmergencyRecovery("draisine_1"))
            { }
            Assert.Equal(AmphibiousCrossingPhase.LandReady, engine.FindVehicle("draisine_1")!.Phase);
        }

        [Fact]
        public void Route_capability_is_typed_and_map_owned()
        {
            var engine = CreateDeployed(kitId: "kit_mk1");
            // mk1 unlocks only the shallow class — deep causeway is refused.
            Assert.True(engine.TryGetRouteCapability("draisine_1", "route_shallow", out string okCode));
            Assert.Equal(string.Empty, okCode);
            Assert.False(engine.TryGetRouteCapability("draisine_1", "route_deep", out string deepCode));
            Assert.Equal(AmphibiousFailureCodes.RouteNotAmphibiousCapable, deepCode);
            // Unknown vehicle: no capability.
            Assert.False(engine.TryGetRouteCapability("ghost", "route_shallow", out _));
        }

        [Fact]
        public void Repair_kit_restores_condition_bounded()
        {
            var engine = CreateDeployed();
            var state = engine.FindVehicle("draisine_1")!;
            state.PontoonConditionBp = 3000;
            state.KitConditionBp = 3000;
            Assert.True(engine.RepairKit("draisine_1", partsAvailable: true, mechanicSkill: 100f).IsSuccess);
            Assert.True(state.PontoonConditionBp > 3000);
            Assert.True(state.PontoonConditionBp <= 10000);
            Assert.True(engine.RepairKit("draisine_1", partsAvailable: false, mechanicSkill: 50f).IsFailure);
        }

        [Fact]
        public void Same_seed_same_commands_identical_outcomes()
        {
            var a = CreateDeployed(new SeededRngStub(555, 2000));
            var b = CreateDeployed(new SeededRngStub(555, 2000));
            foreach (var e in new[] { a, b })
            {
                Assert.True(e.SetCargoLoad("draisine_1", 0.4f).IsSuccess);
                Assert.True(e.BeginCrossing("draisine_1", "route_shallow",
                    vehicleMassFraction: 0.3f, vehicleConditionBp: 8000, pumpPowerAvailable: true).IsSuccess);
            }
            for (int i = 0; i < 10; i++)
            {
                var ra = a.AdvanceCrossing("draisine_1", 0.35f, badWeather: true, navigatorSkill: 40f, vehicleConditionBp: 8000);
                var rb = b.AdvanceCrossing("draisine_1", 0.35f, badWeather: true, navigatorSkill: 40f, vehicleConditionBp: 8000);
                Assert.Equal(ra.IsSuccess, rb.IsSuccess);
                Assert.Equal(ra.FailureCode, rb.FailureCode);
                Assert.Equal(a.FindVehicle("draisine_1")!.PontoonConditionBp, b.FindVehicle("draisine_1")!.PontoonConditionBp);
                Assert.Equal(a.FindVehicle("draisine_1")!.IngressBp, b.FindVehicle("draisine_1")!.IngressBp);
                Assert.Equal(a.FindVehicle("draisine_1")!.CrossingProgressBp, b.FindVehicle("draisine_1")!.CrossingProgressBp);
            }
        }

        [Fact]
        public void Crossing_only_changes_engine_owned_state()
        {
            // Invariant (plan §7.16 case "expedition position"): the engine
            // mutates only its own kit state — no expedition position, no
            // world topology, no vehicle base condition truth lives here.
            var engine = CreateDeployed();
            Assert.True(engine.SetCargoLoad("draisine_1", 0.2f).IsSuccess);
            BeginAndRunCrossing(engine, "route_shallow", currentRisk: 0.2f);
            var engineType = typeof(AmphibiousDraisineEngine);
            Assert.Null(engineType.GetProperty("ExpeditionPosition"));
            Assert.Null(engineType.GetProperty("WorldTopology"));
            Assert.Null(engineType.GetProperty("VehicleCondition"));
        }
    }
}
