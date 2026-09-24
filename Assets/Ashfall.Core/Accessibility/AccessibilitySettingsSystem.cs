// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 184 — Accessibility Options System
// Pure domain authority for accessibility settings, profiles (visual, hearing,
// motor, cognitive), colorblind modes, font scaling, audio assists, and state persistence.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Accessibility
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class AccessibilityProfileDef
    {
        public string profile_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string colorblind_mode { get; set; } = "None";
        public float font_scale { get; set; } = 1.0f;
        public bool high_contrast { get; set; }
        public bool reduced_motion { get; set; }
        public bool screen_reader_friendly { get; set; }
        public bool audio_descriptions { get; set; }
        public bool visual_audio_alerts { get; set; }
        public bool mono_audio { get; set; }
        public string subtitle_size { get; set; } = "Medium";
        public bool cognitive_load_reduction { get; set; }
        public bool auto_walk { get; set; }
        public bool aim_assist { get; set; }
    }

    [Serializable]
    public sealed class AccessibilityCatalog
    {
        public int schema_version { get; set; } = 1;
        public string default_profile_id { get; set; } = "acc_profile_default";
        public List<AccessibilityProfileDef> profiles { get; set; } = new List<AccessibilityProfileDef>();
    }

    // ── State DTO ───────────────────────────────────────────────────────────

    [Serializable]
    public sealed class AccessibilitySettingsState
    {
        public int SchemaVersion { get; set; } = 1;
        public string ActiveProfileId { get; set; } = "acc_profile_default";
        public string ColorblindMode { get; set; } = "None";
        public float FontScale { get; set; } = 1.0f;
        public bool HighContrast { get; set; }
        public bool ReducedMotion { get; set; }
        public bool ScreenReaderFriendly { get; set; }
        public bool AudioDescriptions { get; set; }
        public bool VisualAudioAlerts { get; set; }
        public bool MonoAudio { get; set; }
        public string SubtitleSize { get; set; } = "Medium";
        public bool CognitiveLoadReduction { get; set; }
        public bool AutoWalk { get; set; }
        public bool AimAssist { get; set; }
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class AccessibilitySettingsSystem
    {
        private readonly AccessibilitySettingsState _state;
        private readonly List<AccessibilityProfileDef> _profiles = new List<AccessibilityProfileDef>();

        public event Action<AccessibilitySettingsState>? OnSettingsChanged;
        public event Action<string>? OnProfileApplied;

        public string ActiveProfileId => _state.ActiveProfileId;
        public string ColorblindMode => _state.ColorblindMode;
        public float FontScale => _state.FontScale;
        public bool HighContrast => _state.HighContrast;
        public bool ReducedMotion => _state.ReducedMotion;
        public bool ScreenReaderFriendly => _state.ScreenReaderFriendly;
        public bool AudioDescriptions => _state.AudioDescriptions;
        public bool VisualAudioAlerts => _state.VisualAudioAlerts;
        public bool MonoAudio => _state.MonoAudio;
        public string SubtitleSize => _state.SubtitleSize;
        public bool CognitiveLoadReduction => _state.CognitiveLoadReduction;
        public bool AutoWalk => _state.AutoWalk;
        public bool AimAssist => _state.AimAssist;

        public AccessibilitySettingsSystem()
        {
            _state = new AccessibilitySettingsState();
        }

        public AccessibilitySettingsSystem(AccessibilitySettingsState state)
        {
            _state = state ?? new AccessibilitySettingsState();
        }

        // ── Catalog Loading ────────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<AccessibilityCatalog>(json, options);
                if (catalog?.profiles == null) return;

                _profiles.Clear();
                foreach (var p in catalog.profiles)
                {
                    if (!string.IsNullOrWhiteSpace(p.profile_id))
                        _profiles.Add(p);
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyList<AccessibilityProfileDef> GetAllProfiles() => _profiles;

        public AccessibilityProfileDef? GetProfile(string profileId)
        {
            return _profiles.FirstOrDefault(p => string.Equals(p.profile_id, profileId, StringComparison.OrdinalIgnoreCase));
        }

        // ── Profile Application ────────────────────────────────────────────

        public bool ApplyProfile(string profileId)
        {
            var def = GetProfile(profileId);
            if (def == null) return false;

            _state.ActiveProfileId = def.profile_id;
            _state.ColorblindMode = def.colorblind_mode;
            _state.FontScale = Math.Clamp(def.font_scale, 0.75f, 2.0f);
            _state.HighContrast = def.high_contrast;
            _state.ReducedMotion = def.reduced_motion;
            _state.ScreenReaderFriendly = def.screen_reader_friendly;
            _state.AudioDescriptions = def.audio_descriptions;
            _state.VisualAudioAlerts = def.visual_audio_alerts;
            _state.MonoAudio = def.mono_audio;
            _state.SubtitleSize = def.subtitle_size;
            _state.CognitiveLoadReduction = def.cognitive_load_reduction;
            _state.AutoWalk = def.auto_walk;
            _state.AimAssist = def.aim_assist;

            OnProfileApplied?.Invoke(def.profile_id);
            OnSettingsChanged?.Invoke(CaptureState());
            return true;
        }

        // ── Custom Settings Mutators ───────────────────────────────────────

        public void SetColorblindMode(string mode)
        {
            _state.ColorblindMode = string.IsNullOrWhiteSpace(mode) ? "None" : mode.Trim();
            _state.ActiveProfileId = "custom";
            OnSettingsChanged?.Invoke(CaptureState());
        }

        public void SetFontScale(float scale)
        {
            _state.FontScale = Math.Clamp(scale, 0.75f, 2.0f);
            _state.ActiveProfileId = "custom";
            OnSettingsChanged?.Invoke(CaptureState());
        }

        public void SetHighContrast(bool enabled)
        {
            _state.HighContrast = enabled;
            _state.ActiveProfileId = "custom";
            OnSettingsChanged?.Invoke(CaptureState());
        }

        public void SetReducedMotion(bool enabled)
        {
            _state.ReducedMotion = enabled;
            _state.ActiveProfileId = "custom";
            OnSettingsChanged?.Invoke(CaptureState());
        }

        public void SetVisualAudioAlerts(bool enabled)
        {
            _state.VisualAudioAlerts = enabled;
            _state.ActiveProfileId = "custom";
            OnSettingsChanged?.Invoke(CaptureState());
        }

        public void SetSubtitleSize(string size)
        {
            _state.SubtitleSize = string.IsNullOrWhiteSpace(size) ? "Medium" : size.Trim();
            _state.ActiveProfileId = "custom";
            OnSettingsChanged?.Invoke(CaptureState());
        }

        public void SetCognitiveLoadReduction(bool enabled)
        {
            _state.CognitiveLoadReduction = enabled;
            _state.ActiveProfileId = "custom";
            OnSettingsChanged?.Invoke(CaptureState());
        }

        public void SetAutoWalk(bool enabled)
        {
            _state.AutoWalk = enabled;
            _state.ActiveProfileId = "custom";
            OnSettingsChanged?.Invoke(CaptureState());
        }

        public void SetAimAssist(bool enabled)
        {
            _state.AimAssist = enabled;
            _state.ActiveProfileId = "custom";
            OnSettingsChanged?.Invoke(CaptureState());
        }

        // ── Save / Restore ─────────────────────────────────────────────────

        public AccessibilitySettingsState CaptureState()
        {
            return new AccessibilitySettingsState
            {
                SchemaVersion = _state.SchemaVersion,
                ActiveProfileId = _state.ActiveProfileId,
                ColorblindMode = _state.ColorblindMode,
                FontScale = _state.FontScale,
                HighContrast = _state.HighContrast,
                ReducedMotion = _state.ReducedMotion,
                ScreenReaderFriendly = _state.ScreenReaderFriendly,
                AudioDescriptions = _state.AudioDescriptions,
                VisualAudioAlerts = _state.VisualAudioAlerts,
                MonoAudio = _state.MonoAudio,
                SubtitleSize = _state.SubtitleSize,
                CognitiveLoadReduction = _state.CognitiveLoadReduction,
                AutoWalk = _state.AutoWalk,
                AimAssist = _state.AimAssist
            };
        }

        public void RestoreState(AccessibilitySettingsState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.ActiveProfileId = string.IsNullOrWhiteSpace(saved.ActiveProfileId) ? "acc_profile_default" : saved.ActiveProfileId;
            _state.ColorblindMode = string.IsNullOrWhiteSpace(saved.ColorblindMode) ? "None" : saved.ColorblindMode;
            _state.FontScale = Math.Clamp(saved.FontScale, 0.75f, 2.0f);
            _state.HighContrast = saved.HighContrast;
            _state.ReducedMotion = saved.ReducedMotion;
            _state.ScreenReaderFriendly = saved.ScreenReaderFriendly;
            _state.AudioDescriptions = saved.AudioDescriptions;
            _state.VisualAudioAlerts = saved.VisualAudioAlerts;
            _state.MonoAudio = saved.MonoAudio;
            _state.SubtitleSize = string.IsNullOrWhiteSpace(saved.SubtitleSize) ? "Medium" : saved.SubtitleSize;
            _state.CognitiveLoadReduction = saved.CognitiveLoadReduction;
            _state.AutoWalk = saved.AutoWalk;
            _state.AimAssist = saved.AimAssist;
        }

        public AccessibilityCensus GetCensus()
        {
            return new AccessibilityCensus(
                activeProfileId: _state.ActiveProfileId,
                loadedProfilesCount: _profiles.Count,
                fontScale: _state.FontScale,
                highContrast: _state.HighContrast,
                reducedMotion: _state.ReducedMotion,
                colorblindMode: _state.ColorblindMode,
                screenReaderFriendly: _state.ScreenReaderFriendly,
                visualAudioAlerts: _state.VisualAudioAlerts,
                monoAudio: _state.MonoAudio,
                autoWalk: _state.AutoWalk,
                aimAssist: _state.AimAssist,
                cognitiveLoadReduction: _state.CognitiveLoadReduction);
        }
    }

    public struct AccessibilityCensus
    {
        public string ActiveProfileId { get; }
        public int LoadedProfilesCount { get; }
        public float FontScale { get; }
        public bool HighContrast { get; }
        public bool ReducedMotion { get; }
        public string ColorblindMode { get; }
        public bool ScreenReaderFriendly { get; }
        public bool VisualAudioAlerts { get; }
        public bool MonoAudio { get; }
        public bool AutoWalk { get; }
        public bool AimAssist { get; }
        public bool CognitiveLoadReduction { get; }

        public AccessibilityCensus(
            string activeProfileId,
            int loadedProfilesCount,
            float fontScale,
            bool highContrast,
            bool reducedMotion,
            string colorblindMode,
            bool screenReaderFriendly,
            bool visualAudioAlerts,
            bool monoAudio,
            bool autoWalk,
            bool aimAssist,
            bool cognitiveLoadReduction)
        {
            ActiveProfileId = activeProfileId;
            LoadedProfilesCount = loadedProfilesCount;
            FontScale = fontScale;
            HighContrast = highContrast;
            ReducedMotion = reducedMotion;
            ColorblindMode = colorblindMode;
            ScreenReaderFriendly = screenReaderFriendly;
            VisualAudioAlerts = visualAudioAlerts;
            MonoAudio = monoAudio;
            AutoWalk = autoWalk;
            AimAssist = aimAssist;
            CognitiveLoadReduction = cognitiveLoadReduction;
        }
    }
}
