// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Radio
{
    [Serializable]
    public sealed class ElectricalFaultInput
    {
        public string fault_id = string.Empty;
        public string asset_id = string.Empty;
        public string hazard_type = "electrical_fault";
        public float fault_intensity;
        public float distance;
        public bool energized;
    }

    [Serializable]
    public sealed class UvCoronaEnvironmentState
    {
        public string environment_id = "clear";
        public float visibility = 1f;
        public float humidity = 0.2f;
        public float ash_load;
    }

    [Serializable]
    public sealed class CoronaObservation
    {
        public string observation_id = string.Empty;
        public string fault_id = string.Empty;
        public string asset_id = string.Empty;
        public string hazard_type = string.Empty;
        public float confidence;
        public float signal;
        public bool energized;
        public bool false_positive;
        public int day;
    }

    [Serializable]
    public sealed class UvCoronaDetectionState
    {
        public int schema_version = 1;
        public ulong rng_state;
        public string detector_id = string.Empty;
        public float calibration = 1f;
        public float sensor_condition = 1f;
        public int scan_count;
        public List<CoronaObservation> observations = new List<CoronaObservation>();
    }

    public sealed class UvScanResult
    {
        public bool Success { get; set; }
        public string FailureCode { get; set; } = string.Empty;
        public IReadOnlyList<CoronaObservation> Observations { get; set; } = Array.Empty<CoronaObservation>();
    }

    public static class UvCoronaFailureCodes
    {
        public const string DetectorUnknown = "uv.detector_unknown";
        public const string DetectorUnavailable = "uv.detector_unavailable";
        public const string BatteryDepleted = "uv.battery_depleted";
        public const string EnvironmentUnknown = "uv.environment_unknown";
        public const string InvalidInput = "uv.invalid_input";
    }

    /// <summary>Bounded UV corona observation model. It observes supplied power faults and never mutates them.</summary>
    public sealed class UvCoronaDetectionEngine
    {
        private readonly ISeededRng _rng;
        private UvCoronaDetectionCatalog _catalog;
        private UvCoronaDetectionState _state = new UvCoronaDetectionState();
        private IPlayerInventoryPort? _inventory;
        private int _nextObservation;

        public UvCoronaDetectionEngine(ISeededRng? rng = null, UvCoronaDetectionCatalog? catalog = null)
        {
            _rng = rng ?? new SeededRng(119);
            _catalog = catalog ?? new UvCoronaDetectionCatalog(new UvCoronaCatalogDto
            {
                detectors = new List<UvCoronaDetectorProfile>
                {
                    new UvCoronaDetectorProfile { detector_id = "uv_corona_camera_mk1", battery_item_id = "battery", base_detection_range = 6f }
                },
                environments = new List<UvCoronaEnvironmentProfile>
                {
                    new UvCoronaEnvironmentProfile { environment_id = "clear" }
                }
            });
        }

        public UvCoronaDetectionCatalog Catalog => _catalog;
        public UvCoronaDetectionState State => _state;

        public void BindCatalog(UvCoronaDetectionCatalog catalog) => _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        public void BindInventory(IPlayerInventoryPort inventory) => _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));

        public ActionResult Equip(string detectorId)
        {
            var detector = _catalog.FindDetector(detectorId);
            if (detector == null) return ActionResult.Blocked(UvCoronaFailureCodes.DetectorUnknown, "uv.detector_unknown");
            _state.detector_id = detectorId;
            _state.calibration = Math.Clamp(_state.calibration <= 0f ? 1f : _state.calibration, 0f, 1f);
            _state.sensor_condition = Math.Clamp(_state.sensor_condition <= 0f ? 1f : _state.sensor_condition, 0f, 1f);
            return ActionResult.Success("uv.detector_equipped");
        }

        public UvScanResult Scan(
            IReadOnlyList<ElectricalFaultInput> faults,
            string environmentId,
            float operatorSkill = 0.5f,
            int day = 0)
        {
            var detector = _catalog.FindDetector(_state.detector_id);
            if (detector == null) return new UvScanResult { FailureCode = UvCoronaFailureCodes.DetectorUnavailable };
            var environment = _catalog.FindEnvironment(environmentId);
            if (environment == null) return new UvScanResult { FailureCode = UvCoronaFailureCodes.EnvironmentUnknown };
            if (faults == null) return new UvScanResult { FailureCode = UvCoronaFailureCodes.InvalidInput };
            if (_inventory == null || !_inventory.TryConsume(detector.battery_item_id, detector.battery_use_per_scan))
                return new UvScanResult { FailureCode = UvCoronaFailureCodes.BatteryDepleted };

            float visibility = Math.Clamp(environment.visibility * (1f - environment.ash_load * 0.35f), 0f, 1f);
            float condition = Math.Clamp(_state.sensor_condition, 0.1f, 1f);
            float skill = 0.9f + Math.Clamp(operatorSkill, 0f, 1f) * 0.1f;
            var observations = new List<CoronaObservation>();
            foreach (var fault in faults)
            {
                if (fault == null || fault.distance < 0f || fault.fault_intensity < 0f || fault.fault_intensity > 1f)
                    continue;
                float range = Math.Clamp(1f - fault.distance / detector.base_detection_range, 0f, 1f);
                float signal = Math.Clamp(fault.fault_intensity * range * visibility * condition
                    * _state.calibration * environment.fault_activity_modifier * skill, 0f, 1f);
                float noise = detector.sensor_noise + (1f - condition) * detector.condition_noise_factor;
                float measured = Math.Clamp(signal - noise + (_rng.NextFloat() - 0.5f) * noise, 0f, 1f);
                float confidence = Math.Clamp(measured / Math.Max(0.01f, detector.minimum_fault_intensity), 0f, 1f);
                if (confidence <= 0f) continue;
                observations.Add(new CoronaObservation
                {
                    observation_id = $"uv_obs_{++_nextObservation}",
                    fault_id = fault.fault_id ?? string.Empty,
                    asset_id = fault.asset_id ?? string.Empty,
                    hazard_type = fault.hazard_type ?? "electrical_fault",
                    confidence = confidence,
                    signal = signal,
                    energized = fault.energized,
                    day = day
                });
            }
            _state.scan_count++;
            _state.observations.AddRange(observations);
            return new UvScanResult { Success = true, Observations = observations };
        }

        public void AdvanceDay()
        {
            var detector = _catalog.FindDetector(_state.detector_id);
            if (detector == null) return;
            _state.calibration = Math.Clamp(_state.calibration - detector.calibration_drift_per_day, 0f, 1f);
        }

        public UvCoronaDetectionState CaptureState()
        {
            var copy = new UvCoronaDetectionState
            {
                schema_version = _state.schema_version,
                rng_state = _rng is SeededRng seeded ? seeded.PeekState() : 0UL,
                detector_id = _state.detector_id,
                calibration = _state.calibration,
                sensor_condition = _state.sensor_condition,
                scan_count = _state.scan_count,
                observations = new List<CoronaObservation>()
            };
            foreach (var observation in _state.observations)
                copy.observations.Add(new CoronaObservation
                {
                    observation_id = observation.observation_id,
                    fault_id = observation.fault_id,
                    asset_id = observation.asset_id,
                    hazard_type = observation.hazard_type,
                    confidence = observation.confidence,
                    signal = observation.signal,
                    energized = observation.energized,
                    false_positive = observation.false_positive,
                    day = observation.day
                });
            return copy;
        }

        public void RestoreState(UvCoronaDetectionState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state = state;
            _state.observations ??= new List<CoronaObservation>();
            _state.calibration = Math.Clamp(_state.calibration, 0f, 1f);
            _state.sensor_condition = Math.Clamp(_state.sensor_condition, 0f, 1f);
            if (_rng is SeededRng seeded && _state.rng_state != 0UL)
                seeded.SeekState(_state.rng_state);
            _nextObservation = _state.observations.Count;
        }
    }
}
