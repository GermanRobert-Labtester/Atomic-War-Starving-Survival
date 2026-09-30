// SPDX-License-Identifier: MIT
using System;
#pragma warning disable CS8618

namespace Ashfall.Core.YearOfAsh
{
    public enum YearOfAshPhase
    {
        Phase4_DeepFreeze = 0,    // Days 180 - 240: -35C, ash cloud peak, frozen intake
        Phase5_FactionSiege = 1,  // Days 241 - 300: Artillery barrages, Continuity Decree
        Phase6_TheGreatThaw = 2,  // Days 301 - 360: Black mud runoff, radon gas, final broadcasts
        Phase7_TheLongThaw = 3    // Days 361 - 720: Year Two extended climate cycle
    }

    [Serializable]
    public class YearOfAshTimelineState
    {
        public int currentDay = 180;
        public YearOfAshPhase phase = YearOfAshPhase.Phase4_DeepFreeze;
        public float ambientTemperatureCelsius = -35.0f;
        public float ashCloudOpacity = 0.85f;
        public float radonInfiltrationRate = 0.05f;
        public float thermalStressLevel = 0.40f;
        public int blackBlizzardsExperienced = 0;
        public int artilleryBarragesExperienced = 0;
        public bool continuityDecreeActive = false;
        public bool finalBroadcastsActive = false;
        public string activeClimatePhaseId = "";
        public int yearTwoQuarter = 0;
    }

    /// <summary>
    /// Engine-agnostic timeline & environmental season controller for Days 180 to 360.
    /// Simulates the transition from Deep Freeze to Faction Siege to the Great Thaw.
    /// Zero engine dependencies; deterministic.
    /// </summary>
    public class YearOfAshTimelineSystem
    {
        public const int StartDay = 180;
        public const int LegacyEndDay = 360;
        public const int YearTwoEndDay = 720;
        public const int EndDay = LegacyEndDay;

        private readonly YearOfAshTimelineState _state;
        private YearTwoClimateCatalog? _yearTwoCatalog;

        public YearOfAshTimelineState State => _state;
        public int CurrentDay => _state.currentDay;
        public YearOfAshPhase CurrentPhase => _state.phase;
        public float AmbientTemperatureCelsius => _state.ambientTemperatureCelsius;
        public float AshCloudOpacity => _state.ashCloudOpacity;
        public float RadonInfiltrationRate => _state.radonInfiltrationRate;
        public float ThermalStressLevel => _state.thermalStressLevel;
        public bool ContinuityDecreeActive => _state.continuityDecreeActive;
        public bool FinalBroadcastsActive => _state.finalBroadcastsActive;
        public string ActiveClimatePhaseId => _state.activeClimatePhaseId;
        public int YearTwoQuarter => _state.yearTwoQuarter;
        public YearTwoClimateCatalog? YearTwoCatalog => _yearTwoCatalog;
        public int EffectiveEndDay => (_yearTwoCatalog != null && _yearTwoCatalog.IsValid) ? YearTwoEndDay : EndDay;

        public event Action<YearOfAshPhase> OnPhaseTransitioned;
        public event Action<int, string> OnEnvironmentalCrisisTriggered;
        public event Action<int> OnDayAdvanced;

        public YearOfAshTimelineSystem(YearOfAshTimelineState? state = null)
        {
            _state = state ?? new YearOfAshTimelineState();
            RecalculateEnvironmentalParameters();
        }

        public void BindYearTwoClimateCatalog(YearTwoClimateCatalog? catalog)
        {
            _yearTwoCatalog = catalog;
            RecalculateEnvironmentalParameters();
        }

        public void AdvanceDay(int day)
        {
            if (day < StartDay) day = StartDay;
            int maxDay = EffectiveEndDay;
            if (day > maxDay) day = maxDay;
            if (day <= _state.currentDay) return;

            int previousDay = _state.currentDay;
            _state.currentDay = day;

            YearOfAshPhase oldPhase = _state.phase;
            _state.phase = PhaseForDay(day);
            if (_state.phase == YearOfAshPhase.Phase5_FactionSiege)
            {
                if (!_state.continuityDecreeActive)
                {
                    _state.continuityDecreeActive = true;
                    OnEnvironmentalCrisisTriggered?.Invoke(day, "Continuity Reclamation Decree officially issued across Sector 4 frequencies.");
                }
            }
            else if (_state.phase == YearOfAshPhase.Phase6_TheGreatThaw)
            {
                if (!_state.finalBroadcastsActive)
                {
                    _state.finalBroadcastsActive = true;
                    OnEnvironmentalCrisisTriggered?.Invoke(day, "Long-wave emergency broadcast frequency 142.850 MHz opened.");
                }
            }

            RecalculateEnvironmentalParameters();

            if (oldPhase != _state.phase)
            {
                OnPhaseTransitioned?.Invoke(_state.phase);
            }

            OnDayAdvanced?.Invoke(_state.currentDay);
        }

