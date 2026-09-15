// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Combat
{
    /// <summary>Typed failure codes (plan §12). Host supplies localized prose.</summary>
    public static class SoundRangingFailureCodes
    {
        public const string ArrayOffline = "array_offline";
        public const string InsufficientSensors = "insufficient_sensors";
        public const string CalibrationInvalid = "calibration_invalid";
        public const string ObservationInconclusive = "observation_inconclusive";
        public const string WeatherTooNoisy = "weather_too_noisy";
        public const string ThreatExpired = "threat_expired";
        public const string SensorNodeDamaged = "sensor_node_damaged";
    }

    /// <summary>
    /// DEFENSIVE long-range acoustic sound-ranging station (Plan 123 Core).
    ///
    /// Converts hostile long-range fire into defensive information only:
    /// bearing sector, probable origin region (cells + radius), confidence,
    /// and a coarse source-class estimate. This engine intentionally has NO
    /// weapon-quality targeting output — no exact origin coordinate, no
    /// firing solution, no counter-battery cueing. Consumers are warnings,
    /// map threat markers, and expedition route planning.
    ///
    /// Deterministic: measurement noise and environmental error consume
    /// ISeededRng only. Weather/sensor state enter as bounded modifiers from
    /// the catalog; the map stays the intel authority (estimates are typed
    /// values for the map owner to project).
    /// </summary>
    public sealed class SoundRangingThreatEngine
    {
        public const string SystemId = "sound_ranging";

        // ── Bounded design constants ─────────────────────────────────────
        /// <summary>Minimum operational sensors to localize at all.</summary>
        public const int MinimumOperationalSensors = 2;
        /// <summary>Hard floor on the probable-origin radius (cells).</summary>
        public const int RegionRadiusFloorCells = 1;
        /// <summary>Confidence per confirming observation (bp), capped by profile.</summary>
        public const int ConfidencePerConfirmationBp = 1500;
        public const int BaseConfidenceBp = 3000;
        /// <summary>Calibration drift cap (bp of error multiplier) before recalibration is mandatory.</summary>
        public const int CalibrationDriftCapBp = 5000;
        /// <summary>Confidence decay per stale day (bp).</summary>
        public const int ThreatDecayPerDayBp = 1000;
        /// <summary>Days after the last observation before the estimate expires.</summary>
        public const int ThreatExpiryDays = 5;
        /// <summary>Atmospheric error multiplier above which observation is inconclusive (bp of 1.0x).</summary>
        public const int WeatherTooNoisyThresholdBp = 20000;
        /// <summary>Bearing sector width for coarse output (degrees).</summary>
        public const int BearingSectorWidthDeg = 15;

        /// <summary>Authoritative station state. Owned by this engine only.</summary>
        public sealed class SoundRangingStationState
        {
            public string ArrayProfileId { get; set; } = string.Empty;
            /// <summary>Accumulated calibration drift (bp of error multiplier, 0 = calibrated).</summary>
            public int CalibrationDriftBp { get; set; }
            public List<AcousticSensorNode> Nodes { get; } = new List<AcousticSensorNode>();
            public List<HostileFireObservation> Observations { get; } = new List<HostileFireObservation>();
            public AcousticThreatEstimate? ActiveThreat { get; set; }
            /// <summary>Identifies the correlated stationary source; null = no correlation.</summary>
            public string? CorrelatedSourceTag { get; set; }
            public int ConfirmingObservations { get; set; }
        }

        /// <summary>One array node. Condition/sabotage lives with equipment owners.</summary>
        public sealed class AcousticSensorNode
        {
            public string NodeId { get; set; } = string.Empty;
            public bool Operational { get; set; } = true;
        }

        /// <summary>
        /// INPUT: one hostile long-range fire event. Deliberately coarse —
        /// bearing in degrees, no origin coordinate, no range.
        /// </summary>
        public sealed class HostileFireObservation
        {
            public int Day { get; set; }
            /// <summary>Approximate arrival bearing 0..359 (coarse sensor reading).</summary>
            public int BearingDeg { get; set; }
            /// <summary>Optional source-class signature id from the catalog.</summary>
            public string? SourceClassId { get; set; }
            /// <summary>True when the emitter is assessed to have moved since the last event.</summary>
            public bool MovingSource { get; set; }
            /// <summary>Stable tag identifying a stationary emitter across salvos.</summary>
            public string? SourceTag { get; set; }
        }

        /// <summary>
        /// OUTPUT: defensive threat estimate. Bearing sector + probable
        /// region radius + confidence — NEVER an exact origin coordinate.
        /// </summary>
        public sealed class AcousticThreatEstimate
        {
            public int BearingDeg { get; set; }
            /// <summary>Coarse bearing error (degrees) — bounded >= 2 by design.</summary>
            public float BearingErrorDeg { get; set; }
            /// <summary>Probable-origin region radius in map cells.</summary>
            public int RegionRadiusCells { get; set; }
            public int ConfidenceBp { get; set; }
            public string? SourceClassEstimateId { get; set; }
            public int DayLastObserved { get; set; }
            public bool Expired { get; set; }
        }

        private SoundRangingStationState _state = new SoundRangingStationState();
        private readonly SoundRangingCatalog _catalog;
        private readonly ILog _log;

        public ISeededRng? Rng { get; set; }

        public SoundRangingStationState State => _state;
        public SoundRangingCatalog Catalog => _catalog;

        public SoundRangingThreatEngine(SoundRangingCatalog catalog, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        public SoundRangingArrayProfile? Profile =>
            _catalog.GetArray(string.IsNullOrEmpty(_state.ArrayProfileId) ? "" : _state.ArrayProfileId);

        /// <summary>Deploys the array with the authored sensor count; all nodes operational.</summary>
        public ActionResult Install(string arrayProfileId, bool partsAvailable)
        {
            var profile = _catalog.GetArray(arrayProfileId ?? string.Empty);
            if (profile == null)
                return ActionResult.Blocked(SoundRangingFailureCodes.ArrayOffline, "sra.array_profile_unknown");
            if (!partsAvailable)
                return ActionResult.Blocked("sra_install_parts_missing", "sra.install_parts_missing");

            _state = new SoundRangingStationState { ArrayProfileId = profile.id };
            for (int i = 0; i < profile.sensor_count; i++)
                _state.Nodes.Add(new AcousticSensorNode { NodeId = $"{profile.id}_node_{i}" });
            return ActionResult.Success("sra_deployed", eventId: "sra.install");
        }

        /// <summary>Sabotage/repair path: flips one node's operational state.</summary>
        public ActionResult SetSensorNodeOperational(string nodeId, bool operational)
        {
            var node = _state.Nodes.Find(n => string.Equals(n.NodeId, nodeId, StringComparison.Ordinal));
            if (node == null)
                return ActionResult.Blocked(SoundRangingFailureCodes.SensorNodeDamaged, "sra.node_unknown");
            node.Operational = operational;
            return ActionResult.Success(operational ? "sra_node_repaired" : "sra_node_down",
                eventId: "sra.set_node");
        }

        public int OperationalSensorCount()
        {
            int count = 0;
            foreach (var n in _state.Nodes)
                if (n.Operational) count++;
            return count;
        }

        /// <summary>Daily maintenance tick: calibration drifts; stale intel decays/expires.</summary>
        public ActionResult DecayDay(int currentDay)
        {
            var profile = Profile;
            if (profile == null)
                return ActionResult.Blocked(SoundRangingFailureCodes.ArrayOffline, "sra.array_not_deployed");

            _state.CalibrationDriftBp = Math.Min(CalibrationDriftCapBp,
                _state.CalibrationDriftBp + profile.maintenance_drift_per_day_bp);

            var threat = _state.ActiveThreat;
            if (threat != null)
            {
                int staleDays = currentDay - threat.DayLastObserved;
                if (staleDays >= ThreatExpiryDays)
                {
                    threat.ConfidenceBp = 0;
                    threat.Expired = true;
                    _state.ActiveThreat = null;
                    _state.CorrelatedSourceTag = null;
                    _state.ConfirmingObservations = 0;
                }
                else if (staleDays > 0)
                {
                    threat.ConfidenceBp = Math.Max(0, threat.ConfidenceBp - ThreatDecayPerDayBp * staleDays);
                }
            }
            return ActionResult.Success("sra_day_decay", eventId: "sra.decay_day");
        }

        /// <summary>Maintenance recalibrates the array (requires parts).</summary>
        public ActionResult PerformMaintenance(bool partsAvailable)
        {
            if (Profile == null)
                return ActionResult.Blocked(SoundRangingFailureCodes.ArrayOffline, "sra.array_not_deployed");
            if (_state.CalibrationDriftBp == 0 && OperationalSensorCount() == _state.Nodes.Count)
                return ActionResult.Blocked("sra_maintenance_not_needed", "sra.maintenance_not_needed");
            if (!partsAvailable)
                return ActionResult.Blocked("sra_maintenance_parts_missing", "sra.maintenance_parts_missing");

            _state.CalibrationDriftBp = 0;
            // Repairs do not flip node states — that is the equipment owner's
            // seam — but calibration is this engine's own truth.
            return ActionResult.Success("sra_recalibrated", eventId: "sra.maintenance");
        }

        /// <summary>
        /// Processes one hostile-fire event into a defensive estimate.
        /// Observation-inconclusive and weather/no-sensor cases are typed
        /// failures; nothing here produces weapon-quality targeting data.
        /// </summary>
        public ActionResult<AcousticThreatEstimate> RecordObservation(
            HostileFireObservation observation, string? atmosphericErrorProfileId = null)
        {
            if (observation == null)
                return Blocked(SoundRangingFailureCodes.ObservationInconclusive);
            var profile = Profile;
            if (profile == null)
                return Blocked(SoundRangingFailureCodes.ArrayOffline);

            int operational = OperationalSensorCount();
            if (operational < MinimumOperationalSensors)
                return Blocked(SoundRangingFailureCodes.InsufficientSensors);

            if (_state.CalibrationDriftBp >= CalibrationDriftCapBp)
                return Blocked(SoundRangingFailureCodes.CalibrationInvalid);

            // ── Bounded error model (plan §5.5) ─────────────────────────
            float atmosMultiplier = 1f;
            if (!string.IsNullOrEmpty(atmosphericErrorProfileId))
            {
                var atmos = _catalog.GetAtmospheric(atmosphericErrorProfileId);
                if (atmos != null)
                {
                    atmosMultiplier = atmos.error_multiplier_bp / 10000f;
                    if (atmos.error_multiplier_bp > WeatherTooNoisyThresholdBp)
                        return Blocked(SoundRangingFailureCodes.WeatherTooNoisy);
                }
            }
            float calMult = 1f + _state.CalibrationDriftBp / 10000f;
            float sensorConditionModifier = (float)operational / Math.Max(1, profile.sensor_count);

            float effectiveError = profile.base_bearing_error_deg
                * atmosMultiplier
                * calMult
                * (2f - sensorConditionModifier); // degraded geometry widens error, bounded

            // ── Seeded measurement noise ────────────────────────────────
            float noiseDeg = Rng != null
                ? (float)((Rng.NextDouble() * 2.0 - 1.0) * effectiveError)
                : 0f;
            int estimatedBearing = ((observation.BearingDeg + (int)Math.Round(noiseDeg)) % 360 + 360) % 360;

            // ── Salvo correlation (plan §5.6): stationary sources narrow ─
            bool correlated = !string.IsNullOrEmpty(observation.SourceTag)
                && string.Equals(_state.CorrelatedSourceTag, observation.SourceTag, StringComparison.Ordinal);
            if (observation.MovingSource || (!correlated && _state.ConfirmingObservations > 0 && observation.SourceTag != null))
            {
                // Moving or redeployed source breaks correlation entirely.
                _state.CorrelatedSourceTag = observation.SourceTag;
                _state.ConfirmingObservations = 0;
                correlated = false;
            }
            else if (!correlated)
            {
                _state.CorrelatedSourceTag = observation.SourceTag;
                _state.ConfirmingObservations = 0;
            }

            int radius = profile.base_region_radius_cells;
            if (correlated && _state.ActiveThreat != null)
            {
                _state.ConfirmingObservations++;
                radius = Math.Max(RegionRadiusFloorCells, _state.ActiveThreat.RegionRadiusCells - 1);
            }
            else
            {
                radius = Math.Max(RegionRadiusFloorCells,
                    profile.base_region_radius_cells
                    + (int)Math.Ceiling(effectiveError / 10f)); // wider error → wider region
            }

            // ── Confidence (hard cap from profile) ──────────────────────
            int confidence = BaseConfidenceBp + ConfidencePerConfirmationBp * _state.ConfirmingObservations;
            // Weather and sensor condition bound achievable confidence.
            confidence = (int)(confidence / Math.Max(1f, atmosMultiplier * (2f - sensorConditionModifier)));
            confidence = Math.Min(confidence, profile.confidence_cap_bp);

            var estimate = new AcousticThreatEstimate
            {
                BearingDeg = estimatedBearing,
                // Coarseness is a design invariant: never tighter than the
                // base profile error (never weapon-grade).
                BearingErrorDeg = Math.Max(2f, effectiveError),
                RegionRadiusCells = radius,
                ConfidenceBp = Math.Clamp(confidence, 0, profile.confidence_cap_bp),
                SourceClassEstimateId = observation.SourceClassId,
                DayLastObserved = observation.Day,
                Expired = false
            };
            _state.ActiveThreat = estimate;
            _state.Observations.Add(observation);
            return ActionResult<AcousticThreatEstimate>.Success(estimate);
        }

        /// <summary>Read-only view for map/UI consumers; null when no live threat.</summary>
        public AcousticThreatEstimate? GetActiveThreat() =>
            _state.ActiveThreat != null && !_state.ActiveThreat.Expired ? _state.ActiveThreat : null;

        private static ActionResult<AcousticThreatEstimate> Blocked(string code)
            => ActionResult<AcousticThreatEstimate>.Blocked(code, "sra." + code);
    
        // ── Save round-trip (Plan 123 Phase 8): deep-clone via typed JSON. ──
        public SoundRangingStationState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            return s.Deserialize<SoundRangingStationState>(s.Serialize(_state)) ?? new SoundRangingStationState();
        }

        public void RestoreState(SoundRangingStationState? saved)
        {
            if (saved == null) return; // old saves: no section → undeployed array
            var s = new SystemTextJsonSerializer();
            _state = s.Deserialize<SoundRangingStationState>(s.Serialize(saved)) ?? new SoundRangingStationState();
            _state.CalibrationDriftBp = Math.Clamp(_state.CalibrationDriftBp, 0, CalibrationDriftCapBp);
            if (_state.ActiveThreat != null)
                _state.ActiveThreat.ConfidenceBp = Math.Clamp(_state.ActiveThreat.ConfidenceBp, 0, 10000);
        }
}
}
