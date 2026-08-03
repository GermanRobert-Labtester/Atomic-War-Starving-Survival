using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Environment;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Thin view-model for radiation fog-of-war: confidence rings per location,
    /// unreliable flags, and map-dark state when no working geiger remains.
    /// No game logic — systems push MapTilePlayerView snapshots in.
    /// </summary>
    public class MapKnowledgeHUD : MonoBehaviour
    {
        private readonly List<MapTilePlayerView> _views = new List<MapTilePlayerView>();

        public IReadOnlyList<MapTilePlayerView> Views => _views;
        public bool IsMapDark { get; private set; }
        public int DaysSinceCalibration { get; private set; } = -1;
        public bool HasWorkingGeiger { get; private set; }

        /// <summary>Replace the full tile view list (call after knowledge or device changes).</summary>
        public void SetViews(IReadOnlyList<MapTilePlayerView> views, bool hasWorkingGeiger)
        {
            _views.Clear();
            HasWorkingGeiger = hasWorkingGeiger;
            IsMapDark = !hasWorkingGeiger;
            if (views == null) return;
            for (int i = 0; i < views.Count; i++)
            {
                _views.Add(views[i]);
            }
        }

        /// <summary>HUD strip: days since the best geiger was last calibrated (-1 if none).</summary>
        public void SetCalibrationAge(int daysSinceCalibration)
        {
            DaysSinceCalibration = daysSinceCalibration;
        }

        /// <summary>Confidence ring fill for a location (0..1), or 0 if unknown.</summary>
        public float GetConfidence(string locationId)
        {
            for (int i = 0; i < _views.Count; i++)
            {
                if (_views[i].LocationId == locationId)
                {
                    return _views[i].Confidence;
                }
            }
            return 0f;
        }

        /// <summary>Display string for the calibration age strip.</summary>
        public string GetCalibrationLabel()
        {
            if (DaysSinceCalibration < 0) return "no geiger";
            if (DaysSinceCalibration == 0) return "last calibrated: today";
            if (DaysSinceCalibration == 1) return "last calibrated: 1 day ago";
            return $"last calibrated: {DaysSinceCalibration} days ago";
        }

        /// <summary>
        /// Label for a tile: "?", "unreliable", or the displayed rad (raw debug only).
        /// </summary>
        public string GetTileLabel(string locationId, bool showRawValues = false)
        {
            for (int i = 0; i < _views.Count; i++)
            {
                var v = _views[i];
                if (v.LocationId != locationId) continue;
                if (v.IsDark || v.IsUnknown) return "?";
                // Always surface the player-facing estimate (uncertain number), never TrueRad.
                if (!float.IsNaN(v.DisplayedRad))
                {
                    string num = $"~{v.DisplayedRad:0}";
                    if (v.IsUnreliable) return showRawValues ? $"{num} unreliable" : $"{num}?";
                    return showRawValues ? $"{v.DisplayedRad:0} rad/h" : num;
                }
                if (v.IsUnreliable) return "unreliable";
                return "surveyed";
            }
            return "?";
        }

        /// <summary>Player-facing rad estimate for planning UI (NaN if unknown/dark).</summary>
        public float GetDisplayedRad(string locationId)
        {
            for (int i = 0; i < _views.Count; i++)
            {
                if (_views[i].LocationId == locationId)
                {
                    return _views[i].DisplayedRad;
                }
            }
            return float.NaN;
        }
    }
}