        public void RecalculateEnvironmentalParameters()
        {
            int d = _state.currentDay;
            _state.phase = PhaseForDay(d);

            if (d > EndDay && _yearTwoCatalog != null && _yearTwoCatalog.IsValid)
            {
                if (_yearTwoCatalog.TryGetPhaseForDay(d, out var quarterPhase) && quarterPhase != null)
                {
                    _state.activeClimatePhaseId = quarterPhase.phase_id;
                    _state.yearTwoQuarter = quarterPhase.quarter;
                    var evaluation = _yearTwoCatalog.EvaluateDay(d);
                    _state.ambientTemperatureCelsius = evaluation.TemperatureCelsius;
                    _state.ashCloudOpacity = evaluation.AshCloudOpacity;
                    _state.radonInfiltrationRate = evaluation.RadonInfiltrationRate;
                    _state.thermalStressLevel = evaluation.ThermalStressLevel;
                    return;
                }
            }

            _state.activeClimatePhaseId = "";
            _state.yearTwoQuarter = 0;

            if (_state.phase == YearOfAshPhase.Phase4_DeepFreeze)
            {
                // Days 180 -> 240: Drops from -25C to -45C at day 210, then rises to -30C
                float t = (d - 180) / 60.0f;
                _state.ambientTemperatureCelsius = -25.0f - (20.0f * (float)Math.Sin(t * Math.PI));
                _state.ashCloudOpacity = 0.85f + (0.10f * t);
                _state.radonInfiltrationRate = 0.05f;
                _state.thermalStressLevel = 0.65f + (0.25f * t);
            }
            else if (_state.phase == YearOfAshPhase.Phase5_FactionSiege)
            {
                // Days 241 -> 300: Ambient temp -30C to -10C; artillery dust increases ash opacity
                float t = (d - 240) / 60.0f;
                _state.ambientTemperatureCelsius = -30.0f + (20.0f * t);
                _state.ashCloudOpacity = 0.90f - (0.15f * t);
                _state.radonInfiltrationRate = 0.15f + (0.10f * t);
                _state.thermalStressLevel = 0.50f - (0.20f * t);
            }
            else if (_state.phase == YearOfAshPhase.Phase6_TheGreatThaw)
            {
                // Days 301 -> 360: Ambient temp rises from -10C to +4C; ash clears, radon spikes due to melting permafrost
                float t = (Math.Min(d, EndDay) - 300) / 60.0f;
                _state.ambientTemperatureCelsius = -10.0f + (14.0f * t);
                _state.ashCloudOpacity = 0.75f - (0.45f * t);
                _state.radonInfiltrationRate = 0.25f + (0.50f * t);
                _state.thermalStressLevel = 0.20f;
            }
            else // Phase7_TheLongThaw without valid catalog
            {
                _state.ambientTemperatureCelsius = 4.0f;
                _state.ashCloudOpacity = 0.30f;
                _state.radonInfiltrationRate = 0.75f;
                _state.thermalStressLevel = 0.20f;
            }
        }

        public float CalculateCaloricMultiplier()
        {
            // Cold environment drastically raises metabolic demands
            if (_state.ambientTemperatureCelsius < -20.0f)
                return 1.40f; // +40% food burn in deep freeze
            if (_state.ambientTemperatureCelsius < 0.0f)
                return 1.20f;
            return 1.00f;
        }

        public YearOfAshTimelineState CaptureState()
        {
            return new YearOfAshTimelineState
            {
                currentDay = _state.currentDay,
                phase = _state.phase,
                ambientTemperatureCelsius = _state.ambientTemperatureCelsius,
                ashCloudOpacity = _state.ashCloudOpacity,
                radonInfiltrationRate = _state.radonInfiltrationRate,
                thermalStressLevel = _state.thermalStressLevel,
                blackBlizzardsExperienced = _state.blackBlizzardsExperienced,
                artilleryBarragesExperienced = _state.artilleryBarragesExperienced,
                continuityDecreeActive = _state.continuityDecreeActive,
                finalBroadcastsActive = _state.finalBroadcastsActive,
                activeClimatePhaseId = _state.activeClimatePhaseId,
                yearTwoQuarter = _state.yearTwoQuarter
            };
        }

        /// <summary>
        /// Restores a captured timeline snapshot into the live state. Recomputes
        /// the derived environmental parameters afterwards so the restored day
        /// and phase always agree with the reported temperature/radon values.
        /// A null state is treated as "nothing to restore" (v1 saves carry no
        /// separate timeline section only if truncated; the host guards first).
        /// </summary>
        public void RestoreState(YearOfAshTimelineState state)
        {
            if (state == null) return;
            int maxDay = EffectiveEndDay;
            _state.currentDay = Math.Max(StartDay, Math.Min(maxDay, state.currentDay));
            _state.phase = PhaseForDay(_state.currentDay);
            _state.ambientTemperatureCelsius = state.ambientTemperatureCelsius;
            _state.ashCloudOpacity = state.ashCloudOpacity;
            _state.radonInfiltrationRate = state.radonInfiltrationRate;
            _state.thermalStressLevel = state.thermalStressLevel;
            _state.blackBlizzardsExperienced = state.blackBlizzardsExperienced;
            _state.artilleryBarragesExperienced = state.artilleryBarragesExperienced;
            _state.continuityDecreeActive = state.continuityDecreeActive;
            _state.finalBroadcastsActive = state.finalBroadcastsActive;
            _state.activeClimatePhaseId = state.activeClimatePhaseId ?? "";
            _state.yearTwoQuarter = state.yearTwoQuarter;
            RecalculateEnvironmentalParameters();
        }

        public static YearOfAshPhase PhaseForDay(int day)
        {
            if (day <= 240) return YearOfAshPhase.Phase4_DeepFreeze;
            if (day <= 300) return YearOfAshPhase.Phase5_FactionSiege;
            if (day <= 360) return YearOfAshPhase.Phase6_TheGreatThaw;
            return YearOfAshPhase.Phase7_TheLongThaw;
        }
    }
}
