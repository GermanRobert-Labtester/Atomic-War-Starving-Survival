// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 52 / Scarcity audio state machine — host presentation adapter.
//
// Authority boundary (deliberate): ScarcityAudioStateMachine is the single
// authority for which ambience bed, cue and mix state the shelter is in. This
// adapter binds it to the canonical owners and applies its verdict to the
// audio manager:
//   weather kind     -> WeatherSystem.OnWeatherChanged (the weather owner)
//   generator power  -> PowerGridSystem (breakers/shed state)
//   occupant count   -> SurvivorsHostSession living roster
//   exposure rate    -> the radiation owner's per-survivor dose rate (max)
//   surface/bunker   -> the existing explicit surface listening mode
//
// It replaces the SurfaceAmbienceController's private weather→cue table with
// the authority's profile (mapped onto the cues that actually exist in
// AudioCueCatalog — no invented cue ids), and routes the geiger loop and alert
// ducking through the authority's own seams.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Audio;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp.Audio
{
    public sealed class ScarcityAudioController : IDisposable
    {
        private readonly AudioManager _audio;
        private WeatherSystem? _weather;
        private PowerGridSystem? _powerGrid;
        private SurvivorsHostSession? _survivors;
        private bool _disposed;
        private bool _surfaceListening;

        public ScarcityAudioStateMachine State { get; } = new ScarcityAudioStateMachine();

        public ScarcityAudioController(AudioManager audio)
        {
            _audio = audio ?? throw new ArgumentNullException(nameof(audio));
            State.OnAmbienceBedTransitionSeam += OnAmbienceBedTransition;
            State.OnGeigerLoopStateChangedSeam += OnGeigerLoopStateChanged;
        }

        // ── Canonical owner bindings ──────────────────────────────────────

        public void Subscribe(WeatherSystem? weather)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_weather, weather)) return;
            if (_weather != null) _weather.OnWeatherChanged -= OnWeatherChanged;
            _weather = weather;
            if (_weather != null) _weather.OnWeatherChanged += OnWeatherChanged;
            RefreshContext();
        }

        public void BindPowerGrid(PowerGridSystem? powerGrid)
        {
            _powerGrid = powerGrid;
            RefreshContext();
        }

        public void BindRoster(SurvivorsHostSession? survivors)
        {
            _survivors = survivors;
            RefreshContext();
        }

        /// <summary>
        /// Enter/leave the host's explicit surface listening mode. The
        /// controller never infers the player's location from an expedition.
        /// </summary>
        public void SetSurfaceListening(bool onSurface)
        {
            _surfaceListening = onSurface;
            RefreshContext();
        }

        public void TriggerAlert(string alertCueName) => State.TriggerAlert(alertCueName);

        public void ReleaseAlert() => State.ReleaseAlert();

        // ── Context refresh ───────────────────────────────────────────────

        private void OnWeatherChanged(WeatherKind kind) => RefreshContext();

        /// <summary>
        /// Push the current canonical facts into the authority. Dose rate is the
        /// maximum live survivor rate from the radiation owner; the authority
        /// decides the band and the geiger loop.
        /// </summary>
        public void RefreshContext()
        {
            ThrowIfDisposed();

            int occupants = 0;
            float doseRate = 0f;
            if (_survivors?.Radiation != null)
            {
                foreach (var survivor in _survivors.Roster.Roster)
                {
                    if (survivor == null || string.IsNullOrWhiteSpace(survivor.survivorId) || !survivor.isAlive) continue;
                    occupants++;
                    // The dosimeter owner holds the live rate (dose/hours); the
                    // authority maps it to the geiger band.
                    var dosimeter = _survivors.Radiation.GetDosimeter(survivor.survivorId);
                    if (dosimeter == null) continue;
                    float rate = Math.Max(0f, dosimeter.CurrentReading);
                    if (rate > doseRate) doseRate = rate;
                }
            }
            if (occupants == 0) occupants = _survivors?.Roster?.LivingCount ?? 0;

            State.UpdateContext(
                onSurface: _surfaceListening,
                weatherKind: WeatherKindKey(_weather?.Current ?? WeatherKind.Clear),
                isGeneratorPowered: IsGeneratorPowered(),
                occupantCount: Math.Max(0, occupants));

            if (doseRate > 0f) State.SetRadiationExposure(doseRate);
            else State.EndRadiationExposure();
        }

        private bool IsGeneratorPowered()
        {
            if (_powerGrid == null) return true;
            if (_powerGrid.TotalDrawWatts <= 0f) return true;
            return !_powerGrid.IsBrownout;
        }

        /// <summary>The authority's canonical snake_case weather key.</summary>
        public static string WeatherKindKey(WeatherKind kind) => kind switch
        {
            WeatherKind.Clear => "clear",
            WeatherKind.Rain => "acid_rain",
            WeatherKind.Overcast => "overcast",
            WeatherKind.Ashfall => "ash_fall",
            WeatherKind.FalloutStorm => "ash_storm",
            WeatherKind.Blizzard => "ice_storm",
            WeatherKind.BlackRain => "acid_rain",
            WeatherKind.AcidSnow => "acid_snow",
            WeatherKind.BioFog => "bio_fog",
            WeatherKind.BlackSnow => "black_snow",
            WeatherKind.BloodRain => "blood_rain",
            WeatherKind.EMPStorm => "emp_storm",
            WeatherKind.GlassStorm => "glass_storm",
            WeatherKind.RadHail => "rad_hail",
            WeatherKind.AlgaeBloom => "algae_bloom",
            WeatherKind.AshLightning => "ash_lightning",
            WeatherKind.ParticulateFog => "particulate_fog",
            WeatherKind.ThermalInversion => "thermal_inversion",
            WeatherKind.IceStorm => "ice_storm",
            WeatherKind.Silence => "silence",
            WeatherKind.FalseSpring => "false_spring",
            WeatherKind.SilentSpring => "silent_spring",
            _ => "clear"
        };

        // ── Authority verdict -> audio manager ────────────────────────────

        private void OnAmbienceBedTransition(ScarcityAmbienceBed from, ScarcityAmbienceBed to)
            => ApplyAmbience();

        /// <summary>
        /// Apply the authority's current bed. Weather cues map onto the cue ids
        /// that exist in AudioCueCatalog; an unmapped kind keeps the neutral
        /// surface loop, and AbsoluteSilence stops every ambience loop.
        /// </summary>
        public void ApplyAmbience()
        {
            ThrowIfDisposed();
            if (_audio == null) return;

            if (State.CurrentBed == ScarcityAmbienceBed.AbsoluteSilence)
            {
                _audio.StopAmbience();
                return;
            }

            if (State.CurrentBed == ScarcityAmbienceBed.BunkerAmbience)
            {
                _audio.StopAmbience();
                _audio.SetBunkerOcclusion(true);
                return;
            }

            _audio.SetBunkerOcclusion(false);
            string cue = AmbienceCueForWeather(State.CurrentWeatherKind);
            if (!string.IsNullOrEmpty(cue)) _audio.PlayCue(cue);
        }

        /// <summary>
        /// Map the authority's weather key onto an existing catalog cue. No cue
        /// id is invented here: unknown keys fall back to the neutral surface
        /// loop, which always exists.
        /// </summary>
        public static string AmbienceCueForWeather(string weatherKey)
        {
            switch (weatherKey)
            {
                case "ash_fall":
                case "ash_lightning":
                    return AudioCueCatalog.AmbSurfaceAshfall;
                case "ash_storm":
                case "emp_storm":
                case "glass_storm":
                case "rad_hail":
                case "ice_storm":
                case "blood_rain":
                    return AudioCueCatalog.AmbSurfaceStorm;
                case "fallout_storm":
                    return AudioCueCatalog.AmbSurfaceFalloutStorm;
                case "acid_snow":
                case "black_snow":
                case "nuclear_winter":
                    return AudioCueCatalog.AmbSurfaceBlizzard;
                default:
                    return AudioCueCatalog.AmbSurface;
            }
        }

        private void OnGeigerLoopStateChanged(bool active, GeigerRateBand band)
        {
            ThrowIfDisposed();
            if (_audio == null) return;

            if (!active)
            {
                _audio.StopCue(AudioCueCatalog.RadGeigerLoop);
                _audio.StopCue(AudioCueCatalog.RadGeigerIntense);
                return;
            }

            _audio.PlayCue(AudioCueCatalog.RadGeigerLoop);
            if (band >= GeigerRateBand.High)
            {
                _audio.PlayCue(AudioCueCatalog.RadGeigerIntense);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            State.OnAmbienceBedTransitionSeam -= OnAmbienceBedTransition;
            State.OnGeigerLoopStateChangedSeam -= OnGeigerLoopStateChanged;
            if (_weather != null)
            {
                _weather.OnWeatherChanged -= OnWeatherChanged;
                _weather = null;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ScarcityAudioController));
        }
    }
}
