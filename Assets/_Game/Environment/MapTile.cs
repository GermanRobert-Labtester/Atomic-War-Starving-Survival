using System;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// One scavenger location's radiation truth + player knowledge.
    /// The sim always knows TrueRad; the player only sees measured/rumored values
    /// through instruments that can fail.
    /// </summary>
    [Serializable]
    public class MapTile
    {
        public string LocationId;

        /// <summary>Authoritative ambient rad (rads/hour). Never shown directly to the player.</summary>
        public float TrueRad;

        /// <summary>Last instrument reading written by a survey (may be biased).</summary>
        public float MeasuredRad;

        /// <summary>Campaign day of last successful measurement; -1 if never measured.</summary>
        public int MeasuredAtDay = -1;

        /// <summary>Calibration of the device used for the last measurement (0..1).</summary>
        public float MeasuredWithCalibration;

        /// <summary>What survivors think the rad is (rumors/events), independent of truth.</summary>
        public float RumoredRad;

        /// <summary>Uncertainty mass on the rumor/stale blend; grows each day without a good measure.</summary>
        public float RumorUncertainty = 1f;

        /// <summary>True once any successful survey has ever been recorded.</summary>
        public bool Surveyed;

        public MapTile Clone()
        {
            return new MapTile
            {
                LocationId = LocationId,
                TrueRad = TrueRad,
                MeasuredRad = MeasuredRad,
                MeasuredAtDay = MeasuredAtDay,
                MeasuredWithCalibration = MeasuredWithCalibration,
                RumoredRad = RumoredRad,
                RumorUncertainty = RumorUncertainty,
                Surveyed = Surveyed
            };
        }
    }

    /// <summary>
    /// What the UI should render for a tile: blended rad estimate, confidence, flags.
    /// </summary>
    [Serializable]
    public struct MapTilePlayerView
    {
        public string LocationId;
        /// <summary>Displayed rad estimate (measured, blended, or rumored). NaN if fully unknown.</summary>
        public float DisplayedRad;
        /// <summary>0..1 confidence ring fill (1 = certain).</summary>
        public float Confidence;
        /// <summary>True when reading is stale or was taken with a mis-calibrated device.</summary>
        public bool IsUnreliable;
        /// <summary>True when no measurement exists and no usable rumor — show "?".</summary>
        public bool IsUnknown;
        /// <summary>True when the map is dark (no working geiger) — flying blind.</summary>
        public bool IsDark;
        /// <summary>True if a survey has ever been recorded for this tile.</summary>
        public bool Surveyed;
    }
}
