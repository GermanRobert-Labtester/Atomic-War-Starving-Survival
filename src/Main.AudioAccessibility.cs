// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 169 — Audio Accessibility & Mix Legibility host wiring.
// Binds the Core AudioAccessibilityCoordinator to the live AudioManager and to
// the user accessibility preference, and translates canonical day facts into
// critical-cue visual equivalents.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Audio;
using Ashfall.Core.Campaign;
using AtomicWar.GodotApp.Settings;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AudioAccessibilityHostSession? _audioAccessibility;
        private bool _audioAccessibilityWired;

        public AudioAccessibilityHostSession? AudioAccessibility => _audioAccessibility;

        public void SetupAudioAccessibility()
        {
            if (_audioAccessibility != null) return;

            _audioAccessibility = AudioAccessibilityHostSession.Create(_dataDir);

            _audioAccessibility.DuckingSink = db => _audio?.ApplyAccessibilityDucking(db);
            _audioAccessibility.MixPresetSink = presetId => _audio?.ApplyAccessibilityMixPreset(presetId);
            _audioAccessibility.VisualNotificationSink = EmitAudioAccessibilityNotification;
            _audioAccessibility.BindVisualAlertsEnabled(
                () => UserSettingsStore.Current.VisualAudioAlerts);

            // Apply the persisted acoustic preset as soon as the mixer exists.
            _audio?.ApplyAccessibilityMixPreset(UserSettingsStore.Current.AudioMixPreset);

            WireAudioAccessibilityDayBridge();
        }

        private void WireAudioAccessibilityDayBridge()
        {
            if (_audioAccessibilityWired || _campaignDay == null) return;
            _audioAccessibilityWired = true;
            _campaignDay.OnDayAdvanced += OnCampaignDayAdvancedAudioAccessibility;
        }

        /// <summary>
        /// Plan 169 — translates canonical day facts into critical audio cues.
        /// This never makes a gameplay decision: it only reflects facts other
        /// owners already emitted, and the coordinator owns coalescing/ducking.
        /// </summary>
        private void OnCampaignDayAdvancedAudioAccessibility(DayAdvancedEventArgs args)
        {
            if (_audioAccessibility == null || args?.OwnerReports == null) return;

            // Release yesterday's duck before evaluating today's cues so the
            // duck reflects the current day rather than accumulating.
            _audioAccessibility.ReleaseDucking();

            for (int r = 0; r < args.OwnerReports.Length; r++)
            {
                var report = args.OwnerReports[r];
                if (report?.Events == null) continue;

                for (int e = 0; e < report.Events.Length; e++)
                {
                    string? cueId = MapDayEventToAudioCue(report.Events[e]?.Kind);
                    if (cueId != null)
                        _audioAccessibility.TriggerCue(cueId, args.Day, out _);
                }
            }
        }

        private static string? MapDayEventToAudioCue(string? kind) => kind switch
        {
            "power_critical_deficit" => "cue_low_power_warning",
            "power_brownout_began" => "cue_low_power_warning",
            "medical_admitted" => "cue_medical_emergency",
            "radio_intercept" => "cue_radio_incoming_signal",
            "radio_intercept_decrypted" => "cue_radio_incoming_signal",
            "expedition_milestone" => "cue_expedition_returned",
            "raid_incoming" => "cue_raid_incoming",
            "attack_incoming" => "cue_raid_incoming",
            _ => null
        };

        private void EmitAudioAccessibilityNotification(VisualAudioNotification notification)
        {
            if (notification == null) return;
            GD.Print($"[AudioAccessibility] {notification.Severity}: {notification.VisualLabel} [{notification.IconGlyph}]");
        }

        /// <summary>
        /// Plan 169 — applies a user-selected acoustic preset and persists it in
        /// the sole settings authority. Called by the accessibility UI.
        /// </summary>
        public bool SetAudioMixPreset(string presetId)
        {
            if (string.IsNullOrWhiteSpace(presetId)) return false;
            SetupAudioAccessibility();

            if (_audioAccessibility == null || !_audioAccessibility.ApplyPreset(presetId.Trim()))
                return false;

            var settings = UserSettingsStore.Current;
            settings.AudioMixPreset = presetId.Trim();
            UserSettingsStore.Save(settings);
            return true;
        }

        /// <summary>Plan 169 — manually fires a critical cue (fired by UI/quest hooks).</summary>
        public bool TriggerAudioAccessibilityCue(string cueId)
        {
            if (string.IsNullOrWhiteSpace(cueId)) return false;
            SetupAudioAccessibility();
            return _audioAccessibility != null
                && _audioAccessibility.TriggerCue(cueId.Trim(), _simDay, out _);
        }

        public void ResetAudioAccessibility()
        {
            if (_audioAccessibilityWired && _campaignDay != null)
                _campaignDay.OnDayAdvanced -= OnCampaignDayAdvancedAudioAccessibility;
            _audioAccessibilityWired = false;
            _audioAccessibility = null;
        }
    }
}
