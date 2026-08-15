using System;

namespace Ashfall.Core.Radiation
{
    /// <summary>
    /// Per-survivor dosimeter read-model: records the current dose-rate and
    /// lifetime dose snapshot. Already included in RadiationSystem; this file
    /// carries the standalone serialized shape so a host can persist dosimeters
    /// independently of the radiation engine state.
    /// </summary>
    [Serializable]
    public class DosimeterSave
    {
        public string survivorId = string.Empty;
        public float currentReading;
        public float lifetimeDose;
    }
}
