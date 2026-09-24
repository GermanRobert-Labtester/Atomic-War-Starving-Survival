// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 169 — Audio Accessibility & Mix Legibility host session.
// Wraps the Core AudioAccessibilityCoordinator and connects its seams to the
// engine through delegate sinks. The Core coordinator owns cue mapping, ducking
// computation, alert coalescing, and preset resolution; this session only binds
// them to the mixer and to the accessibility preference, and offers a read-only
// census. It creates no second audio or settings authority.
// ============================================================================

using System;
using System.IO;
using Godot;
using Ashfall.Core.Audio;

namespace AtomicWar.GodotApp
{
    public sealed class AudioAccessibilityHostSession : HostSessionBase
    {
        private readonly AudioAccessibilityCoordinator _coordinator;
        private Func<bool> _visualAlertsEnabled = static () => true;
        private string _lastEvent = string.Empty;

        public AudioAccessibilityCoordinator Coordinator => _coordinator;
        public AudioAccessibilityCensus Census => _coordinator.GetCensus();
        public string LastEvent => _lastEvent;

        /// <summary>Engine sink for the computed side-chain duck attenuation (dB).</summary>
        public Action<float>? DuckingSink { get; set; }

        /// <summary>Engine sink for the selected mix preset id.</summary>
        public Action<string>? MixPresetSink { get; set; }

        /// <summary>Presentation sink for an emitted visual audio notification.</summary>
        public Action<VisualAudioNotification>? VisualNotificationSink { get; set; }

        public AudioAccessibilityHostSession(AudioAccessibilityCoordinator? coordinator = null)
        {
            _coordinator = coordinator ?? new AudioAccessibilityCoordinator();

            _coordinator.OnDuckingChangedSeam = db =>
            {
                DuckingSink?.Invoke(db);
                RaiseStateChanged();
            };

            _coordinator.OnMixPresetChangedSeam = presetId =>
            {
                _lastEvent = $"Applied mix preset {presetId}";
                MixPresetSink?.Invoke(presetId);
                RaiseStateChanged();
            };

            _coordinator.OnVisualNotificationEmittedSeam = notification =>
            {
                // The preference gates the visual layer only; the audio cue still
                // plays and still ducks. No gameplay decision lives here.
                if (_visualAlertsEnabled())
                    VisualNotificationSink?.Invoke(notification);
            };
        }

        public static AudioAccessibilityHostSession Create(
            string dataDir, AudioAccessibilityCoordinator? coordinator = null)
        {
            var session = new AudioAccessibilityHostSession(coordinator);
            if (!string.IsNullOrEmpty(dataDir))
                session.LoadCatalog(dataDir);
            return session;
        }

        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            string path = Path.Combine(dataDir, "audio_accessibility_cues.json");
            if (!File.Exists(path)) return;

            var catalog = AudioAccessibilityCatalogLoader.LoadFromJson(File.ReadAllText(path));
            _coordinator.BindCatalog(catalog);
            _lastEvent = $"Loaded {catalog.Cues.Count} accessibility cues and {catalog.MixPresets.Count} mix presets.";
            RaiseStateChanged();
        }

        /// <summary>Binds the visual-alert preference gate (defaults to enabled).</summary>
        public void BindVisualAlertsEnabled(Func<bool>? gate)
        {
            _visualAlertsEnabled = gate ?? (static () => true);
        }

        public bool TriggerCue(string cueId, double currentTimestampSeconds, out VisualAudioNotification? notification)
        {
            return _coordinator.TriggerCue(cueId, currentTimestampSeconds, out notification);
        }

        public bool ApplyPreset(string presetId) => _coordinator.ApplyPreset(presetId);

        public void ReleaseDucking() => _coordinator.ReleaseDucking();

        public AudioDiagnosticReadout GetDiagnosticReadout() => _coordinator.GetDiagnosticReadout();
    }
}
