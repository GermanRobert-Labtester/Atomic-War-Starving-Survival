using System;
using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Per-instance reliability for a Device-type inventory item (geiger, dosimeter).
    /// Battery and calibration drift; a mis-calibrated instrument lies. Public fields
    /// for JsonUtility save/load.
    /// </summary>
    [Serializable]
    public class DeviceState
    {
        /// <summary>Remaining charge 0..1. Zero prevents measurement.</summary>
        public float Battery = 1f;

        /// <summary>Calibration quality 0..1. 1 = perfect; lower readings under-report true rad.</summary>
        public float Calibration = 1f;

        /// <summary>Hard failure — no measurement until repaired/replaced.</summary>
        public bool Broken;

        /// <summary>Campaign day of last successful recalibration.</summary>
        public int LastCalibratedDay;

        /// <summary>Create a fresh, working instrument.</summary>
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

        /// <summary>Deep copy for save/load safety.</summary>
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

        /// <summary>Clamp fields into valid ranges.</summary>
        public void Normalize()
        {
            Battery = Mathf.Clamp01(Battery);
            Calibration = Mathf.Clamp01(Calibration);
        }
    }
}
