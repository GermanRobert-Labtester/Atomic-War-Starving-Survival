using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Save-safe snapshot for PhotoperiodSystem; plain primitives only so
    /// JsonUtility can round-trip it inside SaveData without reflection pain.
    /// </summary>
    [Serializable]
    public class PhotoperiodState
    {
        public float TotalElapsedHours;
        public float AshBlackoutHoursRemaining;
    }

    /// <summary>
    /// Tracks two world-level light variables every game tick:
    ///
    ///   • <see cref="DaylightHours"/>  — base hours of sunlight today, read from
    ///     SeasonProfile.daylightCurve (shrinks as nuclear winter deepens).
    ///
    ///   • <see cref="SkyClarity"/>     — ash / fallout attenuator (0..1); derived
    ///     from the current WeatherSystem state.  An ash blackout event can push
    ///     this to near-zero for several consecutive days regardless of the curve.
    ///
    ///   • <see cref="EffectiveDaylightHours"/> — DaylightHours × SkyClarity,
    ///     the value NeedsSystem uses to move each survivor's LightExposure.
    ///
    /// Raises <see cref="OnPhotoPeriodChanged"/> whenever either variable changes.
    /// Deterministic and save/load safe: state is a single plain struct.
    /// </summary>
    public class PhotoperiodSystem
    {
        // -------------------------------------------------------------------
        // Sky-clarity constants per weather kind
        // -------------------------------------------------------------------

        public const float ClarityForClear       = 1.00f;
        public const float ClarityForAshfall     = 0.45f;
        public const float ClarityForFalloutStorm= 0.05f;
        public const float ClarityForBlizzard    = 0.25f;

        /// <summary>How many in-game hours an ash-blackout event lasts (0-hour daylight).</summary>
        public const float AshBlackoutDurationHours = 72f; // 3 days

        // -------------------------------------------------------------------
        // Internal state
        // -------------------------------------------------------------------

        private readonly SeasonProfile _profile;
        private readonly WeatherSystem _weatherSystem;
        private float _totalElapsedHours;
        private float _ashBlackoutHoursRemaining;

        // -------------------------------------------------------------------
        // Published properties
        // -------------------------------------------------------------------

        /// <summary>Base daylight hours today as read from the campaign's daylight curve (0..16).</summary>
        public float DaylightHours { get; private set; }

        /// <summary>Ash/fallout sky attenuator (0..1): 1 = clear, 0 = total blackout.</summary>
        public float SkyClarity { get; private set; } = 1f;

        /// <summary>Effective daylight hours after ash loading: DaylightHours × SkyClarity.</summary>
        public float EffectiveDaylightHours => Mathf.Clamp(DaylightHours * SkyClarity, 0f, 16f);

        /// <summary>True while an ash blackout event is active (sky forced to near-zero clarity).</summary>
        public bool IsAshBlackout => _ashBlackoutHoursRemaining > 0f;

        /// <summary>
        /// Fired whenever DaylightHours or SkyClarity changes (not necessarily every tick —
        /// only on actual value transitions, within floating-point epsilon).
        /// Args: (daylightHours, skyClarity, effectiveDaylightHours).
        /// </summary>
        public event Action<float, float, float> OnPhotoPeriodChanged;

        // -------------------------------------------------------------------
        // Construction
        // -------------------------------------------------------------------

        /// <summary>Legacy / manual mode: Tick is a no-op; drive state via ForceAshBlackout.</summary>
        public PhotoperiodSystem() : this(null, null) { }

        /// <summary>
        /// Nuclear-winter mode: DaylightHours and SkyClarity advance automatically on Tick.
        /// </summary>
        public PhotoperiodSystem(SeasonProfile profile, WeatherSystem weatherSystem)
        {
            _profile = profile;
            _weatherSystem = weatherSystem;

            DaylightHours = profile != null ? profile.EvaluateDaylightHours(0f) : 12f;
            SkyClarity = ComputeSkyClarity();
        }

        // -------------------------------------------------------------------
        // Tick
        // -------------------------------------------------------------------

        /// <summary>
        /// Advance photoPeriod state over <paramref name="gameHours"/> of game time.
        /// Should be called once per frame before NeedsSystem.Tick so survivor
        /// LightExposure reads the freshly updated EffectiveDaylightHours.
        /// No-op in legacy/manual mode.
        /// </summary>
        public void Tick(float gameHours)
        {
            if (_profile == null || gameHours <= 0f) return;

            _totalElapsedHours += gameHours;

            // Drain ash blackout timer
            if (_ashBlackoutHoursRemaining > 0f)
            {
                _ashBlackoutHoursRemaining = Mathf.Max(0f, _ashBlackoutHoursRemaining - gameHours);
            }

            float prevDaylight  = DaylightHours;
            float prevClarity   = SkyClarity;

            DaylightHours = _profile.EvaluateDaylightHours(_totalElapsedHours);
            SkyClarity    = ComputeSkyClarity();

            const float eps = 1e-4f;
            if (Mathf.Abs(DaylightHours - prevDaylight) > eps ||
                Mathf.Abs(SkyClarity    - prevClarity)  > eps)
            {
                OnPhotoPeriodChanged?.Invoke(DaylightHours, SkyClarity, EffectiveDaylightHours);
            }
        }

        // -------------------------------------------------------------------
        // Survivor-level light accounting
        // -------------------------------------------------------------------

        /// <summary>
        /// Advance a single survivor's <see cref="Survivor.LightExposure"/>
        /// and <see cref="Survivor.VitaminDProxy"/> for one tick.
        ///
        /// Delegates to <see cref="LightSystemHelper.TickSurvivorLight"/> which lives
        /// in the Survivors assembly, avoiding a circular reference from NeedsSystem.
        /// </summary>
        public static void TickSurvivorLight(
            Survivor     sv,
            float        gameHours,
            float        effectiveDaylightHours,
            bool         growLightActive,
            LightProfile lightProfile)
        {
            LightSystemHelper.TickSurvivorLight(sv, gameHours, effectiveDaylightHours, growLightActive, lightProfile);
        }

        // -------------------------------------------------------------------
        // Debug / scripted events
        // -------------------------------------------------------------------

        /// <summary>
        /// Trigger an ash-blackout event: SkyClarity is clamped to near-zero for
        /// <see cref="AshBlackoutDurationHours"/> in-game hours regardless of weather.
        /// Safe to call multiple times; each call resets the timer to the full duration.
        /// </summary>
        public void ForceAshBlackout()
        {
            _ashBlackoutHoursRemaining = AshBlackoutDurationHours;
            SkyClarity = ComputeSkyClarity();
            OnPhotoPeriodChanged?.Invoke(DaylightHours, SkyClarity, EffectiveDaylightHours);
        }

        /// <summary>
        /// Apply a one-shot light boost to a survivor's LightExposure (e.g. a sun-lamp
        /// session). Delegates to <see cref="LightSystemHelper.ApplySunLampSession"/>.
        /// </summary>
        public static void ApplySunLampSession(
            Survivor     sv,
            float        boostAmount,
            LightProfile lightProfile)
        {
            LightSystemHelper.ApplySunLampSession(sv, boostAmount, lightProfile);
        }

        // -------------------------------------------------------------------
        // Save / Load
        // -------------------------------------------------------------------

        /// <summary>Export a save-safe snapshot.</summary>
        public PhotoperiodState GetState()
        {
            return new PhotoperiodState
            {
                TotalElapsedHours          = _totalElapsedHours,
                AshBlackoutHoursRemaining  = _ashBlackoutHoursRemaining
            };
        }

        /// <summary>Restore from a save-safe snapshot; does NOT fire events.</summary>
        public void RestoreState(PhotoperiodState state)
        {
            if (state == null) return;
            _totalElapsedHours         = state.TotalElapsedHours;
            _ashBlackoutHoursRemaining = state.AshBlackoutHoursRemaining;

            if (_profile != null)
            {
                DaylightHours = _profile.EvaluateDaylightHours(_totalElapsedHours);
            }
            SkyClarity = ComputeSkyClarity();
        }

        // -------------------------------------------------------------------
        // Private helpers
        // -------------------------------------------------------------------

        private float ComputeSkyClarity()
        {
            // Ash blackout overrides weather
            if (_ashBlackoutHoursRemaining > 0f)
            {
                return 0.02f; // near-zero but not absolute (a sliver of diffuse light)
            }

            if (_weatherSystem == null) return ClarityForClear;

            switch (_weatherSystem.Current)
            {
                case WeatherKind.FalloutStorm: return ClarityForFalloutStorm;
                case WeatherKind.Ashfall:      return ClarityForAshfall;
                case WeatherKind.Blizzard:     return ClarityForBlizzard;
                default:                       return ClarityForClear;
            }
        }
    }
}
