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

        private string? _currentLocationId;

        public void SetLocation(string? locationId)
        {
            if (_currentLocationId == locationId) return;
            _currentLocationId = locationId;
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
            StopAllAmbiences();
        }

        private void OnWeatherChanged(WeatherKind kind) => Sync();

        private void Sync()
        {
            if (!_active)
            {
                StopAllAmbiences();
                return;
            }

            bool storm = IsStorm(_weather?.Current ?? WeatherKind.Clear);
            string desired = storm ? AudioCueCatalog.AmbSurfaceStorm : ResolveLocationAmbience(_currentLocationId);
            StopOtherAmbiences(desired);
            _playCue(desired);
        }

        public static string ResolveLocationAmbience(string? locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return AudioCueCatalog.AmbSurface;
            string lower = locationId.ToLowerInvariant();
            if (lower.Contains("hospital") || lower.Contains("clinic") || lower.Contains("pharmacy"))
                return AudioCueCatalog.AmbLocAbandonedHospital;
            if (lower.Contains("gas") || lower.Contains("station") || lower.Contains("roadside"))
                return AudioCueCatalog.AmbLocRuralGasStation;
            if (lower.Contains("suburban") || lower.Contains("house") || lower.Contains("residential") || lower.Contains("neighborhood"))
                return AudioCueCatalog.AmbLocSuburbanRuins;
            if (lower.Contains("bunker") || lower.Contains("military") || lower.Contains("depot") || lower.Contains("silo") || lower.Contains("outpost"))
                return AudioCueCatalog.AmbLocMilitaryBunker;
            if (lower.Contains("geo") || lower.Contains("thermal") || lower.Contains("volcan") || lower.Contains("plant"))
                return AudioCueCatalog.AmbLocGeothermalRuins;
            if (lower.Contains("arcology") || lower.Contains("sector") || lower.Contains("tower") || lower.Contains("facility"))
                return AudioCueCatalog.AmbLocArcologySector;
            if (lower.Contains("warzone") || lower.Contains("front") || lower.Contains("battle") || lower.Contains("trench") || lower.Contains("crossing"))
                return AudioCueCatalog.AmbWarzoneDistantShelling;
            return AudioCueCatalog.AmbSurface;
        }

        private static readonly string[] s_surfaceAmbiences = new[]
        {
            AudioCueCatalog.AmbSurface,
            AudioCueCatalog.AmbSurfaceStorm,
            AudioCueCatalog.AmbLocAbandonedHospital,
            AudioCueCatalog.AmbLocRuralGasStation,
            AudioCueCatalog.AmbLocSuburbanRuins,
            AudioCueCatalog.AmbLocMilitaryBunker,
            AudioCueCatalog.AmbLocGeothermalRuins,
            AudioCueCatalog.AmbLocArcologySector,
            AudioCueCatalog.AmbWarzoneDistantShelling
        };

        private void StopAllAmbiences()
        {
            foreach (var cue in s_surfaceAmbiences)
                _stopCue(cue);
        }

        private void StopOtherAmbiences(string except)
        {
            foreach (var cue in s_surfaceAmbiences)
            {
                if (cue != except)
                    _stopCue(cue);
            }
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
