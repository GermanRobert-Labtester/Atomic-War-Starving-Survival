using System;

namespace AtomicWar._Game.Flashpoint
{
    // -------------------------------------------------------------------
    // Typed EventBus payloads for the Day-30 Flashpoint Choreography.
    // The FlashpointChoreographer is the only publisher; UI / audio / VFX /
    // camera-shake handlers subscribe to the EventBus. Each event is
    // raised exactly once per choreography run; consumers must be
    // idempotent across save/load.
    // -------------------------------------------------------------------

    /// <summary>
    /// Fired the first time the player enters a buildup day (25-29). The
    /// audio layer uses <see cref="AudioCueId"/> to swap the ambient mix;
    /// economy uses <see cref="EconomyModifierId"/> to know which demand
    /// spike to apply; the world flag is set so save/load can skip the
    /// re-entry side effects.
    /// </summary>
    public readonly struct FlashpointBuildupDayEntered
    {
        public readonly int Day;
        public readonly string AudioCueId;
        public readonly string EconomyModifierId;
        public readonly string WorldFlagKey;

        public FlashpointBuildupDayEntered(int day, string audioCueId, string economyModifierId, string worldFlagKey)
        {
            Day = day;
            AudioCueId = audioCueId;
            EconomyModifierId = economyModifierId;
            WorldFlagKey = worldFlagKey;
        }
    }

    /// <summary>Fired once when the choreography coroutine starts. UI fades to white.</summary>
    public readonly struct FlashpointChoreographyStarted
    {
        public readonly string SequenceId;
        public FlashpointChoreographyStarted(string sequenceId) { SequenceId = sequenceId; }
    }

    /// <summary>
    /// The white-flash step begins. <see cref="DurationSeconds"/> is the
    /// on-screen white time; <see cref="IsAccessibilitySafe"/> is true when
    /// the player has enabled the photosensitivity-safe option (flash is
    /// shorter, desaturated, and camera shake is reduced).
    /// </summary>
    public readonly struct FlashpointFlashStarted
    {
        public readonly float DurationSeconds;
        public readonly bool IsAccessibilitySafe;
        public FlashpointFlashStarted(float durationSeconds, bool isAccessibilitySafe)
        {
            DurationSeconds = durationSeconds;
            IsAccessibilitySafe = isAccessibilitySafe;
        }
    }

    /// <summary>
    /// Fired the instant the white flash ends. The mechanical EMP fires
    /// here: devices break, modules disable, the radio is destroyed, every
    /// survivor takes a permanent morale hit, radiation unpauses, and the
    /// weather is forced to Ashfall. The audio layer plays the click/fizz
    /// of unshielded electronics dying.
    /// </summary>
    public readonly struct FlashpointEmptiedDevices
    {
        public readonly int DevicesBroken;
        public readonly int ModulesDisabled;
        public readonly bool RadioDestroyed;
        public readonly float MoraleHitApplied;

        public FlashpointEmptiedDevices(int devicesBroken, int modulesDisabled, bool radioDestroyed, float moraleHitApplied)
        {
            DevicesBroken = devicesBroken;
            ModulesDisabled = modulesDisabled;
            RadioDestroyed = radioDestroyed;
            MoraleHitApplied = moraleHitApplied;
        }
    }

    /// <summary>
    /// The sub-bass shockwave arrives. The audio layer plays a low-pass
    /// rumble (35-55 Hz, 6-8s tail) and the camera-shake handler starts
    /// a 0.08-0.12 amplitude oscillation. Dust falls from the bunker
    /// ceiling (VFX handler). <see cref="Intensity"/> is 0..1.
    /// </summary>
    public readonly struct FlashpointShockwaveHit
    {
        public readonly float Intensity;
        public readonly float DurationSeconds;
        public FlashpointShockwaveHit(float intensity, float durationSeconds)
        {
            Intensity = intensity;
            DurationSeconds = durationSeconds;
        }
    }

    /// <summary>
    /// Hand-cranked / unshielded military sirens spool up after the blast,
    /// muffled by the bunker walls. The audio layer plays a low-pass-filtered
    /// siren loop at low volume.
    /// </summary>
    public readonly struct FlashpointSirensSpooling
    {
        public readonly bool Muffled;
        public FlashpointSirensSpooling(bool muffled) { Muffled = muffled; }
    }

    /// <summary>
    /// The screen takes on the desaturated cold palette of the
    /// SeasonProfile. UI/VFX swap color grading. Particle systems begin
    /// emitting the first flakes of nuclear ash outside the hatch.
    /// </summary>
    public readonly struct FlashpointWeatherShifted
    {
        public readonly string WeatherKindId;
        public FlashpointWeatherShifted(string weatherKindId) { WeatherKindId = weatherKindId; }
    }

    /// <summary>
    /// The radiation UI permanently unlocks: dosimeter and geiger audio
    /// hook become visible on the HUD. Data was already accumulating
    /// before the flash; this is the UI visibility gate.
    /// </summary>
    public readonly struct FlashpointRadiationHudUnlocked
    {
        public static readonly FlashpointRadiationHudUnlocked Instance = new FlashpointRadiationHudUnlocked();
    }

    /// <summary>Fired once when the choreography finishes all its steps.</summary>
    public readonly struct FlashpointChoreographyCompleted
    {
        public static readonly FlashpointChoreographyCompleted Instance = new FlashpointChoreographyCompleted();
    }

    /// <summary>
    /// Prompts #319–#325 — Section X new weather events.
    /// Raised when a Flashpoint choreography step with actionId
    /// <c>"weather_event_trigger"</c> fires. The host (a weather-event
    /// bridge wired in <c>GameBootstrap.Weather.NewContent.cs</c>) maps
    /// <see cref="WeatherEventId"/> to the right
    /// <c>Weather_&lt;Name&gt;.Trigger()</c> call. Use the canonical
    /// snake_case ids from each system's <c>State.weatherId</c>:
    /// <c>weather_ash_lightning</c>, <c>weather_fog_of_particulate</c>,
    /// <c>weather_thermal_inversion</c>, <c>weather_ice_storm</c>,
    /// <c>weather_silence</c>.
    /// </summary>
    public readonly struct FlashpointWeatherEventTriggered
    {
        public readonly string WeatherEventId;
        public FlashpointWeatherEventTriggered(string weatherEventId) { WeatherEventId = weatherEventId; }
    }
}
