using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.YearOfAsh
{
    /// <summary>
    /// Persisted state for the ice-road economy system.
    /// Roads freeze solid during Deep Freeze when ambient temp is at or below -20°C
    /// and no thaw-class storm is active.
    /// </summary>
    [Serializable]
    public class IceRoadState
    {
        public bool iceRoadOpen;
        public int lastOpenDay = -1;
        public int lastClosedDay = -1;
        /// <summary>Running accumulator of expedition exposure risk while the road is open.</summary>
        public float cumulativeExposureScore;
        /// <summary>Total number of days the road has been passable this season.</summary>
        public int totalTradeWindowDays;
    }

    /// <summary>
    /// Engine-agnostic ice-road economy system for the Year of Ash expansion.
    ///
    /// The ice road opens when ambient temperature drops to or below -20°C AND no
    /// thaw_flood or thermal_inversion storm window is active. When open it grants a
    /// supply trade multiplier (1.4×); when closed expeditions are more costly (0.6×).
    ///
    /// Deterministic — no ISeededRng usage needed; road status is fully determined
    /// by temperature and the storm catalog.
    /// </summary>
    public sealed class YearOfAshIceRoadSystem
    {
        private readonly IceRoadState _state;

        public IceRoadState State => _state;
        public bool IsIceRoadOpen => _state.iceRoadOpen;

        /// <summary>Fired when the road opens or closes. (day, isNowOpen)</summary>
        public event Action<int, bool>? OnIceRoadStatusChanged;

        public YearOfAshIceRoadSystem(IceRoadState? state = null)
        {
            _state = state ?? new IceRoadState();
        }

        /// <summary>
        /// Evaluates ice-road status for the given day.
        /// Call this after <see cref="YearOfAshTimelineSystem.AdvanceDay"/> each tick.
        /// </summary>
        /// <param name="day">Current simulation day.</param>
        /// <param name="ambientTempCelsius">From YearOfAshTimelineSystem.AmbientTemperatureCelsius.</param>
        /// <param name="activeStorms">Active storm entries for today (may be null or empty).</param>
        public void TickDay(int day, float ambientTempCelsius, IReadOnlyList<StormWindowEntry>? activeStorms = null)
        {
            bool blockingStorm = false;
            if (activeStorms != null)
            {
                for (int i = 0; i < activeStorms.Count; i++)
                {
                    string t = activeStorms[i]?.type ?? string.Empty;
                    if (t == "thaw_flood" || t == "thermal_inversion")
                    {
                        blockingStorm = true;
                        break;
                    }
                }
            }

            bool shouldBeOpen = ambientTempCelsius <= -20f && !blockingStorm;
            bool wasOpen = _state.iceRoadOpen;

            _state.iceRoadOpen = shouldBeOpen;

            if (shouldBeOpen)
            {
                _state.lastOpenDay = day;
                _state.totalTradeWindowDays++;
                _state.cumulativeExposureScore += GetExpeditionExposureRisk();
            }
            else
            {
                _state.lastClosedDay = day;
            }

            if (wasOpen != shouldBeOpen)
                OnIceRoadStatusChanged?.Invoke(day, shouldBeOpen);
        }

        /// <summary>
        /// Trade supply multiplier.
        /// Open road: 1.4× (pack sledges can move heavy loads).
        /// Closed road: 0.6× (routes impassable or dangerous).
        /// </summary>
        public float GetTradeMultiplier() => _state.iceRoadOpen ? 1.4f : 0.6f;

        /// <summary>
        /// Per-day expedition exposure risk contribution.
        /// Higher when the road is open because expeditions venture further across exposed flats.
        /// </summary>
        public float GetExpeditionExposureRisk() => _state.iceRoadOpen ? 0.30f : 0.10f;

        public IceRoadState CaptureState()
        {
            return new IceRoadState
            {
                iceRoadOpen = _state.iceRoadOpen,
                lastOpenDay = _state.lastOpenDay,
                lastClosedDay = _state.lastClosedDay,
                cumulativeExposureScore = _state.cumulativeExposureScore,
                totalTradeWindowDays = _state.totalTradeWindowDays
            };
        }

        public void RestoreState(IceRoadState? state)
        {
            if (state == null) return;
            _state.iceRoadOpen = state.iceRoadOpen;
            _state.lastOpenDay = state.lastOpenDay;
            _state.lastClosedDay = state.lastClosedDay;
            _state.cumulativeExposureScore = state.cumulativeExposureScore < 0f ? 0f : state.cumulativeExposureScore;
            _state.totalTradeWindowDays = state.totalTradeWindowDays < 0 ? 0 : state.totalTradeWindowDays;
        }
    }
}
