// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Audio
{
    [Serializable]
    public sealed class AudioCueAccessibilityDef
    {
        [JsonPropertyName("cue_id")]
        public string CueId { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("bus_name")]
        public string BusName { get; set; } = string.Empty;

        [JsonPropertyName("visual_label")]
        public string VisualLabel { get; set; } = string.Empty;

        [JsonPropertyName("severity")]
        public string Severity { get; set; } = "Info";

        [JsonPropertyName("duck_level_db")]
        public float DuckLevelDb { get; set; } = -6.0f;

        [JsonPropertyName("coalesce_window_seconds")]
        public float CoalesceWindowSeconds { get; set; } = 2.0f;

        [JsonPropertyName("icon_glyph")]
        public string IconGlyph { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class AudioMixPresetDef
    {
        [JsonPropertyName("preset_id")]
        public string PresetId { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("master_limiter_threshold_db")]
        public float MasterLimiterThresholdDb { get; set; } = 0.0f;

        [JsonPropertyName("dialogue_ducking_attenuation_db")]
        public float DialogueDuckingAttenuationDb { get; set; } = -8.0f;

        [JsonPropertyName("high_frequency_attenuation_db")]
        public float HighFrequencyAttenuationDb { get; set; } = 0.0f;
    }

    [Serializable]
    public sealed class AudioAccessibilityCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("cues")]
        public List<AudioCueAccessibilityDef> Cues { get; set; } = new List<AudioCueAccessibilityDef>();

        [JsonPropertyName("mix_presets")]
        public List<AudioMixPresetDef> MixPresets { get; set; } = new List<AudioMixPresetDef>();
    }

    [Serializable]
    public sealed class VisualAudioNotification
    {
        public string CueId { get; set; } = string.Empty;
        public string VisualLabel { get; set; } = string.Empty;
        public string Severity { get; set; } = "Info";
        public string IconGlyph { get; set; } = string.Empty;
        public double TimestampSeconds { get; set; }
    }

    public sealed class AudioDiagnosticReadout
    {
        public IReadOnlyList<string> ActiveBuses { get; set; } = Array.Empty<string>();
        public float ActiveDuckingDb { get; set; }
        public string CurrentPresetId { get; set; } = string.Empty;
        public VisualAudioNotification? LastNotification { get; set; }
        public int CoalescedAlertCount { get; set; }
    }

    /// <summary>
    /// Plan 169 / DEC-161: Audio Accessibility & Mix Legibility Coordinator.
    /// Maps critical gameplay audio cues to concise visual equivalents, enforces
    /// side-chain ducking across audio buses, coalesces repeated rapid alerts,
    /// and manages dynamic-range / reduced-stimulation acoustic presets.
    /// </summary>
    public sealed class AudioAccessibilityCoordinator
    {
        private readonly Dictionary<string, AudioCueAccessibilityDef> _cues = new Dictionary<string, AudioCueAccessibilityDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, AudioMixPresetDef> _presets = new Dictionary<string, AudioMixPresetDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, double> _lastTriggerTime = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

        private float _activeDuckingDb;
        private string _activePresetId = "preset_full_dynamic";
        private VisualAudioNotification? _lastNotification;
        private int _coalescedAlertCount;

        public IReadOnlyDictionary<string, AudioCueAccessibilityDef> Cues => _cues;
        public IReadOnlyDictionary<string, AudioMixPresetDef> Presets => _presets;
        public float ActiveDuckingDb => _activeDuckingDb;
        public string ActivePresetId => _activePresetId;
        public VisualAudioNotification? LastNotification => _lastNotification;
        public int CoalescedAlertCount => _coalescedAlertCount;

        public Action<VisualAudioNotification>? OnVisualNotificationEmittedSeam { get; set; }
        public Action<float>? OnDuckingChangedSeam { get; set; }
        public Action<string>? OnMixPresetChangedSeam { get; set; }

        public AudioAccessibilityCoordinator()
        {
            LoadFallbackCatalog();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<AudioAccessibilityCatalogData>(json, options);
                if (data != null)
                {
                    if (data.Cues != null && data.Cues.Count > 0)
                    {
                        _cues.Clear();
                        foreach (var cue in data.Cues)
                        {
                            if (!string.IsNullOrEmpty(cue.CueId))
                                _cues[cue.CueId] = cue;
                        }
                    }

                    if (data.MixPresets != null && data.MixPresets.Count > 0)
                    {
                        _presets.Clear();
                        foreach (var p in data.MixPresets)
                        {
                            if (!string.IsNullOrEmpty(p.PresetId))
                                _presets[p.PresetId] = p;
                        }
                    }
                }
            }
            catch (JsonException)
            {
                // Catalog parse failure; fallback retained
            }
        }

        public void BindCatalog(AudioAccessibilityCatalogData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            if (data.Cues != null && data.Cues.Count > 0)
            {
                _cues.Clear();
                foreach (var cue in data.Cues)
                {
                    if (!string.IsNullOrEmpty(cue.CueId))
                        _cues[cue.CueId] = cue;
                }
            }

            if (data.MixPresets != null && data.MixPresets.Count > 0)
            {
                _presets.Clear();
                foreach (var preset in data.MixPresets)
                {
                    if (!string.IsNullOrEmpty(preset.PresetId))
                        _presets[preset.PresetId] = preset;
                }
            }
        }

        public AudioAccessibilityCensus GetCensus()
        {
            return new AudioAccessibilityCensus(
                _cues.Count,
                _presets.Count,
                _activePresetId,
                _activeDuckingDb,
                _coalescedAlertCount,
                _lastNotification != null);
        }

        public bool TriggerCue(string cueId, double currentTimestampSeconds, out VisualAudioNotification? notification)
        {
            notification = null;
            if (string.IsNullOrEmpty(cueId))
                return false;

            if (!_cues.TryGetValue(cueId, out var def))
            {
                // Fallback def for unregistered cue
                def = new AudioCueAccessibilityDef
                {
                    CueId = cueId,
                    Category = "General",
                    BusName = "Sfx",
                    VisualLabel = $"[Audio] {cueId}",
                    Severity = "Info",
                    DuckLevelDb = -6.0f,
                    CoalesceWindowSeconds = 2.0f
                };
            }

            // Check alert cooldown / coalescing window
            if (_lastTriggerTime.TryGetValue(cueId, out double lastTime))
            {
                if (currentTimestampSeconds - lastTime < def.CoalesceWindowSeconds)
                {
                    _coalescedAlertCount++;
                    return false; // Suppressed / coalesced
                }
            }

            _lastTriggerTime[cueId] = currentTimestampSeconds;

            notification = new VisualAudioNotification
            {
                CueId = cueId,
                VisualLabel = def.VisualLabel,
                Severity = def.Severity,
                IconGlyph = def.IconGlyph,
                TimestampSeconds = currentTimestampSeconds
            };

            _lastNotification = notification;
            OnVisualNotificationEmittedSeam?.Invoke(notification);

            // Apply sidechain ducking attenuation (takes deepest attenuation if multiple active)
            float newDuck = def.DuckLevelDb;
            if (newDuck < _activeDuckingDb)
            {
                _activeDuckingDb = newDuck;
                OnDuckingChangedSeam?.Invoke(_activeDuckingDb);
            }

            return true;
        }

        public void ReleaseDucking()
        {
            if (_activeDuckingDb != 0.0f)
            {
                _activeDuckingDb = 0.0f;
                OnDuckingChangedSeam?.Invoke(0.0f);
            }
        }

        public bool ApplyPreset(string presetId)
        {
            if (!_presets.TryGetValue(presetId, out var preset))
                return false;

            _activePresetId = presetId;
            OnMixPresetChangedSeam?.Invoke(presetId);
            return true;
        }

        public AudioDiagnosticReadout GetDiagnosticReadout()
        {
            var activeBuses = _cues.Values.Select(c => c.BusName).Distinct().OrderBy(b => b).ToList();
            return new AudioDiagnosticReadout
            {
                ActiveBuses = activeBuses,
                ActiveDuckingDb = _activeDuckingDb,
                CurrentPresetId = _activePresetId,
                LastNotification = _lastNotification,
                CoalescedAlertCount = _coalescedAlertCount
            };
        }

        private void LoadFallbackCatalog()
        {
            _cues.Clear();
            _cues["cue_alarm_general"] = new AudioCueAccessibilityDef
            {
                CueId = "cue_alarm_general",
                Category = "Alert",
                BusName = "Alert",
                VisualLabel = "[Alarm] General shelter alarm sounding",
                Severity = "Critical",
                DuckLevelDb = -12.0f,
                CoalesceWindowSeconds = 3.0f,
                IconGlyph = "alarm_bell"
            };
            _cues["cue_low_power_warning"] = new AudioCueAccessibilityDef
            {
                CueId = "cue_low_power_warning",
                Category = "Alert",
                BusName = "Alert",
                VisualLabel = "[Power] Battery grid reserves below 15%",
                Severity = "Warning",
                DuckLevelDb = -8.0f,
                CoalesceWindowSeconds = 5.0f,
                IconGlyph = "battery_low"
            };

            _presets.Clear();
            _presets["preset_full_dynamic"] = new AudioMixPresetDef
            {
                PresetId = "preset_full_dynamic",
                DisplayName = "Full Dynamic Range",
                MasterLimiterThresholdDb = 0.0f,
                DialogueDuckingAttenuationDb = -8.0f,
                HighFrequencyAttenuationDb = 0.0f
            };
            _presets["preset_reduced_stimulation"] = new AudioMixPresetDef
            {
                PresetId = "preset_reduced_stimulation",
                DisplayName = "Reduced Stimulation / Quiet",
                MasterLimiterThresholdDb = -6.0f,
                DialogueDuckingAttenuationDb = -14.0f,
                HighFrequencyAttenuationDb = -4.0f
            };
        }
    }

    /// <summary>
    /// Read-only census of live audio-accessibility state (Plan 169). Exposed for
    /// the architecture scanner and the host self-test probe.
    /// </summary>
    public struct AudioAccessibilityCensus
    {
        public int TotalCues { get; }
        public int TotalMixPresets { get; }
        public string ActivePresetId { get; }
        public float ActiveDuckingDb { get; }
        public int CoalescedAlertCount { get; }
        public bool HasEmittedNotification { get; }

        public AudioAccessibilityCensus(
            int totalCues,
            int totalMixPresets,
            string activePresetId,
            float activeDuckingDb,
            int coalescedAlertCount,
            bool hasEmittedNotification)
        {
            TotalCues = totalCues;
            TotalMixPresets = totalMixPresets;
            ActivePresetId = activePresetId;
            ActiveDuckingDb = activeDuckingDb;
            CoalescedAlertCount = coalescedAlertCount;
            HasEmittedNotification = hasEmittedNotification;
        }
    }
}
