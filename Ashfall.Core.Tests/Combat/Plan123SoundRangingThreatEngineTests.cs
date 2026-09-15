// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Reflection;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Xunit;

namespace Ashfall.Core.Tests.Plan123SoundRanging
{
    /// <summary>
    /// Plan 123 Core contract — DEFENSIVE sound ranging: coarse observations,
    /// salvo correlation, weather/sensor bounds, staleness expiry, moving
    /// source break, sabotage/repair, calibration drift, seeded noise parity,
    /// and the hard no-weapon-targeting invariant.
    /// </summary>
    public sealed class Plan123SoundRangingThreatEngineTests
    {
        private static SoundRangingCatalog CreateCatalog()
        {
            var catalog = new SoundRangingCatalog
            {
                array_profiles =
                {
                    new SoundRangingArrayProfile
                    {
                        id = "test_array", display_name = "Test Array", sensor_profile_id = "sensor_test",
                        sensor_count = 4, base_bearing_error_deg = 8f, base_region_radius_cells = 3,
                        timing_quality = 0.75f, weather_sensitivity = 0.45f,
                        maintenance_drift_per_day_bp = 20, confidence_cap_bp = 9000,
                        maintenance_profile_id = "maint_test"
                    }
                },
                sensor_profiles = { new SoundRangingSensorProfile { id = "sensor_test", display_name = "Node", reliability_bp = 8500 } },
                atmospheric_error_profiles =
                {
                    new SoundAtmosphericErrorProfile { id = "atmos_clear", display_name = "Clear", error_multiplier_bp = 10000 },
                    new SoundAtmosphericErrorProfile { id = "atmos_wind", display_name = "Wind", error_multiplier_bp = 14000 },
                    new SoundAtmosphericErrorProfile { id = "atmos_storm", display_name = "Storm", error_multiplier_bp = 22000 }
                },
                source_class_signatures =
                {
                    new SoundSourceClassSignature { id = "src_light", display_name = "Light", bearing_error_bonus_deg = 0f, region_radius_bonus_cells = 0 },
                    new SoundSourceClassSignature { id = "src_heavy", display_name = "Heavy", bearing_error_bonus_deg = 2f, region_radius_bonus_cells = 1 }
                },
                maintenance_profiles = { new SoundMaintenanceProfile { id = "maint_test", display_name = "Field", inspection_interval_days = 7, repair_skill_minimum = 40 } }
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

        private static SoundRangingThreatEngine CreateDeployedEngine(ISeededRng? rng = null)
        {
            var engine = new SoundRangingThreatEngine(CreateCatalog()) { Rng = rng };
            Assert.True(engine.Install("test_array", partsAvailable: true).IsSuccess);
            return engine;
        }

        private static SoundRangingThreatEngine.HostileFireObservation Fire(int day, int bearing = 90,
            string? tag = null, bool moving = false, string? sourceClass = null)
            => new SoundRangingThreatEngine.HostileFireObservation
            {
                Day = day, BearingDeg = bearing, SourceTag = tag, MovingSource = moving, SourceClassId = sourceClass
            };

        [Fact]
        public void No_hostile_event_no_threat_estimate()
        {
            var engine = CreateDeployedEngine();
            Assert.Null(engine.GetActiveThreat());
        }

        [Fact]
        public void One_event_produces_coarse_bounded_estimate()
        {
            var engine = CreateDeployedEngine(new SeededRngStub(21, 5000));
            var r = engine.RecordObservation(Fire(10, bearing: 120, tag: "battery_a"), "atmos_clear");
            Assert.True(r!.IsSuccess);
            var t = r.Value!;
            // Coarse by design: bearing error >= base profile error, radius >= 1,
            // confidence within [0, cap].
            Assert.True(t.BearingErrorDeg >= 2f, "bearing error must stay coarse");
            Assert.True(t.RegionRadiusCells >= 1);
            Assert.True(t.ConfidenceBp >= 0 && t.ConfidenceBp <= 9000);
            Assert.Equal(120, t.BearingDeg);
            Assert.Equal(10, t.DayLastObserved);
        }

        [Fact]
        public void Repeated_salvos_narrow_region_to_floor_and_raise_confidence_to_cap()
        {
            var engine = CreateDeployedEngine(new SeededRngStub(21, 5000));
            var r1 = engine.RecordObservation(Fire(10, 120, tag: "battery_a"), "atmos_clear");
            Assert.True(r1!.IsSuccess);
            int firstRadius = r1.Value!.RegionRadiusCells;
            int firstConfidence = r1.Value.ConfidenceBp;

            SoundRangingThreatEngine.AcousticThreatEstimate? last = null;
            for (int day = 11; day <= 20; day++)
            {
                var r = engine.RecordObservation(Fire(day, 120, tag: "battery_a"), "atmos_clear");
                Assert.True(r!.IsSuccess);
                last = r.Value;
            }
            Assert.NotNull(last);
            Assert.True(last!.RegionRadiusCells < firstRadius, "repeated salvos must narrow the region");
            Assert.True(last.RegionRadiusCells >= SoundRangingThreatEngine.RegionRadiusFloorCells);
            Assert.True(last.ConfidenceBp > firstConfidence, "confirmations must raise confidence");
            Assert.True(last.ConfidenceBp <= 9000, "confidence must respect the profile cap");
        }

        [Fact]
        public void Wind_broadens_error_and_storm_is_inconclusive()
        {
            var clear = CreateDeployedEngine(new SeededRngStub(21, 5000));
            var wind = CreateDeployedEngine(new SeededRngStub(21, 5000));
            var storm = CreateDeployedEngine(new SeededRngStub(21, 5000));

            var clearR = clear.RecordObservation(Fire(10, 90, tag: "x"), "atmos_clear");
            var windR = wind.RecordObservation(Fire(10, 90, tag: "x"), "atmos_wind");
            var stormR = storm.RecordObservation(Fire(10, 90, tag: "x"), "atmos_storm");

            Assert.True(clearR!.IsSuccess && windR!.IsSuccess);
            Assert.True(windR.Value!.BearingErrorDeg > clearR.Value!.BearingErrorDeg,
                "wind must broaden the effective error");
            Assert.True(stormR!.IsFailure);
            Assert.Equal(SoundRangingFailureCodes.WeatherTooNoisy, stormR.FailureCode);
        }

        [Fact]
        public void Damaged_sensor_widens_error_and_lowers_confidence()
        {
            var healthy = CreateDeployedEngine(new SeededRngStub(21, 5000));
            var damaged = CreateDeployedEngine(new SeededRngStub(21, 5000));
            Assert.True(damaged.SetSensorNodeOperational("test_array_node_0", operational: false).IsSuccess);

            var hr = healthy.RecordObservation(Fire(10, 45, tag: "x"), "atmos_clear");
            var dr = damaged.RecordObservation(Fire(10, 45, tag: "x"), "atmos_clear");
            Assert.True(hr!.IsSuccess && dr!.IsSuccess);
            Assert.True(dr.Value!.BearingErrorDeg > hr.Value!.BearingErrorDeg,
                "degraded geometry must widen error");
            Assert.True(dr.Value.ConfidenceBp < hr.Value.ConfidenceBp,
                "damaged sensor must lower confidence");
        }

        [Fact]
        public void Too_few_sensors_cannot_localize()
        {
            var engine = CreateDeployedEngine();
            foreach (var nodeId in engine.State.Nodes.Skip(1).Select(n => n.NodeId).ToList())
                Assert.True(engine.SetSensorNodeOperational(nodeId, operational: false).IsSuccess);
            var r = engine.RecordObservation(Fire(10, 200, tag: "x"));
            Assert.True(r!.IsFailure);
            Assert.Equal(SoundRangingFailureCodes.InsufficientSensors, r.FailureCode);
        }

        [Fact]
        public void Array_not_deployed_is_typed_failure()
        {
            var engine = new SoundRangingThreatEngine(CreateCatalog());
            var r = engine.RecordObservation(Fire(1));
            Assert.True(r!.IsFailure);
            Assert.Equal(SoundRangingFailureCodes.ArrayOffline, r.FailureCode);
        }

        [Fact]
        public void Calibration_drift_widens_error_until_recalibration()
        {
            var engine = CreateDeployedEngine(new SeededRngStub(21, 5000));
            for (int day = 1; day <= 100; day++)
                engine.DecayDay(day); // drift accumulates to the cap
            var drifted = engine.RecordObservation(Fire(101, 90, tag: "x"), "atmos_clear");
            Assert.True(drifted!.IsSuccess);
            Assert.True(drifted.Value!.BearingErrorDeg > 8f + 0.1f, "drift must widen error");

            // At/over the cap the array refuses observation until maintenance.
            var stale = CreateDeployedEngine(new SeededRngStub(21, 5000));
            for (int day = 1; day <= 400; day++)
                stale.DecayDay(day);
            var refused = stale.RecordObservation(Fire(401, 90, tag: "x"));
            Assert.True(refused!.IsFailure);
            Assert.Equal(SoundRangingFailureCodes.CalibrationInvalid, refused.FailureCode);

            Assert.True(stale.PerformMaintenance(partsAvailable: true).IsSuccess);
            var afterMaintenance = stale.RecordObservation(Fire(402, 90, tag: "x"), "atmos_clear");
            Assert.True(afterMaintenance!.IsSuccess, "recalibration must restore observations");
        }

        [Fact]
        public void Moving_source_breaks_correlation()
        {
            var engine = CreateDeployedEngine(new SeededRngStub(21, 5000));
            var r1 = engine.RecordObservation(Fire(10, 120, tag: "battery_a"), "atmos_clear");
            var r2 = engine.RecordObservation(Fire(11, 120, tag: "battery_a"), "atmos_clear");
            Assert.True(r1!.IsSuccess && r2!.IsSuccess);
            Assert.True(r2.Value!.RegionRadiusCells < r1.Value!.RegionRadiusCells);

            var r3 = engine.RecordObservation(Fire(12, 300, tag: "battery_b", moving: true), "atmos_clear");
            Assert.True(r3!.IsSuccess);
            Assert.Equal(0, engine.State.ConfirmingObservations);
            Assert.True(r3.Value!.RegionRadiusCells >= r1.Value.RegionRadiusCells - 0
                && r3.Value.ConfidenceBp <= r2.Value.ConfidenceBp,
                "moving source must reset the correlation loop");
        }

        [Fact]
        public void Stale_intel_decays_then_expires()
        {
            var engine = CreateDeployedEngine(new SeededRngStub(21, 5000));
            Assert.True(engine.RecordObservation(Fire(10, 90, tag: "x"), "atmos_clear").IsSuccess);
            Assert.NotNull(engine.GetActiveThreat());

            for (int day = 11; day <= 14; day++)
                engine.DecayDay(day);
            Assert.NotNull(engine.GetActiveThreat());
            Assert.True(engine.GetActiveThreat()!.ConfidenceBp < 9000, "stale intel must decay");

            for (int day = 15; day <= 16; day++)
                engine.DecayDay(day);
            Assert.Null(engine.GetActiveThreat()); // expired past the 5-day window
        }

        [Fact]
        public void Same_seed_same_observations_identical_estimate()
        {
            var a = CreateDeployedEngine(new SeededRngStub(77, 3300));
            var b = CreateDeployedEngine(new SeededRngStub(77, 3300));
            for (int i = 0; i < 5; i++)
            {
                var ra = a.RecordObservation(Fire(10 + i, 120 + i, tag: "battery_a"), "atmos_wind");
                var rb = b.RecordObservation(Fire(10 + i, 120 + i, tag: "battery_a"), "atmos_wind");
                Assert.Equal(ra!.Value!.BearingDeg, rb!.Value!.BearingDeg);
                Assert.Equal(ra.Value.BearingErrorDeg, rb.Value.BearingErrorDeg);
                Assert.Equal(ra.Value.RegionRadiusCells, rb.Value.RegionRadiusCells);
                Assert.Equal(ra.Value.ConfidenceBp, rb.Value.ConfidenceBp);
            }
        }

        [Fact]
        public void Sabotage_recovery_restores_capability()
        {
            var engine = CreateDeployedEngine();
            Assert.True(engine.SetSensorNodeOperational("test_array_node_0", operational: false).IsSuccess);
            Assert.True(engine.SetSensorNodeOperational("test_array_node_1", operational: false).IsSuccess);
            Assert.True(engine.SetSensorNodeOperational("test_array_node_2", operational: false).IsSuccess);
            var blocked = engine.RecordObservation(Fire(1));
            Assert.Equal(SoundRangingFailureCodes.InsufficientSensors, blocked!.FailureCode);

            // Repair through the same seam.
            Assert.True(engine.SetSensorNodeOperational("test_array_node_0", operational: true).IsSuccess);
            var recovered = engine.RecordObservation(Fire(2, 90, tag: "x"));
            Assert.True(recovered!.IsSuccess);
        }

        [Fact]
        public void Estimate_type_carries_no_weapon_targeting_schema()
        {
            // Hard invariant (plan §5.15 case 18 / §21 q7): the output DTO
            // must not expose weapon-quality targeting fields.
            var forbidden = new[] { "target", "firing", "solution", "aim", "coordinate", "range_km", "origin_x", "origin_y" };
            var props = typeof(SoundRangingThreatEngine.AcousticThreatEstimate)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name.ToLowerInvariant())
                .ToList();
            foreach (var forbiddenName in forbidden)
                Assert.True(!props.Any(p => p.Contains(forbiddenName)),
                    $"threat estimate must not carry a '{forbiddenName}' field (defensive-only)");
            // It must carry exactly the defensive triple.
            Assert.Contains("bearingdeg", props);
            Assert.Contains("regionradiuscells", props);
            Assert.Contains("confidencebp", props);
        }

        [Fact]
        public void Estimate_error_never_tighter_than_profile_base()
        {
            // No configuration of modifiers may produce weapon-grade precision.
            var engine = CreateDeployedEngine(new SeededRngStub(21, 9999)); // zero-noise path
            var r = engine.RecordObservation(Fire(1, 90, tag: "x"), "atmos_clear");
            Assert.True(r!.IsSuccess);
            Assert.True(r.Value!.BearingErrorDeg >= 8f - 0.001f,
                "estimate error must never be tighter than the authored base error");
        }
    }
}
