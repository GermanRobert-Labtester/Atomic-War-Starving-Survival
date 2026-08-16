using System;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.World;
using Godot;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Subscribes to Core domain events and maps them to audio cue IDs.
    /// Thin bridge — no simulation logic, no audio playback directly.
    /// Delegates to AudioManager.PlayCue() for all playback.
    /// </summary>
    public sealed class AudioEventBridge
    {
        private readonly AudioManager _audio;
        private bool _subscribed;

        public AudioEventBridge(AudioManager audio)
        {
            _audio = audio ?? throw new ArgumentNullException(nameof(audio));
        }

        /// <summary>
        /// Subscribe to all relevant domain events.
        /// Safe to call multiple times — guards against duplicate subscriptions.
        /// </summary>
        public void SubscribeAll(
            RadiationSystem? radiation = null,
            WeatherSystem? weather = null)
        {
            if (_subscribed) return;
            _subscribed = true;

            if (radiation != null)
                SubscribeRadiation(radiation);

            if (weather != null)
                SubscribeWeather(weather);
        }

        private void SubscribeRadiation(RadiationSystem radiation)
        {
            radiation.OnStatusGained += (state, status) =>
            {
                switch (status)
                {
                    case SurvivorStatus.AcuteRadiationSickness:
                        _audio.PlayCue(AudioCueCatalog.RadAlertAcute);
                        break;
                    case SurvivorStatus.ChronicIllness:
                        _audio.PlayCue(AudioCueCatalog.RadAlertChronic);
                        break;
                }
            };
        }

        private void SubscribeWeather(WeatherSystem weather)
        {
            weather.OnWeatherChanged += kind =>
            {
                string? cueId = kind switch
                {
                    WeatherKind.FalloutStorm => AudioCueCatalog.WeatherFalloutStorm,
                    WeatherKind.BlackRain => AudioCueCatalog.WeatherBlackRain,
                    WeatherKind.Blizzard => AudioCueCatalog.WeatherBlizzard,
                    _ => null
                };
                if (cueId != null)
                    _audio.PlayCue(cueId);
            };
        }

        /// <summary>
        /// Fire a one-shot cue for game flow events.
        /// Called directly by Main.cs or host sessions for non-domain events.
        /// </summary>
        public void NotifyGameFlow(string cueId)
        {
            _audio.PlayCue(cueId);
        }
    }
}
