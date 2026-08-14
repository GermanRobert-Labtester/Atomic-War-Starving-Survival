using System;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Per-instance reliability for a Device-type inventory item (geiger, dosimeter).
    /// Battery and calibration drift; a mis-calibrated instrument lies.
    /// Ported engine-agnostic from Unity's AtomicWar._Game.Radiation.
    /// </summary>
    [Serializable]
    public class DeviceState
    {
        public float Battery = 1f;
        public float Calibration = 1f;
        public bool Broken;
        public int LastCalibratedDay;

        public static DeviceState CreateDefault(int day = 0)
        {
            return new DeviceState
            {
                Battery = 1f,
                Calibration = 1f,
                Broken = false,
                LastCalibratedDay = day
            };
        }

        public DeviceState Clone()
        {
            return new DeviceState
            {
                Battery = Battery,
                Calibration = Calibration,
                Broken = Broken,
                LastCalibratedDay = LastCalibratedDay
            };
        }

        public void Normalize()
        {
            Battery = MathfCompat.Clamp01(Battery);
            Calibration = MathfCompat.Clamp01(Calibration);
        }
    }

    /// <summary>
    /// Pure helpers for degrading radiation instruments. Thematic core: radiation is
    /// invisible — knowledge only comes from devices that can fail, drift, or lie.
    /// </summary>
    public static class InstrumentDevice
    {
        public const float BatteryDrainPerHour = 0.08f;
        public const float BatteryDrainPerSurvey = 0.15f;
        public const float CalibrationDriftPerDay = 0.03f;
        public const float ReliableCalibrationThreshold = 0.85f;
        public const float MaxCalibrationBias = 0.4f;

        public static bool CanMeasure(DeviceState device)
        {
            if (device == null) return false;
            if (device.Broken) return false;
            if (device.Battery <= 0f) return false;
            return true;
        }

        public static bool IsReliable(DeviceState device)
        {
            return CanMeasure(device) && device.Calibration >= ReliableCalibrationThreshold;
        }

        public static bool TryRead(DeviceState device, float trueRad, out float reading)
        {
            reading = 0f;
            if (!CanMeasure(device)) return false;
            float cal = MathfCompat.Clamp01(device.Calibration);
            float scale = MathfCompat.Lerp(1f - MaxCalibrationBias, 1f, cal);
            reading = MathfCompat.Max(0f, trueRad) * scale;
            return true;
        }

        public static float ReadBiased(DeviceState device, float trueRad)
        {
            return TryRead(device, trueRad, out float reading) ? reading : 0f;
        }

        public static void DrainBattery(DeviceState device, float amount)
        {
            if (device == null || device.Broken) return;
            device.Battery = MathfCompat.Clamp01(device.Battery - MathfCompat.Max(0f, amount));
        }

        public static void DriftCalibration(DeviceState device, float days = 1f)
        {
            if (device == null || device.Broken || days <= 0f) return;
            device.Calibration = MathfCompat.Clamp01(device.Calibration - CalibrationDriftPerDay * days);
        }

        public static void Recharge(DeviceState device)
        {
            if (device == null) return;
            device.Battery = 1f;
        }

        public static void Recalibrate(DeviceState device, int currentDay)
        {
            if (device == null) return;
            device.Calibration = 1f;
            device.LastCalibratedDay = currentDay;
        }

        public static void Break(DeviceState device)
        {
            if (device == null) return;
            device.Broken = true;
        }

        public static void RepairHardFailure(DeviceState device, int currentDay = 0)
        {
            if (device == null) return;
            device.Broken = false;
            device.Calibration = 1f;
            device.LastCalibratedDay = currentDay;
            if (device.Battery <= 0f) device.Battery = 0.25f;
            device.Normalize();
        }

        public static int DaysSinceCalibration(DeviceState device, int currentDay)
        {
            if (device == null) return int.MaxValue;
            return MathfCompat.Max(0, currentDay - device.LastCalibratedDay);
        }
    }
}
