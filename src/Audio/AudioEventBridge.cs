using System;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Narrow host contract used by AudioManager to discover the live Core
    /// systems without owning gameplay state or depending on Main directly.
    /// </summary>
    public interface IAudioDomainProvider
    {
        RadiationSystem? AudioRadiation { get; }
        WeatherSystem? AudioWeather { get; }
    }

    /// <summary>
    /// Subscribes to Core domain events and maps them to stable audio cue IDs.
    /// The bridge owns and releases its subscriptions; rebinding after a new
    /// campaign cannot leave handlers attached to stale host sessions.
    /// </summary>
    public sealed class AudioEventBridge : IDisposable
    {
        private readonly Action<string> _playCue;
        private RadiationSystem? _radiation;
        private WeatherSystem? _weather;
        private bool _disposed;

        public AudioEventBridge(AudioManager audio)
            : this((audio ?? throw new ArgumentNullException(nameof(audio))).PlayCue)
        {
        }

        internal AudioEventBridge(Action<string> playCue)
        {
            _playCue = playCue ?? throw new ArgumentNullException(nameof(playCue));
        }

        /// <summary>
        /// Bind both domains. Safe to call every frame: unchanged references
        /// are no-ops, while replaced sessions are unsubscribed before binding.
        /// </summary>
        public void SubscribeAll(
            RadiationSystem? radiation = null,
            WeatherSystem? weather = null)
        {
            ThrowIfDisposed();
            BindRadiation(radiation);
            BindWeather(weather);
        }

        public void BindRadiation(RadiationSystem? radiation)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_radiation, radiation))
                return;

            if (_radiation != null)
                _radiation.OnStatusGained -= OnRadiationStatusGained;

            _radiation = radiation;
            if (_radiation != null)
                _radiation.OnStatusGained += OnRadiationStatusGained;
        }

        public void BindWeather(WeatherSystem? weather)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_weather, weather))
                return;

            if (_weather != null)
                _weather.OnWeatherChanged -= OnWeatherChanged;

            _weather = weather;
            if (_weather != null)
                _weather.OnWeatherChanged += OnWeatherChanged;
        }

        private void OnRadiationStatusGained(SurvivorRadState state, SurvivorStatus status)
        {
            string? cueId = status switch
            {
                SurvivorStatus.AcuteRadiationSickness => AudioCueCatalog.RadAlertAcute,
                SurvivorStatus.ChronicIllness => AudioCueCatalog.RadAlertChronic,
                _ => null,
            };

            if (cueId != null)
                _playCue(cueId);
        }

        private void OnWeatherChanged(WeatherKind kind)
        {
            string? cueId = kind switch
            {
                WeatherKind.FalloutStorm => AudioCueCatalog.WeatherFalloutStorm,
                WeatherKind.BlackRain => AudioCueCatalog.WeatherBlackRain,
                WeatherKind.Blizzard => AudioCueCatalog.WeatherBlizzard,
                _ => null,
            };

            if (cueId != null)
                _playCue(cueId);
        }

        /// <summary>
        /// Fire a one-shot cue for game-flow events without adding domain logic.
        /// </summary>
        public void NotifyGameFlow(string cueId)
        {
            ThrowIfDisposed();
            _playCue(cueId);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            if (_radiation != null)
                _radiation.OnStatusGained -= OnRadiationStatusGained;
            if (_weather != null)
                _weather.OnWeatherChanged -= OnWeatherChanged;

            _radiation = null;
            _weather = null;
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(AudioEventBridge));
        }

        internal bool HasRadiationBinding => _radiation != null;
        internal bool HasWeatherBinding => _weather != null;
    }
}
