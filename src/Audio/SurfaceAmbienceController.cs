using System;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Selects a normal or storm surface loop only while a host explicitly
    /// enters surface listening mode. Weather is presentation input; this
    /// controller never treats an expedition as proof of player location.
    /// </summary>
    public sealed class SurfaceAmbienceController : IDisposable
    {
        private readonly Action<string> _playCue;
        private readonly Action<string> _stopCue;
        private WeatherSystem? _weather;
        private bool _active;
        private bool _disposed;

        public SurfaceAmbienceController(AudioManager audio)
            : this(
                (audio ?? throw new ArgumentNullException(nameof(audio))).PlayCue,
                audio.StopCue)
        {
        }

        internal SurfaceAmbienceController(Action<string> playCue, Action<string> stopCue)
        {
            _playCue = playCue ?? throw new ArgumentNullException(nameof(playCue));
            _stopCue = stopCue ?? throw new ArgumentNullException(nameof(stopCue));
        }

        public void Subscribe(WeatherSystem? weather)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_weather, weather))
                return;

            if (_weather != null)
                _weather.OnWeatherChanged -= OnWeatherChanged;
            _weather = weather;
            if (_weather != null)
                _weather.OnWeatherChanged += OnWeatherChanged;
            Sync();
        }

        public void Start()
        {
            ThrowIfDisposed();
            _active = true;
            Sync();
        }

        public void Stop()
        {
            if (_disposed) return;
            _active = false;
            _stopCue(AudioCueCatalog.AmbSurface);
            _stopCue(AudioCueCatalog.AmbSurfaceStorm);
        }

        private void OnWeatherChanged(WeatherKind kind) => Sync();

        private void Sync()
        {
            if (!_active)
            {
                _stopCue(AudioCueCatalog.AmbSurface);
                _stopCue(AudioCueCatalog.AmbSurfaceStorm);
                return;
            }

            bool storm = IsStorm(_weather?.Current ?? WeatherKind.Clear);
            string desired = storm ? AudioCueCatalog.AmbSurfaceStorm : AudioCueCatalog.AmbSurface;
            string other = storm ? AudioCueCatalog.AmbSurface : AudioCueCatalog.AmbSurfaceStorm;
            _stopCue(other);
            _playCue(desired);
        }

        private static bool IsStorm(WeatherKind kind) => kind switch
        {
            WeatherKind.Ashfall or WeatherKind.FalloutStorm or WeatherKind.Blizzard
                or WeatherKind.BlackRain or WeatherKind.AcidSnow or WeatherKind.BlackSnow
                or WeatherKind.BloodRain or WeatherKind.EMPStorm or WeatherKind.GlassStorm
                or WeatherKind.RadHail or WeatherKind.AshLightning or WeatherKind.IceStorm => true,
            _ => false,
        };

        public void Dispose()
        {
            if (_disposed) return;
            Stop();
            _disposed = true;
            if (_weather != null)
                _weather.OnWeatherChanged -= OnWeatherChanged;
            _weather = null;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SurfaceAmbienceController));
        }
    }
}
