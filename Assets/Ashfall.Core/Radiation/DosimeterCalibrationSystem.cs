using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Radiation
{
    // ── Device state ─────────────────────────────────────────────────

    /// <summary>Per-device calibration and condition state.</summary>
    [Serializable]
    public class DosimeterDeviceState
    {
        public string deviceTag = string.Empty;
        public string assignedSurvivorId = string.Empty;
        public float batteryLevel = 1.0f;       // 0..1
        public float sensorCondition = 1.0f;     // 0..1, degrades with use
        public float calibrationQuality = 1.0f;  // 0..1, 1 = perfect calibration
        public int readingsSinceCalibration = 0;
        public int lastCalibrationDay = -1;
        public int calibrationCount = 0;
        public bool isOverdue = false;
        public bool isStationOccupied = false;
        public int stationOccupiedUntilDay = -1;
        public float errorBandMsv = 0f;          // current measurement uncertainty (+/-)
    }

    /// <summary>System-wide calibration state (save DTO).</summary>
    [Serializable]
    public class DosimeterCalibrationState
    {
        public string systemId = DosimeterCalibrationSystem.SystemId;
        public List<DosimeterDeviceState> devices = new List<DosimeterDeviceState>();
    }

    // ── System ──────────────────────────────────────────────────────

    /// <summary>
    /// ASHFALL: THE DOSE — Per-device dosimeter calibration and measurement
    /// confidence system. Models device condition, battery, sensor wear,
    /// calibration quality, and measurement uncertainty.
    ///
    /// Key invariant: calibration affects OBSERVED readings and confidence
    /// intervals. It does NOT alter true radiation dose (owned by RadiationSystem)
    /// or cumulative booked dose (owned by DoseLedgerSystem).
    /// </summary>
    public class DosimeterCalibrationSystem
    {
        public const string SystemId = "dosimeter_calibration_system";

        // Calibration thresholds
        public const int ReadingsPerCalibration = 40;
        public const float OverdueErrorBandMultiplier = 2.5f;
        public const float CalibratedErrorBandMultiplier = 0.3f;
        public const float BatteryDrainPerReading = 0.005f;
        public const float SensorWearPerReading = 0.003f;
        public const float MinBatteryForReading = 0.05f;
        public const float MinSensorCondition = 0.1f;
        public const int CalibrationDurationDays = 1;
        public const float TestSourceExposureMsv = 5f;

        // Error band constants (mSv)
        public const float BaseErrorBandMsv = 15f;
        public const float PerfectCalibrationErrorBandMsv = 3f;

        private readonly DosimeterCalibrationState _state = new DosimeterCalibrationState();
        private readonly Dictionary<string, DosimeterDeviceState> _devices = new Dictionary<string, DosimeterDeviceState>();

        // Events
        public event Action<string> OnCalibrationStarted;        // deviceTag
        public event Action<string> OnCalibrationCompleted;      // deviceTag
        public event Action<string> OnCalibrationFailed;         // deviceTag
        public event Action<string> OnDeviceConditionChanged;    // deviceTag
        public event Action<string> OnReadingConfidenceChanged;  // deviceTag
        public event Action<string> OnCalibrationOverdue;        // deviceTag
        public event Action<DosimeterCalibrationState> OnStateChanged;

        public DosimeterCalibrationState State => _state;
        public IReadOnlyDictionary<string, DosimeterDeviceState> Devices => _devices;

        // ── Device registration ──────────────────────────────────────

        /// <summary>Register a dosimeter device. Called when a dosimeter is assigned.</summary>
        public bool RegisterDevice(string deviceTag, string survivorId)
        {
            if (string.IsNullOrEmpty(deviceTag) || string.IsNullOrEmpty(survivorId))
                return false;
            if (_devices.ContainsKey(deviceTag))
                return false;

            var device = new DosimeterDeviceState
            {
                deviceTag = deviceTag,
                assignedSurvivorId = survivorId,
                batteryLevel = 1.0f,
                sensorCondition = 1.0f,
                calibrationQuality = 1.0f,
                readingsSinceCalibration = 0,
                lastCalibrationDay = -1,
                calibrationCount = 0,
                isOverdue = false,
                errorBandMsv = PerfectCalibrationErrorBandMsv
            };
            _devices[deviceTag] = device;
            _state.devices.Add(device);
            RaiseChanged();
            return true;
        }

        /// <summary>Unregister a device (dosimeter lost/destroyed).</summary>
        public bool UnregisterDevice(string deviceTag)
        {
            if (!_devices.Remove(deviceTag)) return false;
            _state.devices.RemoveAll(d => d.deviceTag == deviceTag);
            RaiseChanged();
            return true;
        }

        // ── Reading consumption ──────────────────────────────────────

        /// <summary>
        /// Called when a dose reading is booked. Consumes battery, wears sensor,
        /// increments reading counter, and recalculates error band.
        /// Does NOT alter the actual dose — that's DoseLedgerSystem's job.
        /// </summary>
        public void ConsumeReading(string deviceTag)
        {
            if (!_devices.TryGetValue(deviceTag, out var device)) return;

            // Battery drain
            device.batteryLevel = Math.Max(0f, device.batteryLevel - BatteryDrainPerReading);

            // Sensor wear
            device.sensorCondition = Math.Max(0f, device.sensorCondition - SensorWearPerReading);

            // Reading counter
            device.readingsSinceCalibration++;

            // Overdue check
            if (device.readingsSinceCalibration >= ReadingsPerCalibration && !device.isOverdue)
            {
                device.isOverdue = true;
                OnCalibrationOverdue?.Invoke(deviceTag);
            }

            // Recalculate error band
            RecalculateErrorBand(device);

            OnDeviceConditionChanged?.Invoke(deviceTag);
            OnReadingConfidenceChanged?.Invoke(deviceTag);
            RaiseChanged();
        }

        // ── Calibration ──────────────────────────────────────────────

        /// <summary>
        /// Start a calibration procedure. Consumes time and test-source exposure.
        /// Returns true if calibration started successfully.
        /// </summary>
        public bool StartCalibration(string deviceTag, int currentDay)
        {
            if (!_devices.TryGetValue(deviceTag, out var device)) return false;

            // Check prerequisites
            if (device.batteryLevel < MinBatteryForReading) return false;
            if (device.sensorCondition < MinSensorCondition) return false;
            if (device.isStationOccupied && device.stationOccupiedUntilDay > currentDay) return false;

            // Start calibration
            device.isStationOccupied = true;
            device.stationOccupiedUntilDay = currentDay + CalibrationDurationDays;
            OnCalibrationStarted?.Invoke(deviceTag);
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Complete a calibration procedure. Called when the calibration duration
        /// has elapsed. Resets reading counter, improves calibration quality.
        /// </summary>
        public bool CompleteCalibration(string deviceTag, int currentDay)
        {
            if (!_devices.TryGetValue(deviceTag, out var device)) return false;
            if (!device.isStationOccupied) return false;
            if (device.stationOccupiedUntilDay > currentDay) return false;

            // Complete calibration
            device.isStationOccupied = false;
            device.readingsSinceCalibration = 0;
            device.isOverdue = false;
            device.lastCalibrationDay = currentDay;
            device.calibrationCount++;

            // Improve calibration quality (diminishing returns)
            float improvement = 0.15f * (1f - device.calibrationQuality);
            device.calibrationQuality = Math.Min(1f, device.calibrationQuality + improvement);

            // Recalculate error band
            RecalculateErrorBand(device);

            OnCalibrationCompleted?.Invoke(deviceTag);
            OnReadingConfidenceChanged?.Invoke(deviceTag);
            RaiseChanged();
            return true;
        }

        /// <summary>Cancel an in-progress calibration (e.g., power failure).</summary>
        public bool CancelCalibration(string deviceTag)
        {
            if (!_devices.TryGetValue(deviceTag, out var device)) return false;
            if (!device.isStationOccupied) return false;

            device.isStationOccupied = false;
            device.stationOccupiedUntilDay = -1;
            OnCalibrationFailed?.Invoke(deviceTag);
            RaiseChanged();
            return true;
        }

        // ── Battery and maintenance ──────────────────────────────────

        /// <summary>Replace battery in a device.</summary>
        public bool ReplaceBattery(string deviceTag)
        {
            if (!_devices.TryGetValue(deviceTag, out var device)) return false;
            device.batteryLevel = 1.0f;
            OnDeviceConditionChanged?.Invoke(deviceTag);
            RaiseChanged();
            return true;
        }

        /// <summary>Service/replace sensor in a device.</summary>
        public bool ServiceSensor(string deviceTag)
        {
            if (!_devices.TryGetValue(deviceTag, out var device)) return false;
            device.sensorCondition = 1.0f;
            OnDeviceConditionChanged?.Invoke(deviceTag);
            RaiseChanged();
            return true;
        }

        // ── Queries ──────────────────────────────────────────────────

        /// <summary>Get the current error band for a device (±mSv).</summary>
        public float GetErrorBand(string deviceTag)
        {
            return _devices.TryGetValue(deviceTag, out var device) ? device.errorBandMsv : BaseErrorBandMsv;
        }

        /// <summary>Get the measurement confidence for a device (0..1).</summary>
        public float GetConfidence(string deviceTag)
        {
            if (!_devices.TryGetValue(deviceTag, out var device)) return 0f;
            if (device.batteryLevel < MinBatteryForReading) return 0f;
            if (device.sensorCondition < MinSensorCondition) return 0f;
            return device.calibrationQuality * device.sensorCondition;
        }

        /// <summary>Check if a device can take a reading.</summary>
        public bool CanTakeReading(string deviceTag)
        {
            if (!_devices.TryGetValue(deviceTag, out var device)) return false;
            return device.batteryLevel >= MinBatteryForReading
                && device.sensorCondition >= MinSensorCondition;
        }

        /// <summary>Check if calibration is in progress.</summary>
        public bool IsCalibrating(string deviceTag)
        {
            return _devices.TryGetValue(deviceTag, out var device) && device.isStationOccupied;
        }

        /// <summary>Check if calibration is complete and ready to finalize.</summary>
        public bool IsCalibrationComplete(string deviceTag, int currentDay)
        {
            if (!_devices.TryGetValue(deviceTag, out var device)) return false;
            return device.isStationOccupied && device.stationOccupiedUntilDay <= currentDay;
        }

        /// <summary>Get a specific device state.</summary>
        public DosimeterDeviceState? GetDevice(string deviceTag)
        {
            return _devices.TryGetValue(deviceTag, out var device) ? device : null;
        }

        // ── Error band calculation ───────────────────────────────────

        private void RecalculateErrorBand(DosimeterDeviceState device)
        {
            // Base error band depends on calibration quality
            float baseBand = BaseErrorBandMsv * (1f - device.calibrationQuality)
                           + PerfectCalibrationErrorBandMsv * device.calibrationQuality;

            // Sensor condition widens the band
            float sensorFactor = 1f + (1f - device.sensorCondition) * 0.5f;

            // Overdue status widens the band significantly
            float overdueFactor = device.isOverdue ? OverdueErrorBandMultiplier : 1.0f;

            // Battery doesn't affect accuracy, only ability to read

            device.errorBandMsv = baseBand * sensorFactor * overdueFactor;
        }

        // ── Save / Load ──────────────────────────────────────────────

        public DosimeterCalibrationState CaptureState()
        {
            var copy = new DosimeterCalibrationState
            {
                systemId = _state.systemId
            };
            // Ordinal-ordered copy
            var sorted = new List<DosimeterDeviceState>(_state.devices);
            sorted.Sort((a, b) => string.CompareOrdinal(a.deviceTag, b.deviceTag));
            foreach (var d in sorted)
            {
                copy.devices.Add(new DosimeterDeviceState
                {
                    deviceTag = d.deviceTag,
                    assignedSurvivorId = d.assignedSurvivorId,
                    batteryLevel = d.batteryLevel,
                    sensorCondition = d.sensorCondition,
                    calibrationQuality = d.calibrationQuality,
                    readingsSinceCalibration = d.readingsSinceCalibration,
                    lastCalibrationDay = d.lastCalibrationDay,
                    calibrationCount = d.calibrationCount,
                    isOverdue = d.isOverdue,
                    isStationOccupied = d.isStationOccupied,
                    stationOccupiedUntilDay = d.stationOccupiedUntilDay,
                    errorBandMsv = d.errorBandMsv
                });
            }
            return copy;
        }

        public void RestoreState(DosimeterCalibrationState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _devices.Clear();
            _state.devices.Clear();
            if (saved.devices != null)
            {
                foreach (var d in saved.devices)
                {
                    if (d == null || string.IsNullOrEmpty(d.deviceTag)) continue;
                    var copy = new DosimeterDeviceState
                    {
                        deviceTag = d.deviceTag,
                        assignedSurvivorId = d.assignedSurvivorId,
                        batteryLevel = Math.Clamp(d.batteryLevel, 0f, 1f),
                        sensorCondition = Math.Clamp(d.sensorCondition, 0f, 1f),
                        calibrationQuality = Math.Clamp(d.calibrationQuality, 0f, 1f),
                        readingsSinceCalibration = Math.Max(0, d.readingsSinceCalibration),
                        lastCalibrationDay = d.lastCalibrationDay,
                        calibrationCount = Math.Max(0, d.calibrationCount),
                        isOverdue = d.isOverdue,
                        isStationOccupied = d.isStationOccupied,
                        stationOccupiedUntilDay = d.stationOccupiedUntilDay,
                        errorBandMsv = Math.Max(0f, d.errorBandMsv)
                    };
                    _devices[copy.deviceTag] = copy;
                    _state.devices.Add(copy);
                }
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
