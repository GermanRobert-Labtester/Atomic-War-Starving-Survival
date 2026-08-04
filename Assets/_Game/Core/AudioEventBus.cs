using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Event data when weather or muffling state changes.
    /// </summary>
    public struct WeatherAudioStateEvent
    {
        public WeatherKind Weather;
        public bool IsUnderground;
        public bool IsHatchBreached;
        public bool IsMuffled;
        public float LowPassCutoffHz;
    }

    /// <summary>
    /// Event data when heartbeat ambient state changes.
    /// </summary>
    public struct HeartbeatAudioStateEvent
    {
        public bool IsHeartbeatActive;
        public bool CausedByBlackout;
        public bool CausedByAnxiety;
    }

    /// <summary>
    /// Event data when emergency siren state changes.
    /// </summary>
    public struct EmergencySirenAudioEvent
    {
        public bool IsActive;
    }

    /// <summary>
    /// Dynamic Audio System event bus controller.
    /// Subscribes to EventBus and system state changes, driving dynamic audio parameters
    /// (muffled storm soundscapes, blackout/anxiety heartbeats) without polling Update().
    /// </summary>
    public class AudioEventBus
    {
        private WeatherKind _currentWeather = WeatherKind.Clear;
        private bool _isUnderground = true;
        private bool _isHatchBreached = false;
        private bool _isBlackout = false;
        private bool _isEmergencySirenActive = false;
        private readonly HashSet<string> _anxiousSurvivorIds = new HashSet<string>();

        public WeatherKind CurrentWeather => _currentWeather;
        public bool IsUnderground => _isUnderground;
        public bool IsHatchBreached => _isHatchBreached;
        public bool IsBlackout => _isBlackout;
        public bool IsEmergencySirenActive => _isEmergencySirenActive;
        public int AnxiousSurvivorCount => _anxiousSurvivorIds.Count;

        /// <summary>True when FalloutStorm / BlackRain wind is playing.</summary>
        public bool IsWindPlaying =>
            _currentWeather == WeatherKind.FalloutStorm
            || _currentWeather == WeatherKind.BlackRain;

        /// <summary>
        /// Muffling filter is active when storm wind is playing, camera is underground,
        /// and hatch is NOT breached.
        /// </summary>
        public bool IsWindMuffled => IsWindPlaying && _isUnderground && !_isHatchBreached;

        /// <summary>AudioMixer cutoff frequency (Hz) for muffling filter (500Hz when muffled, 22000Hz when open).</summary>
        public float LowPassCutoffHz => IsWindMuffled ? 500f : 22000f;

        /// <summary>Faint rhythmic low-pass heartbeat plays during blackout OR survivor radiation anxiety.</summary>
        public bool IsHeartbeatPlaying => _isBlackout || _anxiousSurvivorIds.Count > 0;

        public event Action<WeatherAudioStateEvent> OnWeatherAudioStateChanged;
        public event Action<HeartbeatAudioStateEvent> OnHeartbeatAudioStateChanged;
        public event Action<EmergencySirenAudioEvent> OnEmergencySirenStateChanged;

        public AudioEventBus()
        {
            // Subscribe to generic EventBus events if raised
            EventBus.Subscribe<WeatherKind>(OnWeatherChanged);
            EventBus.Subscribe<RaidResolution>(OnRaidResolved);
        }

        public void Teardown()
        {
            EventBus.Unsubscribe<WeatherKind>(OnWeatherChanged);
            EventBus.Unsubscribe<RaidResolution>(OnRaidResolved);
        }

        // -----------------------------------------------------------------
        // State updates
        // -----------------------------------------------------------------

        public void SetWeather(WeatherKind weather)
        {
            if (_currentWeather == weather) return;
            _currentWeather = weather;
            NotifyWeatherAudioChanged();
        }

        public void SetUnderground(bool isUnderground)
        {
            if (_isUnderground == isUnderground) return;
            _isUnderground = isUnderground;
            NotifyWeatherAudioChanged();
        }

        public void SetHatchBreached(bool isBreached)
        {
            if (_isHatchBreached == isBreached) return;
            _isHatchBreached = isBreached;
            NotifyWeatherAudioChanged();
        }

        public void SetBlackout(bool isBlackout)
        {
            if (_isBlackout == isBlackout) return;
            _isBlackout = isBlackout;
            NotifyHeartbeatAudioChanged();
        }

        public void SetSurvivorAnxiety(string survivorId, bool hasAnxiety)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            bool changed = hasAnxiety ? _anxiousSurvivorIds.Add(survivorId) : _anxiousSurvivorIds.Remove(survivorId);
            if (changed)
            {
                NotifyHeartbeatAudioChanged();
            }
        }

        /// <summary>
        /// Trigger emergency siren audio event during severe breaches (Prompt #40).
        /// </summary>
        public void TriggerEmergencySiren(bool active = true)
        {
            _isEmergencySirenActive = active;
            var evt = new EmergencySirenAudioEvent { IsActive = active };
            OnEmergencySirenStateChanged?.Invoke(evt);
            EventBus.Raise(evt);
        }

        // -----------------------------------------------------------------
        // EventBus handlers
        // -----------------------------------------------------------------

        private void OnWeatherChanged(WeatherKind kind)
        {
            SetWeather(kind);
        }

        private void OnRaidResolved(RaidResolution resolution)
        {
            if (resolution != null && resolution.Breached)
            {
                SetHatchBreached(true);
            }
        }

        private void NotifyWeatherAudioChanged()
        {
            var evt = new WeatherAudioStateEvent
            {
                Weather = _currentWeather,
                IsUnderground = _isUnderground,
                IsHatchBreached = _isHatchBreached,
                IsMuffled = IsWindMuffled,
                LowPassCutoffHz = LowPassCutoffHz
            };
            OnWeatherAudioStateChanged?.Invoke(evt);
        }

        private void NotifyHeartbeatAudioChanged()
        {
            var evt = new HeartbeatAudioStateEvent
            {
                IsHeartbeatActive = IsHeartbeatPlaying,
                CausedByBlackout = _isBlackout,
                CausedByAnxiety = _anxiousSurvivorIds.Count > 0
            };
            OnHeartbeatAudioStateChanged?.Invoke(evt);
        }
    }
}
