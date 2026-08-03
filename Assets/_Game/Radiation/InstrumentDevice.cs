using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Pure helpers for degrading radiation instruments. Thematic core: radiation is
    /// invisible — knowledge only comes from devices that can fail, drift, or lie.
    /// </summary>
    public static class InstrumentDevice
    {
        /// <summary>Battery consumed per hour of active surveying / listening.</summary>
        public const float BatteryDrainPerHour = 0.08f;

        /// <summary>Battery consumed for a single discrete survey action.</summary>
        public const float BatteryDrainPerSurvey = 0.15f;

        /// <summary>Calibration lost per campaign day while the device is owned.</summary>
        public const float CalibrationDriftPerDay = 0.03f;

        /// <summary>
        /// Calibration at/above this counts as reliable for map freshness rules.
        /// Below this, readings are flagged "unreliable" and under-report true rad.
        /// </summary>
        public const float ReliableCalibrationThreshold = 0.85f;

        /// <summary>
        /// Maximum under-report fraction when calibration is 0.
        /// measured = trueRad * Lerp(1 - MaxBias, 1, calibration).
        /// </summary>
        public const float MaxCalibrationBias = 0.4f;

        /// <summary>Whether the device can produce a measurement at all.</summary>
        public static bool CanMeasure(DeviceState device)
        {
            if (device == null) return false;
            if (device.Broken) return false;
            if (device.Battery <= 0f) return false;
            return true;
        }

        /// <summary>Whether calibration is good enough for a "reliable" map reading.</summary>
        public static bool IsReliable(DeviceState device)
        {
            return CanMeasure(device) && device.Calibration >= ReliableCalibrationThreshold;
        }

        /// <summary>
        /// Biased reading of true ambient rad. Broken / dead battery returns false and
        /// leaves reading at 0. A mis-calibrated device under-reports (the deadly lie).
        /// </summary>
        public static bool TryRead(DeviceState device, float trueRad, out float reading)
        {
            reading = 0f;
            if (!CanMeasure(device)) return false;

            float cal = Mathf.Clamp01(device.Calibration);
            float scale = Mathf.Lerp(1f - MaxCalibrationBias, 1f, cal);
            reading = Mathf.Max(0f, trueRad) * scale;
            return true;
        }

        /// <summary>Convenience: biased read or 0 if the device cannot measure.</summary>
        public static float ReadBiased(DeviceState device, float trueRad)
        {
            return TryRead(device, trueRad, out float reading) ? reading : 0f;
        }

        /// <summary>
        /// Drain battery by a fixed amount. Empty battery does NOT set Broken —
        /// power and hard failure are separate (recharge restores a drained unit).
        /// </summary>
        public static void DrainBattery(DeviceState device, float amount)
        {
            if (device == null || device.Broken) return;
            device.Battery = Mathf.Clamp01(device.Battery - Mathf.Max(0f, amount));
        }

        /// <summary>Apply daily calibration drift (owned instruments slowly go out of tune).</summary>
        public static void DriftCalibration(DeviceState device, float days = 1f)
        {
            if (device == null || device.Broken || days <= 0f) return;
            device.Calibration = Mathf.Clamp01(device.Calibration - CalibrationDriftPerDay * days);
        }

        /// <summary>Full recharge (consumes a battery item externally).</summary>
        public static void Recharge(DeviceState device)
        {
            if (device == null) return;
            device.Battery = 1f;
            // Recharge alone does not un-break a shattered tube — need a new unit or repair.
            // Spec: batteries restore power; broken stays broken unless we explicitly repair.
        }

        /// <summary>
        /// Recalibrate to perfect (consumes a calibration kit externally).
        /// Does not clear Broken — hard failures require a replacement unit.
        /// </summary>
        public static void Recalibrate(DeviceState device, int currentDay)
        {
            if (device == null) return;
            device.Calibration = 1f;
            device.LastCalibratedDay = currentDay;
        }

        /// <summary>Mark the instrument as hard-broken (EMP, impact, etc.).</summary>
        public static void Break(DeviceState device)
        {
            if (device == null) return;
            device.Broken = true;
        }

        /// <summary>Days since last calibration (clamped ≥ 0).</summary>
        public static int DaysSinceCalibration(DeviceState device, int currentDay)
        {
            if (device == null) return int.MaxValue;
            return Mathf.Max(0, currentDay - device.LastCalibratedDay);
        }
    }
}
