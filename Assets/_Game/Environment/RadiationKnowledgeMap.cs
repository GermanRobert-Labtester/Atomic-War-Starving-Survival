using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Radiation fog-of-war: per-location truth vs player knowledge.
    /// Measurements come only from working instruments (wired by Core/scavenging);
    /// this class never invents a reading on its own.
    /// </summary>
    public class RadiationKnowledgeMap
    {
        /// <summary>Days a measurement stays "fresh" before blending toward rumor.</summary>
        public const float FreshnessDays = 3f;

        /// <summary>Uncertainty added per day without a fresh reliable measure.</summary>
        public const float UncertaintyGrowthPerDay = 0.12f;

        /// <summary>Ceiling on rumor uncertainty.</summary>
        public const float MaxUncertainty = 1f;

        /// <summary>
        /// Calibration at/above this at survey time counts as reliable for freshness.
        /// Kept in sync with InstrumentDevice.ReliableCalibrationThreshold by design —
        /// Environment must not reference Radiation assembly, so the numeric is duplicated
        /// and both sides use the same 0.85f value.
        /// </summary>
        public const float ReliableCalibrationThreshold = 0.85f;

        private readonly Dictionary<string, MapTile> _tiles = new Dictionary<string, MapTile>();

        public event Action OnKnowledgeChanged;

        public IReadOnlyDictionary<string, MapTile> Tiles => _tiles;

        /// <summary>Register or overwrite a tile's true rad and optional initial rumor.</summary>
        public void SeedTile(string locationId, float trueRad, float rumoredRad = -1f, float initialUncertainty = 1f)
        {
            if (string.IsNullOrEmpty(locationId)) return;

            var tile = new MapTile
            {
                LocationId = locationId,
                TrueRad = Mathf.Max(0f, trueRad),
                RumoredRad = rumoredRad >= 0f ? rumoredRad : Mathf.Max(0f, trueRad * 0.5f),
                RumorUncertainty = Mathf.Clamp01(initialUncertainty),
                MeasuredAtDay = -1,
                Surveyed = false
            };
            _tiles[locationId] = tile;
            OnKnowledgeChanged?.Invoke();
        }

        public MapTile GetTile(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            return _tiles.TryGetValue(locationId, out var tile) ? tile : null;
        }

        public float GetTrueRad(string locationId)
        {
            var tile = GetTile(locationId);
            return tile != null ? tile.TrueRad : 0f;
        }

        /// <summary>
        /// Record a successful survey. Caller must have already validated the device
        /// and computed the (possibly biased) measured reading.
        /// </summary>
        public bool RecordSurvey(string locationId, float measuredRad, float deviceCalibration, int day)
        {
            var tile = GetTile(locationId);
            if (tile == null) return false;

            tile.MeasuredRad = Mathf.Max(0f, measuredRad);
            tile.MeasuredAtDay = day;
            tile.MeasuredWithCalibration = Mathf.Clamp01(deviceCalibration);
            tile.Surveyed = true;
            tile.RumorUncertainty = 0f;
            // Keep RumoredRad independent: as the measurement ages, the view blends
            // back toward whatever survivors still "think" (rumors/events), not a
            // copy of the instrument reading.
            OnKnowledgeChanged?.Invoke();
            return true;
        }

        /// <summary>Inject a rumor without surveying (events / radio).</summary>
        public void SetRumor(string locationId, float rumoredRad, float uncertainty)
        {
            var tile = GetTile(locationId);
            if (tile == null) return;
            tile.RumoredRad = Mathf.Max(0f, rumoredRad);
            tile.RumorUncertainty = Mathf.Clamp(uncertainty, 0f, MaxUncertainty);
            OnKnowledgeChanged?.Invoke();
        }

        /// <summary>
        /// Daily fog growth: uncertainty rises on every tile that is unsurveyed or
        /// whose last measurement is past freshness.
        /// </summary>
        public void TickDay(int currentDay)
        {
            foreach (var kv in _tiles)
            {
                var tile = kv.Value;
                if (tile == null) continue;

                bool fresh = IsMeasurementFresh(tile, currentDay)
                             && tile.MeasuredWithCalibration >= ReliableCalibrationThreshold;
                if (!fresh)
                {
                    tile.RumorUncertainty = Mathf.Min(
                        MaxUncertainty,
                        tile.RumorUncertainty + UncertaintyGrowthPerDay);
                }
            }
            OnKnowledgeChanged?.Invoke();
        }

        public static bool IsMeasurementFresh(MapTile tile, int currentDay)
        {
            if (tile == null || !tile.Surveyed || tile.MeasuredAtDay < 0) return false;
            return (currentDay - tile.MeasuredAtDay) < FreshnessDays;
        }

        /// <summary>
        /// Player-facing view for UI. When hasWorkingGeiger is false the whole map
        /// goes dark — flying blind.
        /// </summary>
        public MapTilePlayerView GetPlayerView(string locationId, int currentDay, bool hasWorkingGeiger)
        {
            var tile = GetTile(locationId);
            var view = new MapTilePlayerView
            {
                LocationId = locationId,
                DisplayedRad = float.NaN,
                Confidence = 0f,
                IsUnreliable = true,
                IsUnknown = true,
                IsDark = !hasWorkingGeiger,
                Surveyed = tile != null && tile.Surveyed
            };

            if (!hasWorkingGeiger)
            {
                view.Confidence = 0f;
                view.IsUnknown = true;
                view.IsUnreliable = true;
                return view;
            }

            if (tile == null)
            {
                return view;
            }

            bool fresh = IsMeasurementFresh(tile, currentDay);
            bool calOk = tile.MeasuredWithCalibration >= ReliableCalibrationThreshold;
            bool reliableFresh = tile.Surveyed && fresh && calOk;

            if (!tile.Surveyed)
            {
                // Unsurveyed: rumor or "?"
                if (tile.RumorUncertainty >= 0.99f && tile.RumoredRad <= 0f)
                {
                    view.IsUnknown = true;
                    view.DisplayedRad = float.NaN;
                    view.Confidence = 0f;
                    view.IsUnreliable = true;
                }
                else
                {
                    view.IsUnknown = false;
                    view.DisplayedRad = tile.RumoredRad;
                    view.Confidence = Mathf.Clamp01(1f - tile.RumorUncertainty);
                    view.IsUnreliable = true;
                }
                return view;
            }

            if (reliableFresh)
            {
                view.IsUnknown = false;
                view.DisplayedRad = tile.MeasuredRad;
                view.Confidence = Mathf.Clamp01(1f - tile.RumorUncertainty);
                view.IsUnreliable = false;
                return view;
            }

            // Stale or uncalibrated: blend measured → rumored by age / uncertainty
            float ageDays = tile.MeasuredAtDay >= 0
                ? Mathf.Max(0f, currentDay - tile.MeasuredAtDay)
                : FreshnessDays;
            float staleT = Mathf.Clamp01(ageDays / (FreshnessDays * 2f));
            // Also pull toward rumor by uncertainty mass
            float blend = Mathf.Clamp01(Mathf.Max(staleT, tile.RumorUncertainty));
            float displayed = Mathf.Lerp(tile.MeasuredRad, tile.RumoredRad, blend);

            // Mis-calibrated fresh reading: still show the lie, but flag unreliable
            // and cap confidence so a known-bad instrument never shows a full ring.
            if (fresh && !calOk)
            {
                displayed = tile.MeasuredRad;
                blend = 0f;
                view.IsUnknown = false;
                view.DisplayedRad = displayed;
                view.Confidence = Mathf.Clamp01(tile.MeasuredWithCalibration);
                view.IsUnreliable = true;
                return view;
            }

            view.IsUnknown = false;
            view.DisplayedRad = displayed;
            view.Confidence = Mathf.Clamp01(1f - Mathf.Max(tile.RumorUncertainty, blend));
            view.IsUnreliable = true;
            return view;
        }

        public List<MapTilePlayerView> GetAllPlayerViews(int currentDay, bool hasWorkingGeiger)
        {
            var list = new List<MapTilePlayerView>(_tiles.Count);
            foreach (var id in _tiles.Keys)
            {
                list.Add(GetPlayerView(id, currentDay, hasWorkingGeiger));
            }
            return list;
        }

        /// <summary>
        /// Buffer overload: clears <paramref name="buffer"/> and fills it in place,
        /// so steady-state fog-of-war refreshes allocate nothing (pool-friendly hot path).
        /// </summary>
        public void GetAllPlayerViews(List<MapTilePlayerView> buffer, int currentDay, bool hasWorkingGeiger)
        {
            if (buffer == null) return;
            buffer.Clear();
            foreach (var id in _tiles.Keys)
            {
                buffer.Add(GetPlayerView(id, currentDay, hasWorkingGeiger));
            }
        }

        // -----------------------------------------------------------------
        // Save / load
        // -----------------------------------------------------------------

        public RadiationKnowledgeSave CaptureState()
        {
            var save = new RadiationKnowledgeSave();
            foreach (var kv in _tiles)
            {
                if (kv.Value == null) continue;
                save.Tiles.Add(kv.Value.Clone());
            }
            return save;
        }

        public void RestoreState(RadiationKnowledgeSave save)
        {
            _tiles.Clear();
            if (save?.Tiles == null)
            {
                OnKnowledgeChanged?.Invoke();
                return;
            }

            foreach (var tile in save.Tiles)
            {
                if (tile == null || string.IsNullOrEmpty(tile.LocationId)) continue;
                _tiles[tile.LocationId] = tile.Clone();
            }
            OnKnowledgeChanged?.Invoke();
        }
    }

    [Serializable]
    public class RadiationKnowledgeSave
    {
        public List<MapTile> Tiles = new List<MapTile>();
    }
}
