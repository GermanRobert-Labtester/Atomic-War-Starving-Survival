using System;
#pragma warning disable CS8618

namespace Ashfall.Core.YearOfAsh
{
    [Serializable]
    public class YearOfAshRadonState
    {
        public float indoorRadonBqm3 = 120.0f; // Baseline Becquerels per cubic meter (Safe < 200, Dangerous > 800)
        public float scrubberFilterHealthPercent = 100.0f; // 0 - 100%
        public int activeFoundationFissures = 0;
        public float totalAlphaDoseLogged = 0.0f;
        public bool isScrubberAlarmActive = false;
    }

    /// <summary>
    /// Engine-agnostic radon gas infiltration & ventilation scrubber simulation for Phase VI (Days 300–360).
    /// Simulates radioactive gas release from thawing bedrock permafrost fissures.
    /// Pure C#; zero engine dependencies.
    /// </summary>
    public class YearOfAshRadonSystem
    {
        public const float SafeRadonThreshold = 200.0f;
        public const float DangerousRadonThreshold = 800.0f;

        private readonly YearOfAshRadonState _state;

        public YearOfAshRadonState State => _state;
        public float IndoorRadonBqm3 => _state.indoorRadonBqm3;
        public float ScrubberHealthPercent => _state.scrubberFilterHealthPercent;
        public int ActiveFissures => _state.activeFoundationFissures;
        public bool IsDangerous => _state.indoorRadonBqm3 >= DangerousRadonThreshold;

        public event Action<float> OnRadonLevelChanged;
        public event Action<string> OnRadonAlarmTriggered;

        public YearOfAshRadonSystem(YearOfAshRadonState state = null!)
        {
            _state = state ?? new YearOfAshRadonState();
        }

        public void TickDailyRadon(int day, float ambientTempCelsius)
        {
            if (day < 300)
            {
                // Pre-thaw: baseline radon with slow diffusion
                _state.indoorRadonBqm3 = Math.Max(80.0f, _state.indoorRadonBqm3 - 5.0f);
                return;
            }

            // In Phase VI, thawing permafrost releases Radon-222 from bedrock fissures
            if (ambientTempCelsius > 0.0f && _state.activeFoundationFissures == 0)
            {
                _state.activeFoundationFissures = 1;
            }

            float rawInfiltration = 120.0f + (_state.activeFoundationFissures * 280.0f);

            // Scrubber filtration effect
            float scrubEfficiency = _state.scrubberFilterHealthPercent / 100.0f;
            float scrubbedRate = rawInfiltration * (1.0f - (scrubEfficiency * 0.70f));

            _state.indoorRadonBqm3 = Math.Min(3500.0f, _state.indoorRadonBqm3 + scrubbedRate);

            // Scrubber wears down under high radon load
            if (_state.indoorRadonBqm3 > SafeRadonThreshold)
            {
                float degradation = (_state.indoorRadonBqm3 / 1000.0f) * 1.5f;
                _state.scrubberFilterHealthPercent = Math.Max(0.0f, _state.scrubberFilterHealthPercent - degradation);
            }

            // Alpha dose accumulation
            if (_state.indoorRadonBqm3 > DangerousRadonThreshold)
            {
                _state.totalAlphaDoseLogged += (_state.indoorRadonBqm3 - DangerousRadonThreshold) * 0.001f;
                if (!_state.isScrubberAlarmActive)
                {
                    _state.isScrubberAlarmActive = true;
                    OnRadonAlarmTriggered?.Invoke($"CRITICAL: Subterranean Radon-222 levels exceeded {DangerousRadonThreshold} Bq/m³!");
                }
            }
            else
            {
                _state.isScrubberAlarmActive = false;
            }

            OnRadonLevelChanged?.Invoke(_state.indoorRadonBqm3);
        }

        public bool ReplaceScrubberFilter()
        {
            _state.scrubberFilterHealthPercent = 100.0f;
            _state.indoorRadonBqm3 = Math.Max(80.0f, _state.indoorRadonBqm3 * 0.35f);
            _state.isScrubberAlarmActive = false;
            OnRadonLevelChanged?.Invoke(_state.indoorRadonBqm3);
            return true;
        }

        public bool SealFoundationFissures()
        {
            if (_state.activeFoundationFissures > 0)
            {
                _state.activeFoundationFissures = 0;
                _state.indoorRadonBqm3 = Math.Max(90.0f, _state.indoorRadonBqm3 * 0.6f);
                OnRadonLevelChanged?.Invoke(_state.indoorRadonBqm3);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Snapshot for the save envelope. Returns a copy so later ticks cannot mutate
        /// a capture that is still on its way to disk.
        /// </summary>
        public YearOfAshRadonState CaptureState()
        {
            return new YearOfAshRadonState
            {
                indoorRadonBqm3 = _state.indoorRadonBqm3,
                scrubberFilterHealthPercent = _state.scrubberFilterHealthPercent,
                activeFoundationFissures = _state.activeFoundationFissures,
                totalAlphaDoseLogged = _state.totalAlphaDoseLogged,
                isScrubberAlarmActive = _state.isScrubberAlarmActive
            };
        }

        /// <summary>
        /// Rebuilds live state from a snapshot. Tolerates a null section so a save
        /// written before this section existed restores as "fresh scrubber".
        /// </summary>
        public void RestoreState(YearOfAshRadonState state)
        {
            if (state == null) return;
            _state.indoorRadonBqm3 = state.indoorRadonBqm3;
            _state.scrubberFilterHealthPercent = state.scrubberFilterHealthPercent;
            _state.activeFoundationFissures = state.activeFoundationFissures;
            _state.totalAlphaDoseLogged = state.totalAlphaDoseLogged;
            _state.isScrubberAlarmActive = state.isScrubberAlarmActive;
            OnRadonLevelChanged?.Invoke(_state.indoorRadonBqm3);
        }
    }
}
